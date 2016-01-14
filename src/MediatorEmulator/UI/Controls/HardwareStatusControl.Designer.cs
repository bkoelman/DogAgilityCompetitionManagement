using System.ComponentModel;
using System.Windows.Forms;

namespace DogAgilityCompetition.MediatorEmulator.UI.Controls
{
    partial class HardwareStatusControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.hasVersionMismatchCheckBox = new System.Windows.Forms.CheckBox();
            this.pausePlayClockButton = new System.Windows.Forms.Button();
            this.clockLabel = new System.Windows.Forms.Label();
            this.batteryStatusLabel = new System.Windows.Forms.Label();
            this.batteryStatusTrackBar = new System.Windows.Forms.TrackBar();
            this.alignedCheckBox = new System.Windows.Forms.CheckBox();
            this.automaticCheckBox = new System.Windows.Forms.CheckBox();
            this.signalStrengthTrackBar = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.syncStateLabel = new System.Windows.Forms.Label();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.batteryStatusTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.signalStrengthTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.hasVersionMismatchCheckBox);
            this.groupBox1.Controls.Add(this.pausePlayClockButton);
            this.groupBox1.Controls.Add(this.clockLabel);
            this.groupBox1.Controls.Add(this.batteryStatusLabel);
            this.groupBox1.Controls.Add(this.batteryStatusTrackBar);
            this.groupBox1.Controls.Add(this.alignedCheckBox);
            this.groupBox1.Controls.Add(this.automaticCheckBox);
            this.groupBox1.Controls.Add(this.signalStrengthTrackBar);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.syncStateLabel);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(255, 206);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hardware status";
            // 
            // hasVersionMismatchCheckBox
            // 
            this.hasVersionMismatchCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.hasVersionMismatchCheckBox.AutoSize = true;
            this.hasVersionMismatchCheckBox.Location = new System.Drawing.Point(141, 19);
            this.hasVersionMismatchCheckBox.Name = "hasVersionMismatchCheckBox";
            this.hasVersionMismatchCheckBox.Size = new System.Drawing.Size(108, 17);
            this.hasVersionMismatchCheckBox.TabIndex = 1;
            this.hasVersionMismatchCheckBox.Text = "Version mismatch";
            this.hasVersionMismatchCheckBox.UseVisualStyleBackColor = true;
            this.hasVersionMismatchCheckBox.CheckedChanged += new System.EventHandler(this.HasVersionMismatchCheckBox_CheckedChanged);
            // 
            // pausePlayClockButton
            // 
            this.pausePlayClockButton.Image = global::DogAgilityCompetition.MediatorEmulator.Properties.Resources.PauseButton;
            this.pausePlayClockButton.Location = new System.Drawing.Point(6, 177);
            this.pausePlayClockButton.Name = "pausePlayClockButton";
            this.pausePlayClockButton.Size = new System.Drawing.Size(24, 24);
            this.pausePlayClockButton.TabIndex = 7;
            this.pausePlayClockButton.UseVisualStyleBackColor = true;
            this.pausePlayClockButton.Click += new System.EventHandler(this.PausePlayClockButton_Click);
            // 
            // clockLabel
            // 
            this.clockLabel.AutoSize = true;
            this.clockLabel.Location = new System.Drawing.Point(36, 183);
            this.clockLabel.Name = "clockLabel";
            this.clockLabel.Size = new System.Drawing.Size(111, 13);
            this.clockLabel.TabIndex = 8;
            this.clockLabel.Text = "Clock (sec): 000.0000";
            // 
            // batteryStatusLabel
            // 
            this.batteryStatusLabel.AutoSize = true;
            this.batteryStatusLabel.Location = new System.Drawing.Point(6, 119);
            this.batteryStatusLabel.Name = "batteryStatusLabel";
            this.batteryStatusLabel.Size = new System.Drawing.Size(74, 13);
            this.batteryStatusLabel.TabIndex = 5;
            this.batteryStatusLabel.Text = "Battery status:";
            // 
            // batteryStatusTrackBar
            // 
            this.batteryStatusTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.batteryStatusTrackBar.LargeChange = 10;
            this.batteryStatusTrackBar.Location = new System.Drawing.Point(6, 135);
            this.batteryStatusTrackBar.Maximum = 255;
            this.batteryStatusTrackBar.Name = "batteryStatusTrackBar";
            this.batteryStatusTrackBar.Size = new System.Drawing.Size(243, 45);
            this.batteryStatusTrackBar.TabIndex = 6;
            this.batteryStatusTrackBar.Value = 200;
            this.batteryStatusTrackBar.Scroll += new System.EventHandler(this.TrackBar_Scroll);
            // 
            // alignedCheckBox
            // 
            this.alignedCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.alignedCheckBox.AutoSize = true;
            this.alignedCheckBox.Location = new System.Drawing.Point(188, 42);
            this.alignedCheckBox.Name = "alignedCheckBox";
            this.alignedCheckBox.Size = new System.Drawing.Size(61, 17);
            this.alignedCheckBox.TabIndex = 2;
            this.alignedCheckBox.Text = "Aligned";
            this.alignedCheckBox.UseVisualStyleBackColor = true;
            this.alignedCheckBox.Visible = false;
            this.alignedCheckBox.CheckedChanged += new System.EventHandler(this.AlignedCheckBox_CheckedChanged);
            // 
            // automaticCheckBox
            // 
            this.automaticCheckBox.AutoSize = true;
            this.automaticCheckBox.Location = new System.Drawing.Point(6, 19);
            this.automaticCheckBox.Name = "automaticCheckBox";
            this.automaticCheckBox.Size = new System.Drawing.Size(116, 17);
            this.automaticCheckBox.TabIndex = 0;
            this.automaticCheckBox.Text = "Automatic variation";
            this.automaticCheckBox.UseVisualStyleBackColor = true;
            this.automaticCheckBox.CheckedChanged += new System.EventHandler(this.AutomaticCheckBox_CheckedChanged);
            // 
            // signalStrengthTrackBar
            // 
            this.signalStrengthTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.signalStrengthTrackBar.LargeChange = 10;
            this.signalStrengthTrackBar.Location = new System.Drawing.Point(6, 71);
            this.signalStrengthTrackBar.Maximum = 255;
            this.signalStrengthTrackBar.Name = "signalStrengthTrackBar";
            this.signalStrengthTrackBar.Size = new System.Drawing.Size(243, 45);
            this.signalStrengthTrackBar.TabIndex = 4;
            this.signalStrengthTrackBar.Value = 150;
            this.signalStrengthTrackBar.Scroll += new System.EventHandler(this.TrackBar_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Signal strength:";
            // 
            // syncStateLabel
            // 
            this.syncStateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.syncStateLabel.Location = new System.Drawing.Point(153, 183);
            this.syncStateLabel.Name = "syncStateLabel";
            this.syncStateLabel.Size = new System.Drawing.Size(96, 13);
            this.syncStateLabel.TabIndex = 9;
            this.syncStateLabel.Text = "Sync state";
            this.syncStateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // updateTimer
            // 
            this.updateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
            // 
            // HardwareStatusControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Enabled = false;
            this.Name = "HardwareStatusControl";
            this.Size = new System.Drawing.Size(261, 212);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.batteryStatusTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.signalStrengthTrackBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private CheckBox alignedCheckBox;
        private CheckBox automaticCheckBox;
        private TrackBar signalStrengthTrackBar;
        private Label label2;
        private Timer updateTimer;
        private Label batteryStatusLabel;
        private TrackBar batteryStatusTrackBar;
        private Label clockLabel;
        private Label syncStateLabel;
        private Button pausePlayClockButton;
        private CheckBox hasVersionMismatchCheckBox;
    }
}
