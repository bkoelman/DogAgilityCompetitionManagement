using System.ComponentModel;
using System.Windows.Forms;
using DogAgilityCompetition.Controller.UI.Controls;

namespace DogAgilityCompetition.Controller.UI.Forms
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.logLinkLabel = new System.Windows.Forms.LinkLabel();
            this.emulatorLinkLabel = new System.Windows.Forms.LinkLabel();
            this.displayModeGroupBox = new System.Windows.Forms.GroupBox();
            this.noneRadioButton = new System.Windows.Forms.RadioButton();
            this.customRadioButton = new System.Windows.Forms.RadioButton();
            this.resultsRadioButton = new System.Windows.Forms.RadioButton();
            this.stopClassButton = new System.Windows.Forms.Button();
            this.activeRunRadioButton = new System.Windows.Forms.RadioButton();
            this.startClassButton = new System.Windows.Forms.Button();
            this.preparationGroupBox = new System.Windows.Forms.GroupBox();
            this.setupClassButton = new System.Windows.Forms.Button();
            this.customDisplayButton = new System.Windows.Forms.Button();
            this.networkSetupButton = new System.Windows.Forms.Button();
            this.resultsButton = new System.Windows.Forms.Button();
            this.healthTextBox = new System.Windows.Forms.TextBox();
            this.stateGroupBox = new System.Windows.Forms.GroupBox();
            this.stateVisualizer = new DogAgilityCompetition.Controller.UI.Controls.StateVisualizer();
            this.competitionOverviewGroupBox = new System.Windows.Forms.GroupBox();
            this.competitionStateOverview = new DogAgilityCompetition.Controller.UI.Controls.CompetitionStateOverview();
            this.lastCompletedRunEditor = new DogAgilityCompetition.Controller.UI.Controls.RunResultEditor();
            this.networkStatusView = new DogAgilityCompetition.Controller.UI.Controls.NetworkGrid();
            this.displayModeGroupBox.SuspendLayout();
            this.preparationGroupBox.SuspendLayout();
            this.stateGroupBox.SuspendLayout();
            this.competitionOverviewGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // logLinkLabel
            // 
            this.logLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.logLinkLabel.Location = new System.Drawing.Point(23, 117);
            this.logLinkLabel.Name = "logLinkLabel";
            this.logLinkLabel.Size = new System.Drawing.Size(87, 13);
            this.logLinkLabel.TabIndex = 6;
            this.logLinkLabel.TabStop = true;
            this.logLinkLabel.Text = "Show log";
            this.logLinkLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.logLinkLabel.Visible = false;
            this.logLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LogLinkLabel_LinkClicked);
            // 
            // emulatorLinkLabel
            // 
            this.emulatorLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.emulatorLinkLabel.Location = new System.Drawing.Point(116, 117);
            this.emulatorLinkLabel.Name = "emulatorLinkLabel";
            this.emulatorLinkLabel.Size = new System.Drawing.Size(87, 13);
            this.emulatorLinkLabel.TabIndex = 7;
            this.emulatorLinkLabel.TabStop = true;
            this.emulatorLinkLabel.Text = "Turn emulator on";
            this.emulatorLinkLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.emulatorLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.EmulatorLinkLabel_LinkClicked);
            // 
            // displayModeGroupBox
            // 
            this.displayModeGroupBox.Controls.Add(this.noneRadioButton);
            this.displayModeGroupBox.Controls.Add(this.customRadioButton);
            this.displayModeGroupBox.Controls.Add(this.resultsRadioButton);
            this.displayModeGroupBox.Controls.Add(this.emulatorLinkLabel);
            this.displayModeGroupBox.Controls.Add(this.logLinkLabel);
            this.displayModeGroupBox.Controls.Add(this.stopClassButton);
            this.displayModeGroupBox.Controls.Add(this.activeRunRadioButton);
            this.displayModeGroupBox.Controls.Add(this.startClassButton);
            this.displayModeGroupBox.Location = new System.Drawing.Point(128, 12);
            this.displayModeGroupBox.Name = "displayModeGroupBox";
            this.displayModeGroupBox.Size = new System.Drawing.Size(209, 139);
            this.displayModeGroupBox.TabIndex = 1;
            this.displayModeGroupBox.TabStop = false;
            this.displayModeGroupBox.Text = "Display mode";
            // 
            // noneRadioButton
            // 
            this.noneRadioButton.AutoSize = true;
            this.noneRadioButton.Checked = true;
            this.noneRadioButton.Location = new System.Drawing.Point(6, 88);
            this.noneRadioButton.Name = "noneRadioButton";
            this.noneRadioButton.Size = new System.Drawing.Size(51, 17);
            this.noneRadioButton.TabIndex = 3;
            this.noneRadioButton.TabStop = true;
            this.noneRadioButton.Text = "None";
            this.noneRadioButton.UseVisualStyleBackColor = true;
            this.noneRadioButton.CheckedChanged += new System.EventHandler(this.DisplayModeRadioButton_CheckedChanged);
            // 
            // customRadioButton
            // 
            this.customRadioButton.AutoSize = true;
            this.customRadioButton.Location = new System.Drawing.Point(6, 65);
            this.customRadioButton.Name = "customRadioButton";
            this.customRadioButton.Size = new System.Drawing.Size(60, 17);
            this.customRadioButton.TabIndex = 2;
            this.customRadioButton.Text = "Custom";
            this.customRadioButton.UseVisualStyleBackColor = true;
            this.customRadioButton.CheckedChanged += new System.EventHandler(this.DisplayModeRadioButton_CheckedChanged);
            // 
            // resultsRadioButton
            // 
            this.resultsRadioButton.AutoSize = true;
            this.resultsRadioButton.Location = new System.Drawing.Point(6, 42);
            this.resultsRadioButton.Name = "resultsRadioButton";
            this.resultsRadioButton.Size = new System.Drawing.Size(60, 17);
            this.resultsRadioButton.TabIndex = 1;
            this.resultsRadioButton.Text = "Results";
            this.resultsRadioButton.UseVisualStyleBackColor = true;
            this.resultsRadioButton.CheckedChanged += new System.EventHandler(this.DisplayModeRadioButton_CheckedChanged);
            // 
            // stopClassButton
            // 
            this.stopClassButton.Enabled = false;
            this.stopClassButton.Location = new System.Drawing.Point(128, 42);
            this.stopClassButton.Name = "stopClassButton";
            this.stopClassButton.Size = new System.Drawing.Size(75, 23);
            this.stopClassButton.TabIndex = 5;
            this.stopClassButton.Text = "Stop";
            this.stopClassButton.UseVisualStyleBackColor = true;
            this.stopClassButton.Click += new System.EventHandler(this.StopClassButton_Click);
            // 
            // activeRunRadioButton
            // 
            this.activeRunRadioButton.AutoSize = true;
            this.activeRunRadioButton.Location = new System.Drawing.Point(6, 19);
            this.activeRunRadioButton.Name = "activeRunRadioButton";
            this.activeRunRadioButton.Size = new System.Drawing.Size(73, 17);
            this.activeRunRadioButton.TabIndex = 0;
            this.activeRunRadioButton.Text = "Active run";
            this.activeRunRadioButton.UseVisualStyleBackColor = true;
            this.activeRunRadioButton.CheckedChanged += new System.EventHandler(this.DisplayModeRadioButton_CheckedChanged);
            // 
            // startClassButton
            // 
            this.startClassButton.Enabled = false;
            this.startClassButton.Location = new System.Drawing.Point(128, 16);
            this.startClassButton.Name = "startClassButton";
            this.startClassButton.Size = new System.Drawing.Size(75, 23);
            this.startClassButton.TabIndex = 4;
            this.startClassButton.Text = "Start";
            this.startClassButton.UseVisualStyleBackColor = true;
            this.startClassButton.Click += new System.EventHandler(this.StartClassButton_Click);
            // 
            // preparationGroupBox
            // 
            this.preparationGroupBox.Controls.Add(this.setupClassButton);
            this.preparationGroupBox.Controls.Add(this.customDisplayButton);
            this.preparationGroupBox.Controls.Add(this.networkSetupButton);
            this.preparationGroupBox.Controls.Add(this.resultsButton);
            this.preparationGroupBox.Location = new System.Drawing.Point(12, 12);
            this.preparationGroupBox.Name = "preparationGroupBox";
            this.preparationGroupBox.Size = new System.Drawing.Size(110, 139);
            this.preparationGroupBox.TabIndex = 0;
            this.preparationGroupBox.TabStop = false;
            this.preparationGroupBox.Text = "Preparation";
            // 
            // setupClassButton
            // 
            this.setupClassButton.Location = new System.Drawing.Point(6, 19);
            this.setupClassButton.Name = "setupClassButton";
            this.setupClassButton.Size = new System.Drawing.Size(90, 23);
            this.setupClassButton.TabIndex = 0;
            this.setupClassButton.Text = "Setup class";
            this.setupClassButton.UseVisualStyleBackColor = true;
            this.setupClassButton.Click += new System.EventHandler(this.SetupClassButton_Click);
            // 
            // customDisplayButton
            // 
            this.customDisplayButton.Location = new System.Drawing.Point(6, 106);
            this.customDisplayButton.Name = "customDisplayButton";
            this.customDisplayButton.Size = new System.Drawing.Size(90, 23);
            this.customDisplayButton.TabIndex = 3;
            this.customDisplayButton.Text = "Custom display";
            this.customDisplayButton.UseVisualStyleBackColor = true;
            this.customDisplayButton.Click += new System.EventHandler(this.CustomDisplayButton_Click);
            // 
            // networkSetupButton
            // 
            this.networkSetupButton.Location = new System.Drawing.Point(6, 48);
            this.networkSetupButton.Name = "networkSetupButton";
            this.networkSetupButton.Size = new System.Drawing.Size(90, 23);
            this.networkSetupButton.TabIndex = 1;
            this.networkSetupButton.Text = "Setup network";
            this.networkSetupButton.UseVisualStyleBackColor = true;
            this.networkSetupButton.Click += new System.EventHandler(this.NetworkSetupButton_Click);
            // 
            // resultsButton
            // 
            this.resultsButton.Location = new System.Drawing.Point(6, 77);
            this.resultsButton.Name = "resultsButton";
            this.resultsButton.Size = new System.Drawing.Size(90, 23);
            this.resultsButton.TabIndex = 2;
            this.resultsButton.Text = "Results";
            this.resultsButton.UseVisualStyleBackColor = true;
            this.resultsButton.Click += new System.EventHandler(this.ResultsButton_Click);
            // 
            // healthTextBox
            // 
            this.healthTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.healthTextBox.Location = new System.Drawing.Point(346, 18);
            this.healthTextBox.Multiline = true;
            this.healthTextBox.Name = "healthTextBox";
            this.healthTextBox.ReadOnly = true;
            this.healthTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.healthTextBox.Size = new System.Drawing.Size(762, 133);
            this.healthTextBox.TabIndex = 2;
            // 
            // stateGroupBox
            // 
            this.stateGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.stateGroupBox.Controls.Add(this.stateVisualizer);
            this.stateGroupBox.Location = new System.Drawing.Point(12, 230);
            this.stateGroupBox.Name = "stateGroupBox";
            this.stateGroupBox.Size = new System.Drawing.Size(365, 499);
            this.stateGroupBox.TabIndex = 4;
            this.stateGroupBox.TabStop = false;
            // 
            // stateVisualizer
            // 
            this.stateVisualizer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stateVisualizer.IntermediateTimerCount = 3;
            this.stateVisualizer.Location = new System.Drawing.Point(6, 14);
            this.stateVisualizer.Name = "stateVisualizer";
            this.stateVisualizer.Size = new System.Drawing.Size(353, 479);
            this.stateVisualizer.TabIndex = 0;
            this.stateVisualizer.TabStop = false;
            this.stateVisualizer.Text = "stateVisualizer";
            // 
            // competitionOverviewGroupBox
            // 
            this.competitionOverviewGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.competitionOverviewGroupBox.Controls.Add(this.competitionStateOverview);
            this.competitionOverviewGroupBox.Location = new System.Drawing.Point(383, 230);
            this.competitionOverviewGroupBox.Name = "competitionOverviewGroupBox";
            this.competitionOverviewGroupBox.Size = new System.Drawing.Size(725, 205);
            this.competitionOverviewGroupBox.TabIndex = 5;
            this.competitionOverviewGroupBox.TabStop = false;
            // 
            // competitionStateOverview
            // 
            this.competitionStateOverview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.competitionStateOverview.Location = new System.Drawing.Point(6, 15);
            this.competitionStateOverview.Name = "competitionStateOverview";
            this.competitionStateOverview.Size = new System.Drawing.Size(713, 188);
            this.competitionStateOverview.TabIndex = 0;
            this.competitionStateOverview.TabStop = false;
            // 
            // lastCompletedRunEditor
            // 
            this.lastCompletedRunEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lastCompletedRunEditor.Location = new System.Drawing.Point(12, 157);
            this.lastCompletedRunEditor.Name = "lastCompletedRunEditor";
            this.lastCompletedRunEditor.Size = new System.Drawing.Size(1096, 67);
            this.lastCompletedRunEditor.TabIndex = 3;
            this.lastCompletedRunEditor.RunResultChanging += new System.EventHandler<DogAgilityCompetition.Controller.UI.Controls.RunResultChangingEventArgs>(this.LastCompletedRunEditor_RunResultChanging);
            this.lastCompletedRunEditor.RunResultChanged += new System.EventHandler(this.LastCompletedRunEditor_RunResultChanged);
            // 
            // networkStatusView
            // 
            this.networkStatusView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.networkStatusView.IsConnected = false;
            this.networkStatusView.Location = new System.Drawing.Point(383, 441);
            this.networkStatusView.Name = "networkStatusView";
            this.networkStatusView.Size = new System.Drawing.Size(725, 288);
            this.networkStatusView.TabIndex = 6;
            this.networkStatusView.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1120, 741);
            this.Controls.Add(this.competitionOverviewGroupBox);
            this.Controls.Add(this.stateGroupBox);
            this.Controls.Add(this.lastCompletedRunEditor);
            this.Controls.Add(this.displayModeGroupBox);
            this.Controls.Add(this.preparationGroupBox);
            this.Controls.Add(this.healthTextBox);
            this.Controls.Add(this.networkStatusView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Dog Agility Competition Controller";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.displayModeGroupBox.ResumeLayout(false);
            this.displayModeGroupBox.PerformLayout();
            this.preparationGroupBox.ResumeLayout(false);
            this.stateGroupBox.ResumeLayout(false);
            this.competitionOverviewGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NetworkGrid networkStatusView;
        private Button networkSetupButton;
        private TextBox healthTextBox;
        private Button startClassButton;
        private Button stopClassButton;
        private Button setupClassButton;
        private Button resultsButton;
        private Button customDisplayButton;
        private GroupBox preparationGroupBox;
        private GroupBox displayModeGroupBox;
        private RadioButton customRadioButton;
        private RadioButton resultsRadioButton;
        private RadioButton activeRunRadioButton;
        private RadioButton noneRadioButton;
        private LinkLabel emulatorLinkLabel;
        private LinkLabel logLinkLabel;
        private RunResultEditor lastCompletedRunEditor;
        private StateVisualizer stateVisualizer;
        private GroupBox stateGroupBox;
        private CompetitionStateOverview competitionStateOverview;
        private GroupBox competitionOverviewGroupBox;
    }
}

