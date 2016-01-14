using System.ComponentModel;
using System.Windows.Forms;
using DogAgilityCompetition.MediatorEmulator.UI.Controls;

namespace DogAgilityCompetition.MediatorEmulator.UI.Forms
{
    partial class GateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GateForm));
            this.statusUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.signalButton = new System.Windows.Forms.Button();
            this.networkStatus = new DogAgilityCompetition.MediatorEmulator.UI.Controls.NetworkStatusControl();
            this.powerStatus = new DogAgilityCompetition.MediatorEmulator.UI.Controls.PowerStatusControl();
            this.hardwareStatus = new DogAgilityCompetition.MediatorEmulator.UI.Controls.HardwareStatusControl();
            this.SuspendLayout();
            // 
            // statusUpdateTimer
            // 
            this.statusUpdateTimer.Enabled = true;
            this.statusUpdateTimer.Interval = 1000;
            this.statusUpdateTimer.Tick += new System.EventHandler(this.StatusUpdateTimer_Tick);
            // 
            // signalButton
            // 
            this.signalButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.signalButton.Location = new System.Drawing.Point(221, 34);
            this.signalButton.Name = "signalButton";
            this.signalButton.Size = new System.Drawing.Size(75, 23);
            this.signalButton.TabIndex = 1;
            this.signalButton.Text = "Signal";
            this.signalButton.UseVisualStyleBackColor = true;
            this.signalButton.Click += new System.EventHandler(this.SignalButton_Click);
            // 
            // networkStatus
            // 
            this.networkStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.networkStatus.Enabled = false;
            this.networkStatus.Location = new System.Drawing.Point(12, 74);
            this.networkStatus.Name = "networkStatus";
            this.networkStatus.Size = new System.Drawing.Size(284, 77);
            this.networkStatus.TabIndex = 2;
            this.networkStatus.StatusChanged += new System.EventHandler(this.NetworkStatus_StatusChanged);
            // 
            // powerStatus
            // 
            this.powerStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.powerStatus.Location = new System.Drawing.Point(12, 12);
            this.powerStatus.Name = "powerStatus";
            this.powerStatus.Size = new System.Drawing.Size(203, 56);
            this.powerStatus.SupportsBlink = true;
            this.powerStatus.TabIndex = 0;
            this.powerStatus.StatusChanged += new System.EventHandler(this.PowerStatus_StatusChanged);
            // 
            // hardwareStatus
            // 
            this.hardwareStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hardwareStatus.Enabled = false;
            this.hardwareStatus.Location = new System.Drawing.Point(12, 157);
            this.hardwareStatus.Name = "hardwareStatus";
            this.hardwareStatus.Size = new System.Drawing.Size(284, 211);
            this.hardwareStatus.SupportsAlignment = true;
            this.hardwareStatus.SupportsBatteryStatus = true;
            this.hardwareStatus.SupportsClock = true;
            this.hardwareStatus.TabIndex = 3;
            this.hardwareStatus.StatusChanged += new System.EventHandler(this.HardwareStatus_StatusChanged);
            // 
            // GateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 373);
            this.Controls.Add(this.signalButton);
            this.Controls.Add(this.networkStatus);
            this.Controls.Add(this.powerStatus);
            this.Controls.Add(this.hardwareStatus);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GateForm";
            this.Text = "Gate";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GateForm_FormClosing);
            this.Load += new System.EventHandler(this.GateForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Timer statusUpdateTimer;
        private HardwareStatusControl hardwareStatus;
        private PowerStatusControl powerStatus;
        private NetworkStatusControl networkStatus;
        private Button signalButton;
    }
}