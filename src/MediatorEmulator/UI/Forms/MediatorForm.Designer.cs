using System.ComponentModel;
using System.Windows.Forms;
using DogAgilityCompetition.MediatorEmulator.UI.Controls;
using DogAgilityCompetition.WinForms.Controls;

namespace DogAgilityCompetition.MediatorEmulator.UI.Forms
{
    partial class MediatorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MediatorForm));
            this.label1 = new System.Windows.Forms.Label();
            this.packetOutputPulsingLed = new LedBulb();
            this.label5 = new System.Windows.Forms.Label();
            this.packetInputPulsingLed = new LedBulb();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.stateLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.statusVersionGroupBox = new System.Windows.Forms.GroupBox();
            this.versionLinkLabel = new System.Windows.Forms.LinkLabel();
            this.statusCodeLinkLabel = new System.Windows.Forms.LinkLabel();
            this.logButton = new System.Windows.Forms.Button();
            this.portLabel = new System.Windows.Forms.Label();
            this.changePortButton = new System.Windows.Forms.Button();
            this.portGroupBox = new System.Windows.Forms.GroupBox();
            this.powerStatus = new PowerStatusControl();
            this.groupBox1.SuspendLayout();
            this.statusVersionGroupBox.SuspendLayout();
            this.portGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Status code:";
            // 
            // packetOutputPulsingLed
            // 
            this.packetOutputPulsingLed.BackColor = System.Drawing.SystemColors.Control;
            this.packetOutputPulsingLed.Color = System.Drawing.Color.FromArgb(((int)(((byte)(137)))), ((int)(((byte)(184)))), ((int)(((byte)(255)))));
            this.packetOutputPulsingLed.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.packetOutputPulsingLed.Location = new System.Drawing.Point(67, 19);
            this.packetOutputPulsingLed.Name = "packetOutputPulsingLed";
            this.packetOutputPulsingLed.On = false;
            this.packetOutputPulsingLed.Size = new System.Drawing.Size(16, 16);
            this.packetOutputPulsingLed.TabIndex = 2;
            this.packetOutputPulsingLed.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(89, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "OUT";
            // 
            // packetInputPulsingLed
            // 
            this.packetInputPulsingLed.BackColor = System.Drawing.SystemColors.Control;
            this.packetInputPulsingLed.Color = System.Drawing.Color.FromArgb(((int)(((byte)(137)))), ((int)(((byte)(184)))), ((int)(((byte)(255)))));
            this.packetInputPulsingLed.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.packetInputPulsingLed.Location = new System.Drawing.Point(6, 19);
            this.packetInputPulsingLed.Name = "packetInputPulsingLed";
            this.packetInputPulsingLed.On = false;
            this.packetInputPulsingLed.Size = new System.Drawing.Size(16, 16);
            this.packetInputPulsingLed.TabIndex = 0;
            this.packetInputPulsingLed.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(18, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "IN";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.stateLabel);
            this.groupBox1.Controls.Add(this.packetInputPulsingLed);
            this.groupBox1.Controls.Add(this.packetOutputPulsingLed);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(12, 149);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(272, 76);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection";
            // 
            // stateLabel
            // 
            this.stateLabel.AutoSize = true;
            this.stateLabel.Location = new System.Drawing.Point(6, 53);
            this.stateLabel.Name = "stateLabel";
            this.stateLabel.Size = new System.Drawing.Size(112, 13);
            this.stateLabel.TabIndex = 4;
            this.stateLabel.Text = "Status: Disconnected.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Protocol version:";
            // 
            // statusVersionGroupBox
            // 
            this.statusVersionGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusVersionGroupBox.Controls.Add(this.versionLinkLabel);
            this.statusVersionGroupBox.Controls.Add(this.label1);
            this.statusVersionGroupBox.Controls.Add(this.statusCodeLinkLabel);
            this.statusVersionGroupBox.Controls.Add(this.label2);
            this.statusVersionGroupBox.Location = new System.Drawing.Point(12, 72);
            this.statusVersionGroupBox.Name = "statusVersionGroupBox";
            this.statusVersionGroupBox.Size = new System.Drawing.Size(272, 71);
            this.statusVersionGroupBox.TabIndex = 2;
            this.statusVersionGroupBox.TabStop = false;
            this.statusVersionGroupBox.Text = "Status and version";
            // 
            // versionLinkLabel
            // 
            this.versionLinkLabel.AutoSize = true;
            this.versionLinkLabel.Location = new System.Drawing.Point(98, 47);
            this.versionLinkLabel.Name = "versionLinkLabel";
            this.versionLinkLabel.Size = new System.Drawing.Size(46, 13);
            this.versionLinkLabel.TabIndex = 3;
            this.versionLinkLabel.TabStop = true;
            this.versionLinkLabel.Text = "v0.0.0.0";
            this.versionLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.VersionLinkLabel_LinkClicked);
            // 
            // statusCodeLinkLabel
            // 
            this.statusCodeLinkLabel.AutoSize = true;
            this.statusCodeLinkLabel.Location = new System.Drawing.Point(79, 25);
            this.statusCodeLinkLabel.Name = "statusCodeLinkLabel";
            this.statusCodeLinkLabel.Size = new System.Drawing.Size(55, 13);
            this.statusCodeLinkLabel.TabIndex = 1;
            this.statusCodeLinkLabel.TabStop = true;
            this.statusCodeLinkLabel.Text = "0 (Normal)";
            this.statusCodeLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.StatusCodeLinkLabel_LinkClicked);
            // 
            // logButton
            // 
            this.logButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.logButton.Location = new System.Drawing.Point(203, 168);
            this.logButton.Name = "logButton";
            this.logButton.Size = new System.Drawing.Size(75, 23);
            this.logButton.TabIndex = 4;
            this.logButton.Text = "Log";
            this.logButton.UseVisualStyleBackColor = true;
            this.logButton.Click += new System.EventHandler(this.LogButton_Click);
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(6, 21);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(36, 13);
            this.portLabel.TabIndex = 0;
            this.portLabel.Text = "COMx";
            // 
            // changePortButton
            // 
            this.changePortButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.changePortButton.Location = new System.Drawing.Point(54, 16);
            this.changePortButton.Name = "changePortButton";
            this.changePortButton.Size = new System.Drawing.Size(32, 23);
            this.changePortButton.TabIndex = 1;
            this.changePortButton.Text = "...";
            this.changePortButton.UseVisualStyleBackColor = true;
            this.changePortButton.Click += new System.EventHandler(this.ChangePortButton_Click);
            // 
            // portGroupBox
            // 
            this.portGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.portGroupBox.Controls.Add(this.changePortButton);
            this.portGroupBox.Controls.Add(this.portLabel);
            this.portGroupBox.Location = new System.Drawing.Point(192, 14);
            this.portGroupBox.Name = "portGroupBox";
            this.portGroupBox.Size = new System.Drawing.Size(92, 52);
            this.portGroupBox.TabIndex = 1;
            this.portGroupBox.TabStop = false;
            this.portGroupBox.Text = "Port";
            // 
            // powerStatus
            // 
            this.powerStatus.Location = new System.Drawing.Point(12, 12);
            this.powerStatus.Name = "powerStatus";
            this.powerStatus.Size = new System.Drawing.Size(174, 56);
            this.powerStatus.SupportsBlink = false;
            this.powerStatus.TabIndex = 0;
            this.powerStatus.StatusChanged += new System.EventHandler(this.PowerStatus_StatusChanged);
            // 
            // MediatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 232);
            this.Controls.Add(this.logButton);
            this.Controls.Add(this.portGroupBox);
            this.Controls.Add(this.statusVersionGroupBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.powerStatus);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MediatorForm";
            this.Text = "Mediator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MediatorForm_FormClosing);
            this.Load += new System.EventHandler(this.MediatorForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusVersionGroupBox.ResumeLayout(false);
            this.statusVersionGroupBox.PerformLayout();
            this.portGroupBox.ResumeLayout(false);
            this.portGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PowerStatusControl powerStatus;
        private Label label1;
        private LedBulb packetOutputPulsingLed;
        private Label label5;
        private LedBulb packetInputPulsingLed;
        private Label label4;
        private GroupBox groupBox1;
        private Label label2;
        private GroupBox statusVersionGroupBox;
        private LinkLabel versionLinkLabel;
        private LinkLabel statusCodeLinkLabel;
        private Label stateLabel;
        private Label portLabel;
        private Button changePortButton;
        private GroupBox portGroupBox;
        private Button logButton;
    }
}