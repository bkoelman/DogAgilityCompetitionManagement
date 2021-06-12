using System.ComponentModel;
using System.Windows.Forms;

namespace DogAgilityCompetition.MediatorEmulator.UI.Controls
{
    partial class NetworkStatusControl
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
            this.rolesLabel = new System.Windows.Forms.Label();
            this.isInNetworkCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rolesLabel
            // 
            this.rolesLabel.AutoSize = true;
            this.rolesLabel.Location = new System.Drawing.Point(6, 48);
            this.rolesLabel.Name = "rolesLabel";
            this.rolesLabel.Size = new System.Drawing.Size(66, 15);
            this.rolesLabel.TabIndex = 1;
            this.rolesLabel.Text = "Roles: None";
            // 
            // isInNetworkCheckBox
            // 
            this.isInNetworkCheckBox.AutoSize = true;
            this.isInNetworkCheckBox.Location = new System.Drawing.Point(6, 19);
            this.isInNetworkCheckBox.Name = "isInNetworkCheckBox";
            this.isInNetworkCheckBox.Size = new System.Drawing.Size(86, 17);
            this.isInNetworkCheckBox.TabIndex = 0;
            this.isInNetworkCheckBox.Text = "Is in network";
            this.isInNetworkCheckBox.UseVisualStyleBackColor = true;
            this.isInNetworkCheckBox.CheckedChanged += new System.EventHandler(this.IsInNetworkCheckBox_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.isInNetworkCheckBox);
            this.groupBox1.Controls.Add(this.rolesLabel);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(208, 83);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Network status";
            // 
            // NetworkStatusControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox1);
            this.Enabled = false;
            this.Name = "NetworkStatusControl";
            this.Size = new System.Drawing.Size(214, 89);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Label rolesLabel;
        private CheckBox isInNetworkCheckBox;
        private GroupBox groupBox1;
    }
}
