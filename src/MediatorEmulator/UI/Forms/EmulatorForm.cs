using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Mediator;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.MediatorEmulator.Engine;
using DogAgilityCompetition.MediatorEmulator.Engine.Storage;
using DogAgilityCompetition.MediatorEmulator.Engine.Storage.Serialization;
using DogAgilityCompetition.WinForms;
using JetBrains.Annotations;

namespace DogAgilityCompetition.MediatorEmulator.UI.Forms
{
    /// <summary>
    /// The MDI parent that hosts all devices in the emulated wireless network.
    /// </summary>
    public sealed partial class EmulatorForm : Form
    {
        [NotNull]
        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [NotNull]
        private readonly StartupArguments startupArguments;

        [NotNull]
        private readonly DisposableComponent<CirceMediatorSessionManager> sessionManager;

        [NotNull]
        private readonly RandomSettingsGenerator settingsGenerator = new();

        [NotNull]
        private readonly MostRecentlyUsedContainer mruContainer = RegistrySettingsProvider.GetMruList();

        [NotNull]
        private NetworkConfigurationFile file = new();

        [NotNull]
        private MediatorForm mediatorForm;

        public EmulatorForm([NotNull] StartupArguments startupArguments)
        {
            this.startupArguments = startupArguments;
            InitializeComponent();

            Text += AssemblyReader.GetInformationalVersion();

            sessionManager = new DisposableComponent<CirceMediatorSessionManager>(new CirceMediatorSessionManager(), ref components);
        }

        private void EmulatorForm_Load([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            ApplyLayoutFromStartupArguments();

            CreateMdiChildrenFromFile();

            if (startupArguments.Path != null)
            {
                OpenFromFile(startupArguments.Path);
            }
        }

        private void ApplyLayoutFromStartupArguments()
        {
            if (startupArguments.HasLayout)
            {
                if (startupArguments.Location != null)
                {
                    Location = startupArguments.Location.Value;
                }

                if (startupArguments.Size != null)
                {
                    Size = startupArguments.Size.Value;
                }

                if (startupArguments.TransparentOnTop)
                {
                    Opacity = 0.9;
                    TopMost = true;
                }

                WindowState = startupArguments.State;
            }
            else
            {
                MaximizeOnSecondaryScreen();
            }
        }

        private void MaximizeOnSecondaryScreen()
        {
            Screen secondaryScreen = Screen.AllScreens.FirstOrDefault(s => !s.Primary);

            if (secondaryScreen != null)
            {
                StartPosition = FormStartPosition.Manual;
                Location = secondaryScreen.Bounds.Location;
            }

            WindowState = FormWindowState.Maximized;
        }

        private void CreateMdiChildrenFromFile()
        {
            CreateMediatorForm(file.Configuration.MediatorOrDefault);

            foreach (GateSettingsXml gateSettings in file.Configuration.GatesOrEmpty)
            {
                CreateGateForm(gateSettings);
            }

            foreach (RemoteSettingsXml remoteSettings in file.Configuration.RemotesOrEmpty)
            {
                CreateRemoteForm(remoteSettings);
            }

            foreach (DisplaySettingsXml displaySettings in file.Configuration.DisplaysOrEmpty)
            {
                CreateDisplayForm(displaySettings);
            }
        }

        private void CreateMediatorForm([NotNull] MediatorSettingsXml settings)
        {
            mediatorForm = new MediatorForm(settings, file.Configuration.IsMaximized, sessionManager.Component)
            {
                MdiParent = this
            };

            mediatorForm.WindowStateChanging += MdiFormOnWindowStateChanging;
            mediatorForm.Show();
            Log.Info("Added Mediator.");
        }

        private void MdiFormOnWindowStateChanging([CanBeNull] object sender, [NotNull] WindowStateChangingEventArgs e)
        {
            var deviceForm = sender as Form;

            if (deviceForm != null && deviceForm.WindowState == FormWindowState.Maximized)
            {
                file.Configuration.IsMaximized = false;
            }
            else if (e.NewState == FormWindowState.Maximized)
            {
                file.Configuration.IsMaximized = true;
            }
        }

        private void CreateGateForm([NotNull] GateSettingsXml settings)
        {
            var form = new GateForm(settings, file.Configuration.IsMaximized, sessionManager.Component)
            {
                MdiParent = this
            };

            form.DeviceRemoved += DeviceFormOnDeviceRemoved;
            form.WindowStateChanging += MdiFormOnWindowStateChanging;
            form.Show();
            Log.Info($"Added Gate {settings.DeviceAddressNotNull}.");
        }

        private void DeviceFormOnDeviceRemoved([CanBeNull] object sender, [NotNull] EventArgs<WirelessNetworkAddress> e)
        {
            file.Configuration.RemoveDevice(e.Argument);
            Log.Info($"Removed {e.Argument}.");
        }

        private void CreateRemoteForm([NotNull] RemoteSettingsXml settings)
        {
            var form = new RemoteForm(settings, file.Configuration.IsMaximized, sessionManager.Component)
            {
                MdiParent = this
            };

            form.DeviceRemoved += DeviceFormOnDeviceRemoved;
            form.WindowStateChanging += MdiFormOnWindowStateChanging;
            form.Show();
            Log.Info($"Added Remote {settings.DeviceAddressNotNull}.");
        }

        private void CreateDisplayForm([NotNull] DisplaySettingsXml settings)
        {
            var form = new DisplayForm(settings, file.Configuration.IsMaximized, sessionManager.Component)
            {
                MdiParent = this
            };

            form.DeviceRemoved += DeviceFormOnDeviceRemoved;
            form.WindowStateChanging += MdiFormOnWindowStateChanging;
            form.Show();
            Log.Info($"Added Display {settings.DeviceAddressNotNull}.");
        }

        private void GateToolStripMenuItem_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            var settings = new GateSettingsXml
            {
                SignalStrength = settingsGenerator.GetSignalStrength(),
                BatteryStatus = settingsGenerator.GetBatteryStatus()
            };

            file.Configuration.GatesOrEmpty.Add(settings);
            CreateGateForm(settings);
        }

