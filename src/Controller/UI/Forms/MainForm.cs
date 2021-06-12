using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Controller;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Controller.Engine;
using DogAgilityCompetition.Controller.Engine.Storage;
using DogAgilityCompetition.Controller.Engine.Visualization;
using DogAgilityCompetition.Controller.Properties;
using DogAgilityCompetition.Controller.UI.Controls;
using DogAgilityCompetition.WinForms;

namespace DogAgilityCompetition.Controller.UI.Forms
{
    /// <summary>
    /// Primary application screen.
    /// </summary>
    public sealed partial class MainForm : FormWithHandleManagement
    {
        private readonly DisposableComponent<CirceControllerSessionManager> sessionManager;
        private readonly NetworkHealthMonitor healthMonitor = new();
        private readonly ClockSynchronizationMonitor clockSynchronizationMonitor = new();
        private readonly DisposableComponent<CompetitionClassController> classController;
        private readonly DeviceActionAdapter actionAdapter = new();
        private readonly NumberEntryFilter numberFilter = new();
        private readonly RemoteKeyTracker keyTracker = new();
        private readonly NetworkSetupForm networkSetupForm;
        private readonly TimerDisplayForm timerDisplayForm = new();
        private readonly WirelessDisplayRunVisualizer wirelessRunVisualizer = new();
        private readonly ClassResultsDisplayForm classResultsDisplayForm = new();
        private readonly CustomDisplayForm customDisplayForm = new();
        private readonly LogForm logForm = new();

        private CompetitionClassRequirements requirements = CompetitionClassRequirements.Default;
        private Process? emulatorProcess;

        private CompetitionClassRequirements Requirements
        {
            get => requirements;
            set
            {
                Guard.NotNull(value, nameof(value));

                requirements = value;
                healthMonitor.SetClassRequirements(requirements);
            }
        }

