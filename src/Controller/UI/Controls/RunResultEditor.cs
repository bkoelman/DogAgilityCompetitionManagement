using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine;
using DogAgilityCompetition.Controller.Engine.Storage;
using DogAgilityCompetition.WinForms;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary>
    /// Enables editing the previous run result while another run is active.
    /// </summary>
    public sealed partial class RunResultEditor : UserControl
    {
        [NotNull]
        [ItemNotNull]
        private readonly Lazy<IEnumerable<Control>> allChildControls;

        [CanBeNull]
        private CompetitionRunResult originalRunVersion;

        private int messageHighlightCount;

        private bool HasValidationErrors
        {
            get
            {
                return allChildControls.Value.Any(control => !string.IsNullOrEmpty(errorProvider.GetError(control)));
            }
        }

        private bool CanEdit
        {
            get
            {
                bool canEdit = originalRunVersion != null;

                if (messageHighlighter.IsHighlightEnabled)
                {
                    canEdit = false;
                }

                return canEdit;
            }
        }

        private bool HasChanges => FinishTimeHasChanges || RefusalCountHasChanges || FaultCountHasChanges || EliminatedHasChanges;

        private bool FinishTimeHasChanges
        {
            get
            {
                if (originalRunVersion == null)
                {
                    return false;
                }

                if (!string.IsNullOrEmpty(GetErrorForFinishTimeOnScreen()))
                {
                    return true;
                }

                CompetitionRunTimings screenValue = ConvertFinishTimeFromScreen(false);
                return !EqualitySupport.EqualsWithNulls(originalRunVersion.Timings, screenValue, CompetitionRunTimingsElapsedEqual);
            }
        }

        private bool RefusalCountHasChanges =>
            originalRunVersion != null && (!string.IsNullOrEmpty(GetErrorForRefusalCountOnScreen()) ||
                originalRunVersion.RefusalCount != ConvertRefusalCountFromScreen());

        private bool FaultCountHasChanges =>
            originalRunVersion != null &&
            (!string.IsNullOrEmpty(GetErrorForFaultCountOnScreen()) || originalRunVersion.FaultCount != ConvertFaultCountFromScreen());

        private bool EliminatedHasChanges => originalRunVersion != null && originalRunVersion.IsEliminated != ConvertEliminatedFromScreen();

        public event EventHandler<RunResultChangingEventArgs> RunResultChanging;
        public event EventHandler RunResultChanged;

        public RunResultEditor()
        {
            InitializeComponent();

            BindNumericUpDownToChangeEventHandler(faultsUpDown);
            BindNumericUpDownToChangeEventHandler(refusalsUpDown);

            allChildControls = new Lazy<IEnumerable<Control>>(() => this.GetAllChildControlsRecursive().ToList());

            faultsUpDown.Maximum = CompetitionRunResult.MaxFaultValue;
            faultsUpDown.Increment = CompetitionRunResult.FaultStepSize;

            refusalsUpDown.Maximum = CompetitionRunResult.MaxRefusalsValue;
            refusalsUpDown.Increment = CompetitionRunResult.RefusalStepSize;
        }

        private static bool CompetitionRunTimingsElapsedEqual([NotNull] CompetitionRunTimings firstTimings, [NotNull] CompetitionRunTimings secondTimings)
        {
            return EqualitySupport.EqualsWithNulls(firstTimings.FinishTime, secondTimings.FinishTime, RecordedTimesElapsedEqual);
        }

        private static bool RecordedTimesElapsedEqual([NotNull] RecordedTime firstRecordedTime, [NotNull] RecordedTime secondRecordedTime)
        {
            TimeSpanWithAccuracy elapsed = firstRecordedTime.ElapsedSince(secondRecordedTime);
            return elapsed.TimeValue == TimeSpan.Zero;
        }

        [NotNull]
        private CompetitionRunTimings ConvertFinishTimeFromScreen(bool forceUserEdited)
        {
            TimeSpanWithAccuracy? finishTime = TimeSpanWithAccuracy.FromString(finishTimeTextBox.Text);

            if (finishTime != null && forceUserEdited)
            {
                finishTime = finishTime.Value.ChangeAccuracy(TimeAccuracy.UserEdited);
            }

            CompetitionRunResult originalRunVersionNotNull = AssertOriginalRunVersionNotNull();
            return originalRunVersionNotNull.UpdateFinishTimeFrom(finishTime);
        }

        private int ConvertRefusalCountFromScreen()
        {
            return (int)refusalsUpDown.Value;
        }

        private int ConvertFaultCountFromScreen()
        {
            return (int)faultsUpDown.Value;
        }

        private bool ConvertEliminatedFromScreen()
        {
            return eliminatedCheckBox.Checked;
        }

        private void BindNumericUpDownToChangeEventHandler([NotNull] NumericUpDown control)
        {
            TextBox innerTextBox = control.Controls.OfType<TextBox>().Single();
            innerTextBox.TextChanged += InputField_ValueChanged;
        }

        public void TryUpdateWith([CanBeNull] CompetitionRunResult runResult)
        {
            if (!HasChanges)
            {
                OverwriteWith(runResult);
            }
        }

        public void OverwriteWith([CanBeNull] CompetitionRunResult runResult)
        {
            originalRunVersion = runResult;
            CopyValuesFrom(runResult);
        }

        private void CopyValuesFrom([CanBeNull] CompetitionRunResult runResult)
        {
            errorProvider.Clear();

            SetScreenValueForCompetitor(runResult);
            SetScreenValueForFinishTime(runResult);
            SetScreenValueForFaultCount(runResult);
            SetScreenValueForRefusalCount(runResult);
            SetScreenValueForEliminated(runResult);

            UpdateControlState();
        }

        private void SetScreenValueForCompetitor([CanBeNull] CompetitionRunResult runResult)
        {
            competitorTextBox.Text = runResult == null
                ? string.Empty
                : runResult.Competitor.Number + " - " + runResult.Competitor.HandlerName + " - " + runResult.Competitor.DogName;
        }

        private void SetScreenValueForFinishTime([CanBeNull] CompetitionRunResult runResult)
        {
            string text = string.Empty;

            if (runResult?.Timings?.FinishTime != null)
            {
                TimeSpanWithAccuracy finishTime = runResult.Timings.FinishTime.ElapsedSince(runResult.Timings.StartTime);
                text = finishTime.ToString();
            }

            finishTimeTextBox.Text = text;
        }

        private void SetScreenValueForFaultCount([CanBeNull] CompetitionRunResult runResult)
        {
            if (runResult == null)
            {
                faultsUpDown.Text = string.Empty;
            }
            else
            {
                faultsUpDown.Value = runResult.FaultCount;
                faultsUpDown.Text = runResult.FaultCount.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void SetScreenValueForRefusalCount([CanBeNull] CompetitionRunResult runResult)
        {
            if (runResult == null)
            {
                refusalsUpDown.Text = string.Empty;
            }
            else
            {
                refusalsUpDown.Value = runResult.RefusalCount;
                refusalsUpDown.Text = runResult.RefusalCount.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void SetScreenValueForEliminated([CanBeNull] CompetitionRunResult runResult)
        {
            eliminatedCheckBox.Checked = runResult is { IsEliminated: true };
        }

        private void FinishTimeTextBox_Validating([CanBeNull] object sender, [NotNull] CancelEventArgs e)
        {
            string errorText = GetErrorForFinishTimeOnScreen();
            errorProvider.SetError(finishTimeTextBox, errorText);
        }

        [NotNull]
        private string GetErrorForFinishTimeOnScreen()
        {
            try
            {
                TimeSpanWithAccuracy.FromString(finishTimeTextBox.Text);
            }
            catch (FormatException ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }

        private void FaultsUpDown_Validating([CanBeNull] object sender, [NotNull] CancelEventArgs e)
        {
            string errorText = GetErrorForFaultCountOnScreen();
            errorProvider.SetError(faultsUpDown, errorText);
        }

        [NotNull]
        private string GetErrorForFaultCountOnScreen()
        {
            try
            {
                CompetitionRunResult.AssertFaultCountIsValid((int)faultsUpDown.Value);
            }
            catch (ArgumentException ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }

        private void RefusalsUpDown_Validating([CanBeNull] object sender, [NotNull] CancelEventArgs e)
        {
            string errorText = GetErrorForRefusalCountOnScreen();
            errorProvider.SetError(refusalsUpDown, errorText);
        }

        [NotNull]
        private string GetErrorForRefusalCountOnScreen()
        {
            try
            {
                CompetitionRunResult.AssertRefusalCountIsValid((int)refusalsUpDown.Value);
            }
            catch (ArgumentException ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }

        private void AcceptButton_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            if (ParentForm == null || HasValidationErrors)
            {
                return;
            }

            CompetitionRunResult originalRunVersionNotNull = AssertOriginalRunVersionNotNull();

            // @formatter:keep_existing_linebreaks true

            CompetitionRunResult newRunVersion = originalRunVersionNotNull
                .ChangeTimings(ConvertFinishTimeFromScreen(true))
                .ChangeFaultCount(ConvertFaultCountFromScreen())
                .ChangeRefusalCount(ConvertRefusalCountFromScreen())
                .ChangeIsEliminated(ConvertEliminatedFromScreen());

            // @formatter:keep_existing_linebreaks restore

            var changingEventArgs = new RunResultChangingEventArgs(originalRunVersionNotNull, newRunVersion);
            RunResultChanging?.Invoke(this, changingEventArgs);

            if (changingEventArgs.ErrorMessage == null)
            {
                ShowAnimatedSuccess("Competitor updated.");

                originalRunVersion = CacheManager.DefaultInstance.ActiveModel.GetRunResultFor(newRunVersion.Competitor.Number);
                RunResultChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                ShowAnimatedFailure(changingEventArgs.ErrorMessage);
            }
        }

        [AssertionMethod]
        [NotNull]
        private CompetitionRunResult AssertOriginalRunVersionNotNull()
        {
            return Assertions.InternalValueIsNotNull(() => originalRunVersion, () => originalRunVersion);
        }

        private void ShowAnimatedSuccess([NotNull] string message)
        {
            messageLabel.Visible = true;
            messageLabel.ForeColor = Color.DarkGreen;
            StartHighlightMessage(message, Color.Lime);
        }

        private void ShowAnimatedFailure([NotNull] string message)
        {
            messageLabel.Visible = true;
            messageLabel.ForeColor = Color.DarkRed;
            StartHighlightMessage(message, Color.Red);
        }

        private void StartHighlightMessage([NotNull] string message, Color highlightColor)
        {
            messageLabel.Text = message;

            messageHighlightCount = 0;
            messageHighlighter.HighlightColor = highlightColor;
            messageHighlighter.IsHighlightEnabled = true;
            UpdateControlState();
        }

        private void MessageHighlighter_HighlightCycleFinished([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            messageHighlightCount++;

            if (messageHighlightCount == 2)
            {
                messageLabel.Visible = false;
                messageHighlighter.IsHighlightEnabled = false;
                UpdateControlState();
            }
        }

        private void RevertButton_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            CompetitionRunResult originalRunVersionNotNull = AssertOriginalRunVersionNotNull();
            CompetitionRunResult activeVersionOrNull = CacheManager.DefaultInstance.ActiveModel.GetRunResultOrNull(originalRunVersionNotNull.Competitor.Number);
            OverwriteWith(activeVersionOrNull);
        }

        private void InputField_ValueChanged([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            UpdateControlState();
        }

        private void UpdateControlState()
        {
            bool canEdit = CanEdit;
            bool hasChanges = HasChanges;

            competitorTextBox.Enabled = canEdit;
            finishTimeTextBox.Enabled = canEdit;
            faultsUpDown.Enabled = canEdit;
            refusalsUpDown.Enabled = canEdit;
            eliminatedCheckBox.Enabled = canEdit;

            acceptButton.Enabled = canEdit && hasChanges;
            revertButton.Enabled = canEdit && hasChanges;
            pendingChangeLabel.Visible = hasChanges;
        }
    }
}