        private void RemoteToolStripMenuItem_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            var settings = new RemoteSettingsXml
            {
                SignalStrength = settingsGenerator.GetSignalStrength(),
                BatteryStatus = settingsGenerator.GetBatteryStatus(),
                Features = RemoteEmulatorFeatures.All
            };

            file.Configuration.RemotesOrEmpty.Add(settings);
            CreateRemoteForm(settings);
        }

        private void DisplayToolStripMenuItem_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            var settings = new DisplaySettingsXml
            {
                SignalStrength = settingsGenerator.GetSignalStrength(),
                BatteryStatus = settingsGenerator.GetBatteryStatus()
            };

            file.Configuration.DisplaysOrEmpty.Add(settings);
            CreateDisplayForm(settings);
        }

        private void OpenToolStripMenuItem_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "Open network configuration";
                dialog.Filter = "Network Configuration Files (*.xml)|*.xml|All Files (*.*)|*.*";

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    OpenFromFile(dialog.FileName);
                }
            }
        }

        private bool OpenFromFile([NotNull] string path)
        {
            Guard.NotNullNorEmpty(path, nameof(path));

            NetworkConfigurationFile newFile;

            try
            {
                newFile = NetworkConfigurationFile.Load(path);
            }
            catch (Exception ex)
            {
                string message = $"Failed to load network configuration from file:\n\n{path}\n\n{ex.Message}";
                MessageBox.Show(this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            CloseMdiChildren(true);
            file = newFile;
            CreateMdiChildrenFromFile();

            mruContainer.MarkAsUsed(Path.GetFullPath(path));
            return true;
        }

        private void CloseMdiChildren(bool includeMediator)
        {
            if (includeMediator)
            {
                mediatorForm.CloseWithOverride();
                Log.Info("Removed Mediator.");
            }

            Form[] childrenNotMediator = MdiChildren.Where(form => form != mediatorForm).ToArray();

            foreach (Form child in childrenNotMediator)
            {
                child.Close();
            }
        }

        private void SaveToolStripMenuItem_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            if (file.FilePath == null)
            {
                SaveAsToolStripMenuItem_Click(this, EventArgs.Empty);
            }
            else
            {
                SaveFile(file.FilePath);
            }
        }

        private void SaveAsToolStripMenuItem_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Title = "Save network configuration";
                dialog.Filter = "Network Configuration Files (*.xml)|*.xml|All Files (*.*)|*.*";

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    SaveFile(dialog.FileName);
                }
            }
        }

        private void SaveFile([NotNull] string path)
        {
            string absolutePath = Path.GetFullPath(path);

            file.Configuration.IsMaximized = MdiChildren.Any(child => child.WindowState == FormWindowState.Maximized);
            mruContainer.MarkAsUsed(absolutePath);
            file.SaveAs(absolutePath);
        }

        private void CascadeToolStripMenuItem_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileHorizontalToolStripMenuItem_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void TileVerticalToolStripMenuItem_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void ArrangeToolStripMenuItem_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void MediatorToolStripMenuItem_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            mediatorForm.BringToFront();
        }

        private void MinimizeAllWindowsToolStripMenuItem_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            foreach (Form child in MdiChildren)
            {
                child.WindowState = FormWindowState.Minimized;
            }
        }

        private void CloseAllWindowsToolStripMenuItem_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            CloseMdiChildren(false);
        }

        private void FileToolStripMenuItem_DropDownOpening([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            foreach (ToolStripItem mruMenuItem in recentFilesToolStripMenuItem.DropDownItems)
            {
                mruMenuItem.Click -= MruMenuItemOnClick;
            }

            recentFilesToolStripMenuItem.DropDownItems.Clear();

            foreach (string path in mruContainer.Items)
            {
                var mruMenuItem = new ToolStripMenuItem(Path.GetFileName(path))
                {
                    Tag = path
                };

                mruMenuItem.Click += MruMenuItemOnClick;

                recentFilesToolStripMenuItem.DropDownItems.Add(mruMenuItem);
            }

            recentFilesToolStripMenuItem.Enabled = mruContainer.Items.Count > 0;
        }

        private void MruMenuItemOnClick([CanBeNull] object sender, [NotNull] EventArgs eventArgs)
        {
            var mruMenuItem = (ToolStripMenuItem)sender;

            if (mruMenuItem != null)
            {
                string path = (string)mruMenuItem.Tag;

                if (!OpenFromFile(path))
                {
                    mruContainer.Remove(path);
                }
            }
        }

        private void EmulatorForm_FormClosing([CanBeNull] object sender, [NotNull] EventArgs eventArgs)
        {
            RegistrySettingsProvider.SaveMruList(mruContainer);
        }
    }
}