        public MainForm()
        {
            InitializeComponent();
            EnsureHandleCreated();

            Text += AssemblyReader.GetInformationalVersion();

            // Justification for discard: This registers a callback to the allocated object.
            _ = new DisposableComponent<DisposableHolder>(new DisposableHolder(EmulatorProcessKill), ref components);

            var runVisualizer = new CompositeRunVisualizer(new ICompetitionRunVisualizer[]
            {
                new WinFormsVisualizer(this, competitionStateOverview),
                new WinFormsVisualizer(this, timerDisplayForm),
                wirelessRunVisualizer
            });

            classController =
                new DisposableComponent<CompetitionClassController>(
                    new CompetitionClassController(() => healthMonitor.GetLatest(), clockSynchronizationMonitor, runVisualizer), ref components);

#if DEBUG
            logLinkLabel.Visible = true;
#endif

            emulatorLinkLabel.Visible = HasEmulatorFiles();

            logForm.VisibleChanged += (_, _) => logLinkLabel.Text = logForm.Visible ? "Hide log" : "Show log";

            networkSetupForm = new NetworkSetupForm(this);

            healthMonitor.HealthChanged += (_, e) => this.EnsureOnMainThread(() =>
            {
                UpdateControlsEnabledStateAfterHealthChanged(e.Argument);
                ShowNetworkHealth(e.Argument);
            });

            classController.Component.UnknownErrorDuringSave += (_, e) => this.EnsureOnMainThread(() => ShowError(e.Argument.ToString()));
            classController.Component.Aborted += (_, _) => this.EnsureOnMainThread(ClassControllerOnAborted);
            classController.Component.RankingChanged += (_, e) => this.EnsureOnMainThread(() => ClassControllerOnRankingChanged(e));
            classController.Component.StateTransitioned += (_, e) => this.EnsureOnMainThread(() => stateVisualizer.TransitionTo(e.Argument));

            networkStatusView.SwitchToStatusMode();

            actionAdapter.CommandReceived += (_, e) => classController.Component.HandleCommand(e.Command);
            actionAdapter.GatePassed += (_, e) => classController.Component.HandleGatePassed(e.SensorTime, e.GatePassage, e.Source);

            numberFilter.NotifyCompetitorSelecting += (_, e) => classController.Component.StartCompetitorSelection(e.IsCurrentCompetitor);
            numberFilter.NotifyDigitReceived += (_, e) => classController.Component.ReceiveDigit(e.IsCurrentCompetitor, e.CompetitorNumber);
            numberFilter.NotifyCompetitorSelectCanceled += (_, e) => classController.Component.CompleteCompetitorSelection(e.IsCurrentCompetitor, null);
            numberFilter.NotifyCompetitorSelected += (_, e) => classController.Component.CompleteCompetitorSelection(e.IsCurrentCompetitor, e.CompetitorNumber);
            numberFilter.NotifyUnknownAction += (_, e) => actionAdapter.Adapt(e.Source, e.Key, e.SensorTime);

            keyTracker.ModifierKeyDown += (_, e) => numberFilter.HandleModifierKeyDown(e.Source, e.Modifier);
            keyTracker.KeyDown += (_, e) => numberFilter.HandleKeyDown(e.Source, e.Key, e.SensorTime);
            keyTracker.ModifierKeyUp += (_, e) => numberFilter.HandleModifierKeyUp(e.Source, e.Modifier);
            keyTracker.MissingKey += (_, e) => numberFilter.HandleMissingKey(e.Source, e.SensorTime);

            sessionManager = new DisposableComponent<CirceControllerSessionManager>(new CirceControllerSessionManager(), ref components);
            sessionManager.Component.PacketSending += (_, _) => this.EnsureOnMainThread(() => logForm.PulseOutputLed());
            sessionManager.Component.PacketReceived += (_, _) => this.EnsureOnMainThread(() => logForm.PulseInputLed());

            sessionManager.Component.ConnectionStateChanged += (_, e) =>
            {
                healthMonitor.HandleConnectionStateChanged(e.State);
                this.EnsureOnMainThread(() => networkStatusView.IsConnected = e.State == ControllerConnectionState.Connected);
            };

            sessionManager.Component.DeviceActionReceived += (_, e) => keyTracker.ProcessDeviceAction(e.Argument);

            sessionManager.Component.DeviceTracker.DeviceAdded += (_, e) => healthMonitor.HandleDeviceAdded(e.Argument);
            sessionManager.Component.DeviceTracker.DeviceChanged += (_, e) => healthMonitor.HandleDeviceChanged(e.Argument);
            sessionManager.Component.DeviceTracker.DeviceRemoved += (_, e) => healthMonitor.HandleDeviceRemoved(e.Argument);
            sessionManager.Component.DeviceTracker.MediatorStatusChanged += (_, e) => healthMonitor.HandleMediatorStatusChanged(e.Argument);

            clockSynchronizationMonitor.Initialize(sessionManager.Component);

            sessionManager.Component.DeviceTracker.DeviceAdded += (_, e) => this.EnsureOnMainThread(() => networkStatusView.AddOrUpdate(e.Argument));
            sessionManager.Component.DeviceTracker.DeviceChanged += (_, e) => this.EnsureOnMainThread(() => networkStatusView.AddOrUpdate(e.Argument));
            sessionManager.Component.DeviceTracker.DeviceRemoved += (_, e) => this.EnsureOnMainThread(() => networkStatusView.Remove(e.Argument));

            networkSetupForm.SessionManager = sessionManager.Component;
        }

