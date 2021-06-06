using System.ComponentModel;
using System.Windows.Forms;
using DogAgilityCompetition.WinForms.Controls;

namespace DogAgilityCompetition.Controller.UI.Forms
{
    partial class LogForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogForm));
            this.logPanel = new System.Windows.Forms.Panel();
            this.copyLogLinkLabel = new System.Windows.Forms.LinkLabel();
            this.freezeLogTextBox = new System.Windows.Forms.CheckBox();
            this.packetOutputPulsingLed = new DogAgilityCompetition.WinForms.Controls.LedBulb();
            this.label5 = new System.Windows.Forms.Label();
            this.packetInputPulsingLed = new DogAgilityCompetition.WinForms.Controls.LedBulb();
            this.label4 = new System.Windows.Forms.Label();
            this.hideLockSleepCheckBox = new System.Windows.Forms.CheckBox();
            this.packetsRadioButton = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.allRadioButton = new System.Windows.Forms.RadioButton();
            this.nonNetworkRadioButton = new System.Windows.Forms.RadioButton();
            this.networkRadioButton = new System.Windows.Forms.RadioButton();
            this.clearLogLinkLabel = new System.Windows.Forms.LinkLabel();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.logPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // logPanel
            // 
            this.logPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logPanel.Controls.Add(this.copyLogLinkLabel);
            this.logPanel.Controls.Add(this.freezeLogTextBox);
            this.logPanel.Controls.Add(this.packetOutputPulsingLed);
            this.logPanel.Controls.Add(this.label5);
            this.logPanel.Controls.Add(this.packetInputPulsingLed);
            this.logPanel.Controls.Add(this.label4);
            this.logPanel.Controls.Add(this.hideLockSleepCheckBox);
            this.logPanel.Controls.Add(this.packetsRadioButton);
            this.logPanel.Controls.Add(this.label3);
            this.logPanel.Controls.Add(this.allRadioButton);
            this.logPanel.Controls.Add(this.nonNetworkRadioButton);
            this.logPanel.Controls.Add(this.networkRadioButton);
            this.logPanel.Controls.Add(this.clearLogLinkLabel);
            this.logPanel.Controls.Add(this.logTextBox);
            this.logPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logPanel.Location = new System.Drawing.Point(0, 0);
            this.logPanel.Name = "logPanel";
            this.logPanel.Size = new System.Drawing.Size(996, 236);
            this.logPanel.TabIndex = 6;
            // 
            // copyLogLinkLabel
            // 
            this.copyLogLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.copyLogLinkLabel.AutoSize = true;
            this.copyLogLinkLabel.Location = new System.Drawing.Point(829, 5);
            this.copyLogLinkLabel.Name = "copyLogLinkLabel";
            this.copyLogLinkLabel.Size = new System.Drawing.Size(89, 15);
            this.copyLogLinkLabel.TabIndex = 11;
            this.copyLogLinkLabel.TabStop = true;
            this.copyLogLinkLabel.Text = "Copy to clipboard";
            this.copyLogLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.CopyLogLinkLabel_LinkClicked);
            // 
            // freezeLogTextBox
            // 
            this.freezeLogTextBox.AutoSize = true;
            this.freezeLogTextBox.Location = new System.Drawing.Point(517, 4);
            this.freezeLogTextBox.Name = "freezeLogTextBox";
            this.freezeLogTextBox.Size = new System.Drawing.Size(58, 17);
            this.freezeLogTextBox.TabIndex = 6;
            this.freezeLogTextBox.Text = "Freeze";
            this.freezeLogTextBox.UseVisualStyleBackColor = true;
            this.freezeLogTextBox.CheckedChanged += new System.EventHandler(this.FreezeLogTextBox_CheckedChanged);
            // 
            // packetOutputPulsingLed
            // 
            this.packetOutputPulsingLed.BackColor = System.Drawing.Color.Transparent;
            this.packetOutputPulsingLed.On = false;
            this.packetOutputPulsingLed.Location = new System.Drawing.Point(677, 3);
            this.packetOutputPulsingLed.Name = "packetOutputPulsingLed";
            this.packetOutputPulsingLed.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.packetOutputPulsingLed.Size = new System.Drawing.Size(16, 16);
            this.packetOutputPulsingLed.TabIndex = 9;
            this.packetOutputPulsingLed.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(699, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 15);
            this.label5.TabIndex = 10;
            this.label5.Text = "OUT";
            // 
            // packetInputPulsingLed
            // 
            this.packetInputPulsingLed.BackColor = System.Drawing.Color.Transparent;
            this.packetInputPulsingLed.On = false;
            this.packetInputPulsingLed.Location = new System.Drawing.Point(613, 4);
            this.packetInputPulsingLed.Name = "packetInputPulsingLed";
            this.packetInputPulsingLed.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.packetInputPulsingLed.Size = new System.Drawing.Size(16, 16);
            this.packetInputPulsingLed.TabIndex = 7;
            this.packetInputPulsingLed.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(635, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(18, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "IN";
            // 
            // hideLockSleepCheckBox
            // 
            this.hideLockSleepCheckBox.AutoSize = true;
            this.hideLockSleepCheckBox.Location = new System.Drawing.Point(400, 4);
            this.hideLockSleepCheckBox.Name = "hideLockSleepCheckBox";
            this.hideLockSleepCheckBox.Size = new System.Drawing.Size(111, 17);
            this.hideLockSleepCheckBox.TabIndex = 5;
            this.hideLockSleepCheckBox.Text = "Hide locks/sleeps";
            this.hideLockSleepCheckBox.UseVisualStyleBackColor = true;
            this.hideLockSleepCheckBox.CheckedChanged += new System.EventHandler(this.HideLockSleepCheckBox_CheckedChanged);
            // 
            // packetsRadioButton
            // 
            this.packetsRadioButton.AutoSize = true;
            this.packetsRadioButton.Location = new System.Drawing.Point(59, 3);
            this.packetsRadioButton.Name = "packetsRadioButton";
            this.packetsRadioButton.Size = new System.Drawing.Size(86, 17);
            this.packetsRadioButton.TabIndex = 1;
            this.packetsRadioButton.TabStop = true;
            this.packetsRadioButton.Text = "Packets only";
            this.packetsRadioButton.UseVisualStyleBackColor = true;
            this.packetsRadioButton.CheckedChanged += new System.EventHandler(this.PacketsRadioButton_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "Log filter:";
            // 
            // allRadioButton
            // 
            this.allRadioButton.AutoSize = true;
            this.allRadioButton.Checked = true;
            this.allRadioButton.Location = new System.Drawing.Point(358, 3);
            this.allRadioButton.Name = "allRadioButton";
            this.allRadioButton.Size = new System.Drawing.Size(36, 17);
            this.allRadioButton.TabIndex = 4;
            this.allRadioButton.TabStop = true;
            this.allRadioButton.Text = "All";
            this.allRadioButton.UseVisualStyleBackColor = true;
            this.allRadioButton.CheckedChanged += new System.EventHandler(this.AllRadioButton_CheckedChanged);
            // 
            // nonNetworkRadioButton
            // 
            this.nonNetworkRadioButton.AutoSize = true;
            this.nonNetworkRadioButton.Location = new System.Drawing.Point(244, 3);
            this.nonNetworkRadioButton.Name = "nonNetworkRadioButton";
            this.nonNetworkRadioButton.Size = new System.Drawing.Size(108, 17);
            this.nonNetworkRadioButton.TabIndex = 3;
            this.nonNetworkRadioButton.Text = "Non-network only";
            this.nonNetworkRadioButton.UseVisualStyleBackColor = true;
            this.nonNetworkRadioButton.CheckedChanged += new System.EventHandler(this.NonNetworkRadioButton_CheckedChanged);
            // 
            // networkRadioButton
            // 
            this.networkRadioButton.AutoSize = true;
            this.networkRadioButton.Location = new System.Drawing.Point(151, 3);
            this.networkRadioButton.Name = "networkRadioButton";
            this.networkRadioButton.Size = new System.Drawing.Size(87, 17);
            this.networkRadioButton.TabIndex = 2;
            this.networkRadioButton.Text = "Network only";
            this.networkRadioButton.UseVisualStyleBackColor = true;
            this.networkRadioButton.CheckedChanged += new System.EventHandler(this.NetworkRadioButton_CheckedChanged);
            // 
            // clearLogLinkLabel
            // 
            this.clearLogLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clearLogLinkLabel.AutoSize = true;
            this.clearLogLinkLabel.Location = new System.Drawing.Point(937, 5);
            this.clearLogLinkLabel.Name = "clearLogLinkLabel";
            this.clearLogLinkLabel.Size = new System.Drawing.Size(48, 15);
            this.clearLogLinkLabel.TabIndex = 12;
            this.clearLogLinkLabel.TabStop = true;
            this.clearLogLinkLabel.Text = "Clear log";
            this.clearLogLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ClearLogLinkLabel_LinkClicked);
            // 
            // logTextBox
            // 
            this.logTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.logTextBox.Location = new System.Drawing.Point(6, 26);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logTextBox.Size = new System.Drawing.Size(985, 205);
            this.logTextBox.TabIndex = 13;
            this.logTextBox.WordWrap = false;
            // 
            // LogForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(996, 236);
            this.Controls.Add(this.logPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LogForm";
            this.Opacity = 0.9D;
            this.Text = "Application Log";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LogForm_FormClosing);
            this.Load += new System.EventHandler(this.LogForm_Load);
            this.logPanel.ResumeLayout(false);
            this.logPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel logPanel;
        private LinkLabel copyLogLinkLabel;
        private CheckBox freezeLogTextBox;
        private LedBulb packetOutputPulsingLed;
        private Label label5;
        private LedBulb packetInputPulsingLed;
        private Label label4;
        private CheckBox hideLockSleepCheckBox;
        private RadioButton packetsRadioButton;
        private Label label3;
        private RadioButton allRadioButton;
        private RadioButton nonNetworkRadioButton;
        private RadioButton networkRadioButton;
        private LinkLabel clearLogLinkLabel;
        private TextBox logTextBox;
    }
}