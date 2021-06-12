using System.ComponentModel;
using System.Windows.Forms;
using DogAgilityCompetition.MediatorEmulator.UI.Controls;

namespace DogAgilityCompetition.MediatorEmulator.UI.Forms
{
    partial class RemoteForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoteForm));
            this.statusUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.keypad = new DogAgilityCompetition.MediatorEmulator.UI.Controls.KeypadControl();
            this.networkStatus = new DogAgilityCompetition.MediatorEmulator.UI.Controls.NetworkStatusControl();
            this.powerStatus = new DogAgilityCompetition.MediatorEmulator.UI.Controls.PowerStatusControl();
            this.hardwareStatus = new DogAgilityCompetition.MediatorEmulator.UI.Controls.HardwareStatusControl();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusUpdateTimer
            // 
            this.statusUpdateTimer.Enabled = true;
            this.statusUpdateTimer.Interval = 1000;
            this.statusUpdateTimer.Tick += new System.EventHandler(this.StatusUpdateTimer_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.keypad);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(256, 518);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Keys";
            // 
            // keypad
            // 
            this.keypad.Location = new System.Drawing.Point(6, 19);
            this.keypad.MinimumSize = new System.Drawing.Size(243, 486);
            this.keypad.Name = "keypad";
            this.keypad.Size = new System.Drawing.Size(243, 486);
            this.keypad.TabIndex = 0;
            this.keypad.StatusChanged += new System.EventHandler(this.Keypad_StatusChanged);
            // 
            // networkStatus
            // 
            this.networkStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.networkStatus.Enabled = false;
            this.networkStatus.Location = new System.Drawing.Point(274, 74);
            this.networkStatus.Name = "networkStatus";
            this.networkStatus.Size = new System.Drawing.Size(271, 77);
            this.networkStatus.TabIndex = 2;
            this.networkStatus.StatusChanged += new System.EventHandler(this.NetworkStatus_StatusChanged);
            // 
            // powerStatus
            // 
            this.powerStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.powerStatus.Location = new System.Drawing.Point(274, 12);
            this.powerStatus.Name = "powerStatus";
            this.powerStatus.Size = new System.Drawing.Size(271, 56);
            this.powerStatus.SupportsBlink = true;
            this.powerStatus.TabIndex = 1;
            this.powerStatus.StatusChanged += new System.EventHandler(this.PowerStatus_StatusChanged);
            // 
            // hardwareStatus
            // 
            this.hardwareStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hardwareStatus.Enabled = false;
            this.hardwareStatus.Location = new System.Drawing.Point(274, 157);
            this.hardwareStatus.Name = "hardwareStatus";
            this.hardwareStatus.Size = new System.Drawing.Size(271, 211);
            this.hardwareStatus.SupportsAlignment = false;
            this.hardwareStatus.SupportsBatteryStatus = true;
            this.hardwareStatus.SupportsClock = true;
            this.hardwareStatus.TabIndex = 3;
            this.hardwareStatus.StatusChanged += new System.EventHandler(this.HardwareStatus_StatusChanged);
            // 
            // RemoteForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(557, 538);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.networkStatus);
            this.Controls.Add(this.powerStatus);
            this.Controls.Add(this.hardwareStatus);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RemoteForm";
            this.Text = "Remote";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RemoteForm_FormClosing);
            this.Load += new System.EventHandler(this.RemoteForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Timer statusUpdateTimer;
        private HardwareStatusControl hardwareStatus;
        private PowerStatusControl powerStatus;
        private NetworkStatusControl networkStatus;
        private KeypadControl keypad;
        private GroupBox groupBox1;
    }
}