        private void ShowError(string message)
        {
            MessageBox.Show(this, message, @"Error - " + Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            LoadSettings();

            // @formatter:keep_existing_linebreaks true

            Requirements = Requirements
                .ChangeIntermediateTimerCount(CacheManager.DefaultInstance.ActiveModel.IntermediateTimerCount)
                .ChangeStartFinishMinDelayForSingleSensor(CacheManager.DefaultInstance.ActiveModel.StartFinishMinDelayForSingleSensor);

            // @formatter:keep_existing_linebreaks restore

            ApplyModelChangesToDisplayForms();

            sessionManager.Component.Start();
        }

        private static void LoadSettings()
        {
            Settings.Default.Reload();
        }

        private void MainForm_FormClosed(object? sender, FormClosedEventArgs e)
        {
            Settings.Default.DebugTextBoxMode = TextBoxAppender.Mode.ToString();
            Settings.Default.DebugTextBoxSwitches = TextBoxAppender.Switches.ToString();
            Settings.Default.DebugTextBoxIsFrozen = TextBoxAppender.IsFrozen.ToString();

            Settings.Default.Save();
        }

        private void UpdateControlsEnabledStateAfterHealthChanged(NetworkHealthReport report)
        {
            bool isRunInProgress = report.RunComposition != null;

            startClassButton.Enabled = classController.Component.CanStartClass && activeRunRadioButton.Checked;
            stopClassButton.Enabled = isRunInProgress;

            setupClassButton.Enabled = !isRunInProgress;
            networkSetupButton.Enabled = !isRunInProgress;
            resultsButton.Enabled = !isRunInProgress;

            activeRunRadioButton.Enabled = !isRunInProgress;
            resultsRadioButton.Enabled = !isRunInProgress;
            customRadioButton.Enabled = !isRunInProgress;
            noneRadioButton.Enabled = !isRunInProgress;
        }

        private void SetupClassButton_Click(object? sender, EventArgs e)
        {
            using var form = new ClassSetupForm();

            if (form.ShowDialog() == DialogResult.OK)
            {
                // @formatter:keep_existing_linebreaks true

                Requirements = Requirements
                    .ChangeIntermediateTimerCount(CacheManager.DefaultInstance.ActiveModel.IntermediateTimerCount)
                    .ChangeStartFinishMinDelayForSingleSensor(CacheManager.DefaultInstance.ActiveModel.StartFinishMinDelayForSingleSensor);

                // @formatter:keep_existing_linebreaks restore

                healthMonitor.ForceChanged();

                ApplyModelChangesToDisplayForms();
            }
        }

        private void ApplyModelChangesToDisplayForms()
        {
            CompetitionClassModel modelSnapshot = CacheManager.DefaultInstance.ActiveModel;

            CompetitionRunResult? lastCompleted = modelSnapshot.GetLastCompletedOrNull();
            lastCompletedRunEditor.TryUpdateWith(lastCompleted);

            IReadOnlyCollection<CompetitionRunResult> rankings = modelSnapshot.FilterCompletedAndSortedAscendingByPlacement().Results;
            classResultsDisplayForm.UpdateRankings(rankings);
            classResultsDisplayForm.SetClass(modelSnapshot.ClassInfo);

            stateVisualizer.IntermediateTimerCount = modelSnapshot.IntermediateTimerCount;
        }

        private void ClassControllerOnRankingChanged(RankingChangeEventArgs e)
        {
            classResultsDisplayForm.UpdateRankings(e.Rankings);

            if (e.PreviousRunResult != null)
            {
                lastCompletedRunEditor.OverwriteWith(e.PreviousRunResult);
            }
        }

        private void ResultsButton_Click(object? sender, EventArgs e)
        {
            using var form = new ClassResultsForm();

            if (form.ShowDialog() == DialogResult.OK)
            {
                ApplyModelChangesToDisplayForms();
            }
        }

        private void CustomDisplayButton_Click(object? sender, EventArgs e)
        {
            using var form = new CustomDisplaySetupForm();

            if (form.ShowDialog() == DialogResult.OK)
            {
                customDisplayForm.ReloadConfiguration();
            }
        }

        private void DisplayModeRadioButton_CheckedChanged(object? sender, EventArgs e)
        {
            healthMonitor.ForceChanged();

            if (activeRunRadioButton.Checked)
            {
                ToggleActiveDisplay(timerDisplayForm);
            }
            else if (resultsRadioButton.Checked)
            {
                ToggleActiveDisplay(classResultsDisplayForm);
            }
            else if (customRadioButton.Checked)
            {
                ToggleActiveDisplay(customDisplayForm);
            }
            else
            {
                ToggleActiveDisplay(null);
            }
        }

        private void ToggleActiveDisplay(Form? formToShow)
        {
            List<Form> formsToHide = new Form[]
            {
                timerDisplayForm,
                classResultsDisplayForm,
                customDisplayForm
            }.ToList();

            if (formToShow != null)
            {
                formsToHide.Remove(formToShow);
                MaximizeFormOnAlternateScreen(formToShow);
            }

            foreach (Form form in formsToHide)
            {
                form.Visible = false;
            }
        }

        private void MaximizeFormOnAlternateScreen(Form form)
        {
            if (!form.Visible)
            {
                Screen currentScreen = Screen.FromControl(this);
                Screen? otherScreen = Screen.AllScreens.FirstOrDefault(s => s.DeviceName != currentScreen.DeviceName);

                if (otherScreen != null)
                {
                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = otherScreen.Bounds.Location;
                }

                form.FormBorderStyle = FormBorderStyle.None;
                form.WindowState = FormWindowState.Maximized;
                form.Show();
            }
        }

        private void StartClassButton_Click(object? sender, EventArgs e)
        {
            NetworkComposition compositionSnapshot = networkStatusView.NetworkComposition;
            compositionSnapshot = compositionSnapshot.ChangeRequirements(Requirements);

            // Allow incoming DeviceActions.
            actionAdapter.RunComposition = compositionSnapshot;

            // Causes HealthChanged event to fire, where we update enabled state of controls.
            healthMonitor.SelectRunComposition(compositionSnapshot);

            wirelessRunVisualizer.InitializeRun(sessionManager.Component, compositionSnapshot);

            classController.Component.StartClass(Requirements);
        }

        private void StopClassButton_Click(object? sender, EventArgs e)
        {
            classController.Component.RequestAbort();
        }

        private void ClassControllerOnAborted()
        {
            // Block incoming DeviceActions.
            actionAdapter.RunComposition = NetworkComposition.Empty;

            // Causes HealthChanged event to fire, where we update enabled state of controls.
            healthMonitor.SelectRunComposition(null);
        }

        private void NetworkSetupButton_Click(object? sender, EventArgs e)
        {
            networkSetupForm.ShowDialog();
        }

        private void ShowNetworkHealth(NetworkHealthReport healthReport)
        {
            var messageBuilder = new StringBuilder();

            if (!healthReport.IsConnected)
            {
                if (healthReport.HasProtocolVersionMismatch)
                {
                    messageBuilder.AppendLine("Not connected to mediator (protocol version mismatch).");
                }
                else if (healthReport.MediatorStatus == KnownMediatorStatusCode.MediatorUnconfigured)
                {
                    messageBuilder.AppendLine("Not connected to mediator (mediator is unconfigured).");
                }
                else
                {
                    messageBuilder.AppendLine("Not connected to mediator.");
                }
            }

            if (healthReport.MediatorStatus != KnownMediatorStatusCode.Normal && healthReport.MediatorStatus != KnownMediatorStatusCode.MediatorUnconfigured)
            {
                string statusName = KnownMediatorStatusCode.GetNameFor(healthReport.MediatorStatus);
                messageBuilder.AppendLine($"Mediator reports status {statusName}.");
            }

            foreach (WirelessNetworkAddress deviceAddress in healthReport.MisalignedSensors)
            {
                messageBuilder.AppendLine($"Sensor {deviceAddress} is not aligned.");
            }

            foreach (WirelessNetworkAddress deviceAddress in healthReport.UnsyncedSensors)
            {
                messageBuilder.AppendLine($"Sensor {deviceAddress} is not synchronized.");
            }

            foreach (WirelessNetworkAddress deviceAddress in healthReport.VersionMismatchingSensors)
            {
                messageBuilder.AppendLine($"Sensor {deviceAddress} does not match mediator version.");
            }

            if (healthReport.ClassCompliance != null)
            {
                foreach (NetworkComplianceMismatch mismatch in healthReport.ClassCompliance)
                {
                    messageBuilder.AppendLine(mismatch.Message);
                }
            }

            healthTextBox.Text = messageBuilder.ToString();
        }

        private void LastCompletedRunEditor_RunResultChanging(object? sender, RunResultChangingEventArgs e)
        {
            string? errorMessage = classController.Component.TryUpdateRunResult(e.OriginalRunResult, e.NewRunResult);
            e.ErrorMessage = errorMessage;
        }

        private void LastCompletedRunEditor_RunResultChanged(object? sender, EventArgs e)
        {
            ApplyModelChangesToDisplayForms();
        }

        private void EmulatorLinkLabel_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            if (emulatorProcess == null)
            {
                string? emulatorExePath = GetEmulatorExePath();

                if (emulatorExePath == null)
                {
                    MessageBox.Show("DogAgilityCompetition.MediatorEmulator.exe not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string? emulatorXmlPath = GetEmulatorXmlPath();

                if (emulatorXmlPath == null)
                {
                    MessageBox.Show("RemoteForDebug.xml not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                const int emulatorMargin = 30;
                const int emulatorHeight = 609;
                const int emulatorWidth = 300;

                int xPos = Location.X + Width - emulatorMargin - emulatorWidth;
                int yPos = Location.Y + Height - emulatorMargin - emulatorHeight;

                var startInfo = new ProcessStartInfo
                {
                    FileName = emulatorExePath,
                    Arguments = $"\"{emulatorXmlPath}\" transparentontop pos={yPos}x{xPos} size={emulatorHeight}x{emulatorWidth}"
                };

                emulatorProcess = Process.Start(startInfo);

                if (emulatorProcess != null)
                {
                    emulatorProcess.EnableRaisingEvents = true;
                    emulatorProcess.Exited += EmulatorProcessOnExited;
                }

                emulatorLinkLabel.Text = @"Turn emulator off";
            }
            else
            {
                EmulatorProcessKill();
            }
        }

        private static bool HasEmulatorFiles()
        {
            return GetEmulatorExePath() != null && GetEmulatorXmlPath() != null;
        }

        private static string? GetEmulatorExePath()
        {
            string emulatorExePath = GetPathForFileInApplicationFolder("DogAgilityCompetition.MediatorEmulator.exe");

            return File.Exists(emulatorExePath) ? emulatorExePath : null;
        }

        private static string? GetEmulatorXmlPath()
        {
            string emulatorXmlPath = GetPathForFileInApplicationFolder("RemoteForDebug.xml");
            return File.Exists(emulatorXmlPath) ? emulatorXmlPath : null;
        }

        private static string GetPathForFileInApplicationFolder(string fileName)
        {
            string applicationPath = AppContext.BaseDirectory;
            string applicationFolder = Path.GetDirectoryName(applicationPath)!;
            return Path.GetFullPath(Path.Combine(applicationFolder, fileName));
        }

        private void EmulatorProcessOnExited(object? sender, EventArgs eventArgs)
        {
            this.EnsureOnMainThread(() =>
            {
                if (emulatorProcess != null)
                {
                    emulatorProcess.Dispose();
                    emulatorProcess = null;
                }

                emulatorLinkLabel.Text = @"Turn emulator on";
            });
        }

        private void EmulatorProcessKill()
        {
            if (emulatorProcess is { HasExited: false })
            {
                emulatorProcess.Kill();
            }
        }

        private void LogLinkLabel_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            logForm.Visible = !logForm.Visible;
        }
    }
}
