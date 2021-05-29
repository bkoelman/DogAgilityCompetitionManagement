using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using DogAgilityCompetition.Controller.Engine;
using DogAgilityCompetition.Controller.Engine.Storage;
using DogAgilityCompetition.WinForms;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Forms
{
    /// <summary>
    /// Enables configuration of competition class and competitors.
    /// </summary>
    public sealed partial class ClassSetupForm : Form
    {
        [NotNull]
        [ItemNotNull]
        private readonly Lazy<IEnumerable<Control>> allChildControls;

        [NotNull]
        private CompetitionClassModel originalVersion;

        private bool hasImportedCompetitors;

        private bool HasValidationErrors
        {
            get
            {
                return allChildControls.Value.Any(control => !string.IsNullOrEmpty(errorProvider.GetError(control)));
            }
        }

        public ClassSetupForm()
        {
            InitializeComponent();

            allChildControls = new Lazy<IEnumerable<Control>>(() => this.GetAllChildControlsRecursive().ToList());
            originalVersion = CacheManager.DefaultInstance.ActiveModel;
        }

        private void ClassSetupForm_Load([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            competitorsGrid.DataSource = originalVersion.Results;
            gradeTextBox.Text = originalVersion.ClassInfo.Grade;
            classTypeTextBox.Text = originalVersion.ClassInfo.Type;
            inspectorNameTextBox.Text = originalVersion.ClassInfo.InspectorName;
            ringNameTextBox.Text = originalVersion.ClassInfo.RingName;

            standardCourseTimeTextBox.Text = originalVersion.ClassInfo.StandardCourseTime != null
                ? $"{originalVersion.ClassInfo.StandardCourseTime.Value.TotalSeconds:0}"
                : string.Empty;

            maximumCourseTimeTextBox.Text = originalVersion.ClassInfo.MaximumCourseTime != null
                ? $"{originalVersion.ClassInfo.MaximumCourseTime.Value.TotalSeconds:0}"
                : string.Empty;

            trackLengthTextBox.Text = originalVersion.ClassInfo.TrackLengthInMeters?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
            intermediateCountUpDown.Value = originalVersion.IntermediateTimerCount;

            eliminatedPictureAlert.Item = originalVersion.Alerts.Eliminated.Picture;
            firstPlacePictureAlert.Item = originalVersion.Alerts.FirstPlace.Picture;
            cleanRunInSctPictureAlert.Item = originalVersion.Alerts.CleanRunInStandardCourseTime.Picture;
            eliminatedSoundAlert.Item = originalVersion.Alerts.Eliminated.Sound;
            firstPlaceSoundAlert.Item = originalVersion.Alerts.FirstPlace.Sound;
            cleanRunInSctSoundAlert.Item = originalVersion.Alerts.CleanRunInStandardCourseTime.Sound;
            readyToStartSoundAlert.Item = originalVersion.Alerts.ReadyToStart.Sound;
            customItemASoundAlert.Item = originalVersion.Alerts.CustomItemA.Sound;

            if (originalVersion.StartFinishMinDelayForSingleSensor == TimeSpan.Zero)
            {
                startFinishMinDelayUpDown.Value = 1;
                startFinishMinDelayCheckBox.Checked = false;
            }
            else
            {
                startFinishMinDelayUpDown.Value = (decimal)originalVersion.StartFinishMinDelayForSingleSensor.TotalSeconds;
                startFinishMinDelayCheckBox.Checked = true;
            }
        }

        private void ClearButton_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            competitorsGrid.DataSource = new List<CompetitionRunResult>();
        }

        private void ImportButton_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "Select competitors file to import";
                dialog.Filter = "Csv files (*.csv)|*.csv|All files (*.*)|*.*";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var importer = new RunResultsImporter(competitorsGrid.DataSource);

                        IEnumerable<CompetitionRunResult> results = importer.ImportFrom(dialog.FileName,
                            mergeDeletesCheckBox.Checked, discardTimingsCheckBox.Checked);

                        competitorsGrid.DataSource = results;
                        hasImportedCompetitors = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.ToString(), "Import failed - " + Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
#if DEBUG
                    // NICE-TO-HAVE: Remove test code from Debug builds.
                    // International demo names available at: http://www.fakenamegenerator.com/
                    intermediateCountUpDown.Value = 1;

                    competitorsGrid.DataSource = new List<CompetitionRunResult>
                    {
                        new(new Competitor(1, "Tania", "Zita").ChangeCountryCode("NL")),
                        new(new Competitor(2, "кучер", "dziesięć").ChangeCountryCode("RU")),
                        new(new Competitor(3, "Ελληvικά", "Αλκιβιαδης").ChangeCountryCode("GR")),
                        new(new Competitor(4, "Україна", "être").ChangeCountryCode("PL")),
                        new(new Competitor(5, "Annelies", "Seena").ChangeCountryCode("NL")),
                        new(new Competitor(6, "Ben", "Rain").ChangeCountryCode("NL")),
                        new(new Competitor(7, "Feline", "Cadie").ChangeCountryCode("NL")),
                        new(new Competitor(8, "Marissa", "Yentl").ChangeCountryCode("NL")),
                        new(new Competitor(9, "Elly-may", "Benthe").ChangeCountryCode("NL")),
                        new(new Competitor(13, "Alwin", "Mekx").ChangeCountryCode("NL")),
                        new(new Competitor(14, "Wilbert", "Bryce").ChangeCountryCode("NL")),
                        new(new Competitor(20, "Marissa", "Kida").ChangeCountryCode("NL")),
                        new(new Competitor(21, "Margreet", "Blizzard").ChangeCountryCode("NL")),
                        new(new Competitor(24, "Carla", "Jill").ChangeCountryCode("NL")),
                        new(new Competitor(25, "Anouk", "Ikey").ChangeCountryCode("NL")),
                        new(new Competitor(26, "Johan", "Djintie").ChangeCountryCode("NL")),
                        new(new Competitor(27, "Marina", "Jess").ChangeCountryCode("NL"))
                    };

                    hasImportedCompetitors = true;

                    gradeTextBox.Text = "B1 small";
                    classTypeTextBox.Text = "Agility";
                    inspectorNameTextBox.Text = "John Doe";
                    ringNameTextBox.Text = "R1";
                    standardCourseTimeTextBox.Text = "45";
                    maximumCourseTimeTextBox.Text = "65";
                    trackLengthTextBox.Text = "57";
#endif
                }
            }
        }

        private void StandardCourseTimeTextBox_Validating([CanBeNull] object sender, [NotNull] CancelEventArgs e)
        {
            ValidateNullOrPositiveIntegerTextBox(standardCourseTimeTextBox);
        }

        private void MaximumCourseTimeTextBox_Validating([CanBeNull] object sender, [NotNull] CancelEventArgs e)
        {
            ValidateNullOrPositiveIntegerTextBox(maximumCourseTimeTextBox);
        }

        private void TrackLengthTextBox_Validating([CanBeNull] object sender, [NotNull] CancelEventArgs e)
        {
            ValidateNullOrPositiveIntegerTextBox(trackLengthTextBox);
        }

        private void ValidateNullOrPositiveIntegerTextBox([NotNull] TextBox textBox)
        {
            if (!string.IsNullOrWhiteSpace(textBox.Text) && (!int.TryParse(textBox.Text, out int value) || value < 1))
            {
                errorProvider.SetError(textBox, "Must be zero or higher.");
            }
            else
            {
                errorProvider.SetError(textBox, string.Empty);
            }
        }

        private void StartFinishMinDelayCheckBox_CheckedChanged([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            startFinishMinDelayUpDown.Enabled = startFinishMinDelayCheckBox.Checked;
        }

        private void OkButton_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            if (HasValidationErrors)
            {
                MessageBox.Show(this, "Please correct all invalid input fields first.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // @formatter:keep_existing_linebreaks true

            CompetitionClassModel newVersion = originalVersion
                .ChangeClassInfo(originalVersion.ClassInfo
                    .ChangeGrade(gradeTextBox.Text.Trim())
                    .ChangeType(classTypeTextBox.Text.Trim())
                    .ChangeInspectorName(inspectorNameTextBox.Text.Trim())
                    .ChangeRingName(ringNameTextBox.Text.Trim())
                    .ChangeStandardCourseTime(NullableTimeSpanFromTextBox(standardCourseTimeTextBox))
                    .ChangeMaximumCourseTime(NullableTimeSpanFromTextBox(maximumCourseTimeTextBox))
                    .ChangeTrackLengthInMeters(NullableIntFromTextBox(trackLengthTextBox)))
                .ChangeRunResults(competitorsGrid.DataSource)
                .SafeChangeLastCompletedCompetitorNumber(hasImportedCompetitors
                    ? null
                    : originalVersion.LastCompletedCompetitorNumber)
                .ChangeIntermediateTimerCount((int)intermediateCountUpDown.Value)
                .ChangeStartFinishMinDelayForSingleSensor(startFinishMinDelayCheckBox.Checked
                    ? TimeSpan.FromSeconds((double)startFinishMinDelayUpDown.Value)
                    : TimeSpan.Zero)
                .ChangeAlerts(new CompetitionAlerts(new AlertSource(eliminatedPictureAlert.Item, eliminatedSoundAlert.Item),
                    new AlertSource(firstPlacePictureAlert.Item, firstPlaceSoundAlert.Item),
                    new AlertSource(cleanRunInSctPictureAlert.Item, cleanRunInSctSoundAlert.Item),
                    new AlertSource(AlertPictureSourceItem.None, readyToStartSoundAlert.Item),
                    new AlertSource(AlertPictureSourceItem.None, customItemASoundAlert.Item)))
                .RecalculatePlacements();

            // @formatter:keep_existing_linebreaks restore

            ApplyModelChange(newVersion);

            DialogResult = DialogResult.OK;
        }

        [CanBeNull]
        private static TimeSpan? NullableTimeSpanFromTextBox([NotNull] TextBox source)
        {
            return string.IsNullOrWhiteSpace(source.Text) ? null : TimeSpan.FromSeconds(int.Parse(source.Text.Trim()));
        }

        [CanBeNull]
        private static int? NullableIntFromTextBox([NotNull] TextBox source)
        {
            return string.IsNullOrWhiteSpace(source.Text) ? null : int.Parse(source.Text.Trim());
        }

        private void ApplyModelChange([NotNull] CompetitionClassModel newVersion)
        {
            originalVersion = CacheManager.DefaultInstance.ReplaceModel(newVersion, originalVersion);
        }

        private void ClassSetupForm_FormClosed([CanBeNull] object sender, [NotNull] FormClosedEventArgs e)
        {
            SystemSound.PlayWaveFile(null);
        }
    }
}
