using System.ComponentModel;
using System.Windows.Forms;
using DogAgilityCompetition.MediatorEmulator.UI.Controls;

namespace DogAgilityCompetition.MediatorEmulator.UI.Forms
{
    partial class DisplayForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DisplayForm));
            this.statusUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.displayStatus = new DogAgilityCompetition.MediatorEmulator.UI.Controls.DisplayStatusControl();
            this.networkStatus = new DogAgilityCompetition.MediatorEmulator.UI.Controls.NetworkStatusControl();
            this.powerStatus = new DogAgilityCompetition.MediatorEmulator.UI.Controls.PowerStatusControl();
            this.hardwareStatus = new DogAgilityCompetition.MediatorEmulator.UI.Controls.HardwareStatusControl();
            this.SuspendLayout();
            // 
            // statusUpdateTimer
            // 
            this.statusUpdateTimer.Interval = 1000;
            this.statusUpdateTimer.Tick += new System.EventHandler(this.StatusUpdateTimer_Tick);
            // 
            // displayStatus
            // 
            this.displayStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.displayStatus.Location = new System.Drawing.Point(12, 74);
            this.displayStatus.Name = "displayStatus";
            this.displayStatus.Size = new System.Drawing.Size(264, 145);
            this.displayStatus.TabIndex = 1;
            // 
            // networkStatus
            // 
            this.networkStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.networkStatus.Enabled = false;
            this.networkStatus.Location = new System.Drawing.Point(12, 225);
            this.networkStatus.Name = "networkStatus";
            this.networkStatus.Size = new System.Drawing.Size(264, 77);
            this.networkStatus.TabIndex = 2;
            this.networkStatus.StatusChanged += new System.EventHandler(this.NetworkStatus_StatusChanged);
            // 
            // powerStatus
            // 
            this.powerStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.powerStatus.Location = new System.Drawing.Point(12, 12);
            this.powerStatus.Name = "powerStatus";
            this.powerStatus.Size = new System.Drawing.Size(264, 56);
            this.powerStatus.SupportsBlink = true;
            this.powerStatus.TabIndex = 0;
            this.powerStatus.StatusChanged += new System.EventHandler(this.PowerStatus_StatusChanged);
            // 
            // hardwareStatus
            // 
            this.hardwareStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hardwareStatus.Enabled = false;
            this.hardwareStatus.Location = new System.Drawing.Point(12, 308);
            this.hardwareStatus.Name = "hardwareStatus";
            this.hardwareStatus.Size = new System.Drawing.Size(264, 211);
            this.hardwareStatus.SupportsAlignment = false;
            this.hardwareStatus.SupportsBatteryStatus = true;
            this.hardwareStatus.SupportsClock = false;
            this.hardwareStatus.TabIndex = 3;
            this.hardwareStatus.StatusChanged += new System.EventHandler(this.HardwareStatus_StatusChanged);
            // 
            // DisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 524);
            this.Controls.Add(this.displayStatus);
            this.Controls.Add(this.networkStatus);
            this.Controls.Add(this.powerStatus);
            this.Controls.Add(this.hardwareStatus);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DisplayForm";
            this.Text = "Display";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DisplayForm_FormClosing);
            this.Load += new System.EventHandler(this.RemoteDisplayForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Timer statusUpdateTimer;
        private NetworkStatusControl networkStatus;
        private PowerStatusControl powerStatus;
        private HardwareStatusControl hardwareStatus;
        private DisplayStatusControl displayStatus;
    }
}