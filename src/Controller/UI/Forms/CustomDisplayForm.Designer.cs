using System.ComponentModel;
using System.Windows.Forms;
using DogAgilityCompetition.Controller.UI.Controls;

namespace DogAgilityCompetition.Controller.UI.Forms
{
    partial class CustomDisplayForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomDisplayForm));
            this.topPanel = new System.Windows.Forms.Panel();
            this.topLabel = new DogAgilityCompetition.Controller.UI.Controls.AutoScalingLabel();
            this.middlePanel = new System.Windows.Forms.Panel();
            this.middleLabel = new DogAgilityCompetition.Controller.UI.Controls.AutoScalingLabel();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.bottomLabel = new DogAgilityCompetition.Controller.UI.Controls.AutoScalingLabel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.clockTimer = new System.Windows.Forms.Timer(this.components);
            this.topPanel.SuspendLayout();
            this.middlePanel.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.topPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.topPanel.Controls.Add(this.topLabel);
            this.topPanel.Location = new System.Drawing.Point(12, 12);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(674, 112);
            this.topPanel.TabIndex = 0;
            // 
            // topLabel
            // 
            this.topLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(71)))));
            this.topLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topLabel.Location = new System.Drawing.Point(0, 0);
            this.topLabel.Name = "topLabel";
            this.topLabel.Size = new System.Drawing.Size(672, 110);
            this.topLabel.TabIndex = 1;
            this.topLabel.Text = "XXX";
            this.topLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // middlePanel
            // 
            this.middlePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.middlePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.middlePanel.Controls.Add(this.middleLabel);
            this.middlePanel.Location = new System.Drawing.Point(12, 130);
            this.middlePanel.Name = "middlePanel";
            this.middlePanel.Size = new System.Drawing.Size(674, 112);
            this.middlePanel.TabIndex = 1;
            // 
            // middleLabel
            // 
            this.middleLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(71)))));
            this.middleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.middleLabel.Location = new System.Drawing.Point(0, 0);
            this.middleLabel.Name = "middleLabel";
            this.middleLabel.Size = new System.Drawing.Size(672, 110);
            this.middleLabel.TabIndex = 2;
            this.middleLabel.Text = "XXX";
            this.middleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bottomPanel
            // 
            this.bottomPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bottomPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bottomPanel.Controls.Add(this.bottomLabel);
            this.bottomPanel.Location = new System.Drawing.Point(12, 248);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(674, 112);
            this.bottomPanel.TabIndex = 1;
            // 
            // bottomLabel
            // 
            this.bottomLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(71)))));
            this.bottomLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bottomLabel.Location = new System.Drawing.Point(0, 0);
            this.bottomLabel.Name = "bottomLabel";
            this.bottomLabel.Size = new System.Drawing.Size(672, 110);
            this.bottomLabel.TabIndex = 3;
            this.bottomLabel.Text = "XXX";
            this.bottomLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox.Location = new System.Drawing.Point(2, 2);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(100, 50);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 4;
            this.pictureBox.TabStop = false;
            this.pictureBox.Visible = false;
            // 
            // clockTimer
            // 
            this.clockTimer.Interval = 1000;
            this.clockTimer.Tick += new System.EventHandler(this.ClockTimer_Tick);
            // 
            // CustomDisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(698, 372);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.middlePanel);
            this.Controls.Add(this.topPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CustomDisplayForm";
            this.Text = "Custom Display Form - Dog Agility Competition Controller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CustomDisplayForm_FormClosing);
            this.SizeChanged += new System.EventHandler(this.CustomDisplayForm_SizeChanged);
            this.topPanel.ResumeLayout(false);
            this.middlePanel.ResumeLayout(false);
            this.bottomPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel topPanel;
        private Panel middlePanel;
        private Panel bottomPanel;
        private AutoScalingLabel topLabel;
        private AutoScalingLabel middleLabel;
        private AutoScalingLabel bottomLabel;
        private PictureBox pictureBox;
        private Timer clockTimer;

    }
}