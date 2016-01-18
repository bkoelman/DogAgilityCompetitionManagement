using System.ComponentModel;
using System.Windows.Forms;
using DogAgilityCompetition.Controller.UI.Controls;

namespace DogAgilityCompetition.Controller.UI.Forms
{
    partial class ClassSetupForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClassSetupForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.evenlySplitContainer1 = new DogAgilityCompetition.Controller.UI.Controls.EvenlySplitContainer();
            this.gradeTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.ringNameTextBox = new System.Windows.Forms.TextBox();
            this.classTypeTextBox = new System.Windows.Forms.TextBox();
            this.inspectorNameTextBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.standardCourseTimeTextBox = new System.Windows.Forms.TextBox();
            this.maximumCourseTimeTextBox = new System.Windows.Forms.TextBox();
            this.intermediateCountUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.trackLengthTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.startFinishMinDelayCheckBox = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.startFinishMinDelayUpDown = new System.Windows.Forms.NumericUpDown();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.importButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.discardTimingsCheckBox = new System.Windows.Forms.CheckBox();
            this.mergeDeletesCheckBox = new System.Windows.Forms.CheckBox();
            this.clearButton = new System.Windows.Forms.Button();
            this.competitorsGrid = new DogAgilityCompetition.Controller.UI.Controls.CompetitionRunResultsGrid();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.evenlySplitContainer2 = new DogAgilityCompetition.Controller.UI.Controls.EvenlySplitContainer();
            this.cleanRunInSctPictureAlert = new DogAgilityCompetition.Controller.UI.Controls.CompetitionPictureAlert();
            this.eliminatedPictureAlert = new DogAgilityCompetition.Controller.UI.Controls.CompetitionPictureAlert();
            this.firstPlacePictureAlert = new DogAgilityCompetition.Controller.UI.Controls.CompetitionPictureAlert();
            this.customItemASoundAlert = new DogAgilityCompetition.Controller.UI.Controls.CompetitionSoundAlert();
            this.readyToStartSoundAlert = new DogAgilityCompetition.Controller.UI.Controls.CompetitionSoundAlert();
            this.cleanRunInSctSoundAlert = new DogAgilityCompetition.Controller.UI.Controls.CompetitionSoundAlert();
            this.eliminatedSoundAlert = new DogAgilityCompetition.Controller.UI.Controls.CompetitionSoundAlert();
            this.firstPlaceSoundAlert = new DogAgilityCompetition.Controller.UI.Controls.CompetitionSoundAlert();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.evenlySplitContainer1)).BeginInit();
            this.evenlySplitContainer1.Panel1.SuspendLayout();
            this.evenlySplitContainer1.Panel2.SuspendLayout();
            this.evenlySplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.intermediateCountUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startFinishMinDelayUpDown)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.evenlySplitContainer2)).BeginInit();
            this.evenlySplitContainer2.Panel1.SuspendLayout();
            this.evenlySplitContainer2.Panel2.SuspendLayout();
            this.evenlySplitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.evenlySplitContainer1);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.startFinishMinDelayCheckBox);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.startFinishMinDelayUpDown);
            this.groupBox1.Location = new System.Drawing.Point(12, 239);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(877, 160);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Class information";
            // 
            // evenlySplitContainer1
            // 
            this.evenlySplitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.evenlySplitContainer1.IsSplitterFixed = true;
            this.evenlySplitContainer1.Location = new System.Drawing.Point(6, 19);
            this.evenlySplitContainer1.Name = "evenlySplitContainer1";
            // 
            // evenlySplitContainer1.Panel1
            // 
            this.evenlySplitContainer1.Panel1.Controls.Add(this.gradeTextBox);
            this.evenlySplitContainer1.Panel1.Controls.Add(this.label10);
            this.evenlySplitContainer1.Panel1.Controls.Add(this.label13);
            this.evenlySplitContainer1.Panel1.Controls.Add(this.ringNameTextBox);
            this.evenlySplitContainer1.Panel1.Controls.Add(this.classTypeTextBox);
            this.evenlySplitContainer1.Panel1.Controls.Add(this.inspectorNameTextBox);
            this.evenlySplitContainer1.Panel1.Controls.Add(this.label12);
            this.evenlySplitContainer1.Panel1.Controls.Add(this.label11);
            // 
            // evenlySplitContainer1.Panel2
            // 
            this.evenlySplitContainer1.Panel2.Controls.Add(this.label1);
            this.evenlySplitContainer1.Panel2.Controls.Add(this.standardCourseTimeTextBox);
            this.evenlySplitContainer1.Panel2.Controls.Add(this.maximumCourseTimeTextBox);
            this.evenlySplitContainer1.Panel2.Controls.Add(this.intermediateCountUpDown);
            this.evenlySplitContainer1.Panel2.Controls.Add(this.label2);
            this.evenlySplitContainer1.Panel2.Controls.Add(this.label5);
            this.evenlySplitContainer1.Panel2.Controls.Add(this.label7);
            this.evenlySplitContainer1.Panel2.Controls.Add(this.trackLengthTextBox);
            this.evenlySplitContainer1.Panel2.Controls.Add(this.label4);
            this.evenlySplitContainer1.Panel2.Controls.Add(this.label6);
            this.evenlySplitContainer1.Panel2.Controls.Add(this.label3);
            this.evenlySplitContainer1.Size = new System.Drawing.Size(861, 107);
            this.evenlySplitContainer1.SplitterDistance = 430;
            this.evenlySplitContainer1.TabIndex = 0;
            // 
            // gradeTextBox
            // 
            this.gradeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gradeTextBox.Location = new System.Drawing.Point(89, 6);
            this.gradeTextBox.Name = "gradeTextBox";
            this.gradeTextBox.Size = new System.Drawing.Size(339, 20);
            this.gradeTextBox.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(36, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "&Grade";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 87);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(58, 13);
            this.label13.TabIndex = 6;
            this.label13.Text = "Ring &name";
            // 
            // ringNameTextBox
            // 
            this.ringNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ringNameTextBox.Location = new System.Drawing.Point(89, 84);
            this.ringNameTextBox.Name = "ringNameTextBox";
            this.ringNameTextBox.Size = new System.Drawing.Size(339, 20);
            this.ringNameTextBox.TabIndex = 7;
            // 
            // classTypeTextBox
            // 
            this.classTypeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.classTypeTextBox.Location = new System.Drawing.Point(89, 32);
            this.classTypeTextBox.Name = "classTypeTextBox";
            this.classTypeTextBox.Size = new System.Drawing.Size(339, 20);
            this.classTypeTextBox.TabIndex = 3;
            // 
            // inspectorNameTextBox
            // 
            this.inspectorNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inspectorNameTextBox.Location = new System.Drawing.Point(89, 58);
            this.inspectorNameTextBox.Name = "inspectorNameTextBox";
            this.inspectorNameTextBox.Size = new System.Drawing.Size(339, 20);
            this.inspectorNameTextBox.TabIndex = 5;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 61);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 13);
            this.label12.TabIndex = 4;
            this.label12.Text = "Ins&pector name";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 35);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(55, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "C&lass type";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Standard Course Time (SCT)";
            // 
            // standardCourseTimeTextBox
            // 
            this.standardCourseTimeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.standardCourseTimeTextBox.Location = new System.Drawing.Point(170, 6);
            this.standardCourseTimeTextBox.Name = "standardCourseTimeTextBox";
            this.standardCourseTimeTextBox.Size = new System.Drawing.Size(183, 20);
            this.standardCourseTimeTextBox.TabIndex = 1;
            this.standardCourseTimeTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.StandardCourseTimeTextBox_Validating);
            // 
            // maximumCourseTimeTextBox
            // 
            this.maximumCourseTimeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maximumCourseTimeTextBox.Location = new System.Drawing.Point(170, 32);
            this.maximumCourseTimeTextBox.Name = "maximumCourseTimeTextBox";
            this.maximumCourseTimeTextBox.Size = new System.Drawing.Size(183, 20);
            this.maximumCourseTimeTextBox.TabIndex = 4;
            this.maximumCourseTimeTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.MaximumCourseTimeTextBox_Validating);
            // 
            // intermediateCountUpDown
            // 
            this.intermediateCountUpDown.Location = new System.Drawing.Point(170, 85);
            this.intermediateCountUpDown.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.intermediateCountUpDown.Name = "intermediateCountUpDown";
            this.intermediateCountUpDown.Size = new System.Drawing.Size(59, 20);
            this.intermediateCountUpDown.TabIndex = 10;
            this.intermediateCountUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "&Maximum Course Time (MCT)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "&Track length";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 87);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(106, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Interme&diate Timers *";
            // 
            // trackLengthTextBox
            // 
            this.trackLengthTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackLengthTextBox.Location = new System.Drawing.Point(170, 58);
            this.trackLengthTextBox.Name = "trackLengthTextBox";
            this.trackLengthTextBox.Size = new System.Drawing.Size(183, 20);
            this.trackLengthTextBox.TabIndex = 7;
            this.trackLengthTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.TrackLengthTextBox_Validating);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(371, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "(seconds)";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(380, 61);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "(meters)";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(371, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "(seconds)";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(742, 137);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(129, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "* Indicates a required field";
            // 
            // startFinishMinDelayCheckBox
            // 
            this.startFinishMinDelayCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.startFinishMinDelayCheckBox.AutoSize = true;
            this.startFinishMinDelayCheckBox.Location = new System.Drawing.Point(12, 137);
            this.startFinishMinDelayCheckBox.Name = "startFinishMinDelayCheckBox";
            this.startFinishMinDelayCheckBox.Size = new System.Drawing.Size(377, 17);
            this.startFinishMinDelayCheckBox.TabIndex = 1;
            this.startFinishMinDelayCheckBox.Text = "Use a single gate for Start and Finish. Minimum elapsed time to allow finish:";
            this.startFinishMinDelayCheckBox.UseVisualStyleBackColor = true;
            this.startFinishMinDelayCheckBox.CheckedChanged += new System.EventHandler(this.StartFinishMinDelayCheckBox_CheckedChanged);
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(464, 138);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "(seconds)";
            // 
            // startFinishMinDelayUpDown
            // 
            this.startFinishMinDelayUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.startFinishMinDelayUpDown.Enabled = false;
            this.startFinishMinDelayUpDown.Location = new System.Drawing.Point(395, 135);
            this.startFinishMinDelayUpDown.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.startFinishMinDelayUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.startFinishMinDelayUpDown.Name = "startFinishMinDelayUpDown";
            this.startFinishMinDelayUpDown.Size = new System.Drawing.Size(59, 20);
            this.startFinishMinDelayUpDown.TabIndex = 2;
            this.startFinishMinDelayUpDown.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(814, 566);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(730, 566);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "&Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // importButton
            // 
            this.importButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.importButton.Location = new System.Drawing.Point(87, 192);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(75, 23);
            this.importButton.TabIndex = 2;
            this.importButton.Text = "&Import";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.discardTimingsCheckBox);
            this.groupBox2.Controls.Add(this.mergeDeletesCheckBox);
            this.groupBox2.Controls.Add(this.clearButton);
            this.groupBox2.Controls.Add(this.competitorsGrid);
            this.groupBox2.Controls.Add(this.importButton);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(877, 221);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Competitors";
            // 
            // discardTimingsCheckBox
            // 
            this.discardTimingsCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.discardTimingsCheckBox.AutoSize = true;
            this.discardTimingsCheckBox.Checked = true;
            this.discardTimingsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.discardTimingsCheckBox.Enabled = false;
            this.discardTimingsCheckBox.Location = new System.Drawing.Point(501, 196);
            this.discardTimingsCheckBox.Name = "discardTimingsCheckBox";
            this.discardTimingsCheckBox.Size = new System.Drawing.Size(187, 17);
            this.discardTimingsCheckBox.TabIndex = 4;
            this.discardTimingsCheckBox.Text = "Discard timings in file during import";
            this.discardTimingsCheckBox.UseVisualStyleBackColor = true;
            // 
            // mergeDeletesCheckBox
            // 
            this.mergeDeletesCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mergeDeletesCheckBox.AutoSize = true;
            this.mergeDeletesCheckBox.Location = new System.Drawing.Point(168, 196);
            this.mergeDeletesCheckBox.Name = "mergeDeletesCheckBox";
            this.mergeDeletesCheckBox.Size = new System.Drawing.Size(327, 17);
            this.mergeDeletesCheckBox.TabIndex = 3;
            this.mergeDeletesCheckBox.Text = "&Remove existing competitors that are missing in file during import";
            this.mergeDeletesCheckBox.UseVisualStyleBackColor = true;
            // 
            // clearButton
            // 
            this.clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.clearButton.Location = new System.Drawing.Point(6, 192);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 23);
            this.clearButton.TabIndex = 1;
            this.clearButton.Text = "Clear &all";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // competitorsGrid
            // 
            this.competitorsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.competitorsGrid.HideBackColors = true;
            this.competitorsGrid.IsEditable = false;
            this.competitorsGrid.Location = new System.Drawing.Point(6, 19);
            this.competitorsGrid.Name = "competitorsGrid";
            this.competitorsGrid.ShowPlacement = false;
            this.competitorsGrid.Size = new System.Drawing.Size(865, 167);
            this.competitorsGrid.TabIndex = 0;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.evenlySplitContainer2);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Location = new System.Drawing.Point(12, 405);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(877, 155);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Pictures to show and sounds to play during competition runs";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 130);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(34, 13);
            this.label18.TabIndex = 4;
            this.label18.Text = "Key 1";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 103);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(73, 13);
            this.label17.TabIndex = 3;
            this.label17.Text = "Ready to start";
            // 
            // evenlySplitContainer2
            // 
            this.evenlySplitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.evenlySplitContainer2.IsSplitterFixed = true;
            this.evenlySplitContainer2.Location = new System.Drawing.Point(112, 19);
            this.evenlySplitContainer2.Name = "evenlySplitContainer2";
            // 
            // evenlySplitContainer2.Panel1
            // 
            this.evenlySplitContainer2.Panel1.Controls.Add(this.cleanRunInSctPictureAlert);
            this.evenlySplitContainer2.Panel1.Controls.Add(this.eliminatedPictureAlert);
            this.evenlySplitContainer2.Panel1.Controls.Add(this.firstPlacePictureAlert);
            // 
            // evenlySplitContainer2.Panel2
            // 
            this.evenlySplitContainer2.Panel2.Controls.Add(this.customItemASoundAlert);
            this.evenlySplitContainer2.Panel2.Controls.Add(this.readyToStartSoundAlert);
            this.evenlySplitContainer2.Panel2.Controls.Add(this.cleanRunInSctSoundAlert);
            this.evenlySplitContainer2.Panel2.Controls.Add(this.eliminatedSoundAlert);
            this.evenlySplitContainer2.Panel2.Controls.Add(this.firstPlaceSoundAlert);
            this.evenlySplitContainer2.Size = new System.Drawing.Size(759, 131);
            this.evenlySplitContainer2.SplitterDistance = 379;
            this.evenlySplitContainer2.TabIndex = 5;
            // 
            // cleanRunInSctPictureAlert
            // 
            this.cleanRunInSctPictureAlert.AlertName = "Clean run in SCT";
            this.cleanRunInSctPictureAlert.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cleanRunInSctPictureAlert.ErrorProvider = this.errorProvider;
            this.cleanRunInSctPictureAlert.Location = new System.Drawing.Point(3, 54);
            this.cleanRunInSctPictureAlert.Name = "cleanRunInSctPictureAlert";
            this.cleanRunInSctPictureAlert.Size = new System.Drawing.Size(373, 22);
            this.cleanRunInSctPictureAlert.TabIndex = 2;
            // 
            // eliminatedPictureAlert
            // 
            this.eliminatedPictureAlert.AlertName = "Eliminated";
            this.eliminatedPictureAlert.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eliminatedPictureAlert.ErrorProvider = this.errorProvider;
            this.eliminatedPictureAlert.Location = new System.Drawing.Point(3, 2);
            this.eliminatedPictureAlert.Name = "eliminatedPictureAlert";
            this.eliminatedPictureAlert.Size = new System.Drawing.Size(373, 22);
            this.eliminatedPictureAlert.TabIndex = 0;
            // 
            // firstPlacePictureAlert
            // 
            this.firstPlacePictureAlert.AlertName = "First place";
            this.firstPlacePictureAlert.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.firstPlacePictureAlert.ErrorProvider = this.errorProvider;
            this.firstPlacePictureAlert.Location = new System.Drawing.Point(3, 28);
            this.firstPlacePictureAlert.Name = "firstPlacePictureAlert";
            this.firstPlacePictureAlert.Size = new System.Drawing.Size(373, 22);
            this.firstPlacePictureAlert.TabIndex = 1;
            // 
            // customItemASoundAlert
            // 
            this.customItemASoundAlert.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.customItemASoundAlert.ErrorProvider = this.errorProvider;
            this.customItemASoundAlert.Location = new System.Drawing.Point(3, 106);
            this.customItemASoundAlert.Name = "customItemASoundAlert";
            this.customItemASoundAlert.Size = new System.Drawing.Size(370, 22);
            this.customItemASoundAlert.TabIndex = 4;
            // 
            // readyToStartSoundAlert
            // 
            this.readyToStartSoundAlert.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.readyToStartSoundAlert.ErrorProvider = this.errorProvider;
            this.readyToStartSoundAlert.Location = new System.Drawing.Point(3, 80);
            this.readyToStartSoundAlert.Name = "readyToStartSoundAlert";
            this.readyToStartSoundAlert.Size = new System.Drawing.Size(370, 22);
            this.readyToStartSoundAlert.TabIndex = 3;
            // 
            // cleanRunInSctSoundAlert
            // 
            this.cleanRunInSctSoundAlert.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cleanRunInSctSoundAlert.ErrorProvider = this.errorProvider;
            this.cleanRunInSctSoundAlert.Location = new System.Drawing.Point(3, 54);
            this.cleanRunInSctSoundAlert.Name = "cleanRunInSctSoundAlert";
            this.cleanRunInSctSoundAlert.Size = new System.Drawing.Size(370, 22);
            this.cleanRunInSctSoundAlert.TabIndex = 2;
            // 
            // eliminatedSoundAlert
            // 
            this.eliminatedSoundAlert.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eliminatedSoundAlert.ErrorProvider = this.errorProvider;
            this.eliminatedSoundAlert.Location = new System.Drawing.Point(3, 2);
            this.eliminatedSoundAlert.Name = "eliminatedSoundAlert";
            this.eliminatedSoundAlert.Size = new System.Drawing.Size(370, 22);
            this.eliminatedSoundAlert.TabIndex = 0;
            // 
            // firstPlaceSoundAlert
            // 
            this.firstPlaceSoundAlert.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.firstPlaceSoundAlert.ErrorProvider = this.errorProvider;
            this.firstPlaceSoundAlert.Location = new System.Drawing.Point(3, 28);
            this.firstPlaceSoundAlert.Name = "firstPlaceSoundAlert";
            this.firstPlaceSoundAlert.Size = new System.Drawing.Size(370, 22);
            this.firstPlaceSoundAlert.TabIndex = 1;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 52);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(55, 13);
            this.label16.TabIndex = 1;
            this.label16.Text = "First place";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 78);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(87, 13);
            this.label15.TabIndex = 2;
            this.label15.Text = "Clean run in SCT";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 26);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(55, 13);
            this.label14.TabIndex = 0;
            this.label14.Text = "Eliminated";
            // 
            // ClassSetupForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(901, 601);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(690, 430);
            this.Name = "ClassSetupForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Class Setup - Dog Agility Competition Controller";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ClassSetupForm_FormClosed);
            this.Load += new System.EventHandler(this.ClassSetupForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.evenlySplitContainer1.Panel1.ResumeLayout(false);
            this.evenlySplitContainer1.Panel1.PerformLayout();
            this.evenlySplitContainer1.Panel2.ResumeLayout(false);
            this.evenlySplitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.evenlySplitContainer1)).EndInit();
            this.evenlySplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.intermediateCountUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startFinishMinDelayUpDown)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.evenlySplitContainer2.Panel1.ResumeLayout(false);
            this.evenlySplitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.evenlySplitContainer2)).EndInit();
            this.evenlySplitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private Label label1;
        private TextBox standardCourseTimeTextBox;
        private Label label2;
        private TextBox maximumCourseTimeTextBox;
        private Label label4;
        private Label label3;
        private TextBox trackLengthTextBox;
        private Label label6;
        private Label label5;
        private Label label7;
        private NumericUpDown intermediateCountUpDown;
        private Button cancelButton;
        private Button okButton;
        private TextBox classTypeTextBox;
        private Label label11;
        private TextBox gradeTextBox;
        private Label label10;
        private Button importButton;
        private GroupBox groupBox2;
        private CompetitionRunResultsGrid competitorsGrid;
        private Button clearButton;
        private Label label8;
        private NumericUpDown startFinishMinDelayUpDown;
        private TextBox ringNameTextBox;
        private TextBox inspectorNameTextBox;
        private Label label13;
        private Label label12;
        private CheckBox startFinishMinDelayCheckBox;
        private CheckBox mergeDeletesCheckBox;
        private CheckBox discardTimingsCheckBox;
        private ErrorProvider errorProvider;
        private Label label9;
        private GroupBox groupBox3;
        private Label label14;
        private Label label15;
        private EvenlySplitContainer evenlySplitContainer1;
        private Label label16;
        private EvenlySplitContainer evenlySplitContainer2;
        private Label label17;
        private CompetitionPictureAlert firstPlacePictureAlert;
        private CompetitionSoundAlert firstPlaceSoundAlert;
        private CompetitionPictureAlert cleanRunInSctPictureAlert;
        private CompetitionPictureAlert eliminatedPictureAlert;
        private CompetitionSoundAlert readyToStartSoundAlert;
        private CompetitionSoundAlert cleanRunInSctSoundAlert;
        private CompetitionSoundAlert eliminatedSoundAlert;
        private Label label18;
        private CompetitionSoundAlert customItemASoundAlert;
    }
}