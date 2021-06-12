using System.ComponentModel;
using System.Windows.Forms;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    partial class RunHistoryLine
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
            this.competitorNumberPanel = new System.Windows.Forms.Panel();
            this.competitorNumberLabel = new SingleLineLabel();
            this.countryCodePanel = new System.Windows.Forms.Panel();
            this.countryCodeLabel = new SingleLineLabel();
            this.dogNamePanel = new System.Windows.Forms.Panel();
            this.dogNameLabel = new SingleLineLabel();
            this.handlerNamePanel = new System.Windows.Forms.Panel();
            this.handlerNameLabel = new SingleLineLabel();
            this.finishTimePanel = new System.Windows.Forms.Panel();
            this.finishTimeLabel = new SingleLineLabel();
            this.faultsPanel = new System.Windows.Forms.Panel();
            this.faultsLabel = new SingleLineLabel();
            this.refusalsPanel = new System.Windows.Forms.Panel();
            this.refusalsLabel = new SingleLineLabel();
            this.placementPanel = new System.Windows.Forms.Panel();
            this.placementLabel = new SingleLineLabel();
            this.competitorNumberPanel.SuspendLayout();
            this.countryCodePanel.SuspendLayout();
            this.dogNamePanel.SuspendLayout();
            this.handlerNamePanel.SuspendLayout();
            this.finishTimePanel.SuspendLayout();
            this.faultsPanel.SuspendLayout();
            this.refusalsPanel.SuspendLayout();
            this.placementPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // competitorNumberPanel
            // 
            this.competitorNumberPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.competitorNumberPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(194)))));
            this.competitorNumberPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.competitorNumberPanel.Controls.Add(this.competitorNumberLabel);
            this.competitorNumberPanel.Location = new System.Drawing.Point(0, 0);
            this.competitorNumberPanel.Name = "competitorNumberPanel";
            this.competitorNumberPanel.Size = new System.Drawing.Size(131, 50);
            this.competitorNumberPanel.TabIndex = 26;
            // 
            // competitorNumberLabel
            // 
            this.competitorNumberLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.competitorNumberLabel.Font = new System.Drawing.Font("Arial Narrow", 28.6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.competitorNumberLabel.Location = new System.Drawing.Point(0, 0);
            this.competitorNumberLabel.Name = "competitorNumberLabel";
            this.competitorNumberLabel.Size = new System.Drawing.Size(129, 48);
            this.competitorNumberLabel.TabIndex = 3;
            this.competitorNumberLabel.Text = "0005";
            this.competitorNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // countryCodePanel
            // 
            this.countryCodePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.countryCodePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(194)))));
            this.countryCodePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.countryCodePanel.Controls.Add(this.countryCodeLabel);
            this.countryCodePanel.Location = new System.Drawing.Point(133, 0);
            this.countryCodePanel.Name = "countryCodePanel";
            this.countryCodePanel.Size = new System.Drawing.Size(124, 50);
            this.countryCodePanel.TabIndex = 27;
            // 
            // countryCodeLabel
            // 
            this.countryCodeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.countryCodeLabel.Font = new System.Drawing.Font("Arial Narrow", 28.6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.countryCodeLabel.Location = new System.Drawing.Point(0, 0);
            this.countryCodeLabel.Name = "countryCodeLabel";
            this.countryCodeLabel.Size = new System.Drawing.Size(122, 48);
            this.countryCodeLabel.TabIndex = 3;
            this.countryCodeLabel.Text = "NL";
            this.countryCodeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dogNamePanel
            // 
            this.dogNamePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dogNamePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(194)))));
            this.dogNamePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dogNamePanel.Controls.Add(this.dogNameLabel);
            this.dogNamePanel.Location = new System.Drawing.Point(259, 0);
            this.dogNamePanel.Name = "dogNamePanel";
            this.dogNamePanel.Size = new System.Drawing.Size(182, 50);
            this.dogNamePanel.TabIndex = 28;
            // 
            // dogNameLabel
            // 
            this.dogNameLabel.AutoEllipsis = true;
            this.dogNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dogNameLabel.Font = new System.Drawing.Font("Arial Narrow", 28.6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dogNameLabel.Location = new System.Drawing.Point(0, 0);
            this.dogNameLabel.Name = "dogNameLabel";
            this.dogNameLabel.Size = new System.Drawing.Size(180, 48);
            this.dogNameLabel.TabIndex = 3;
            this.dogNameLabel.Text = "Pluto";
            this.dogNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // handlerNamePanel
            // 
            this.handlerNamePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.handlerNamePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(194)))));
            this.handlerNamePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.handlerNamePanel.Controls.Add(this.handlerNameLabel);
            this.handlerNamePanel.Location = new System.Drawing.Point(443, 0);
            this.handlerNamePanel.Name = "handlerNamePanel";
            this.handlerNamePanel.Size = new System.Drawing.Size(214, 50);
            this.handlerNamePanel.TabIndex = 29;
            // 
            // handlerNameLabel
            // 
            this.handlerNameLabel.AutoEllipsis = true;
            this.handlerNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.handlerNameLabel.Font = new System.Drawing.Font("Arial Narrow", 28.6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.handlerNameLabel.Location = new System.Drawing.Point(0, 0);
            this.handlerNameLabel.Name = "handlerNameLabel";
            this.handlerNameLabel.Size = new System.Drawing.Size(212, 48);
            this.handlerNameLabel.TabIndex = 3;
            this.handlerNameLabel.Text = "Benny Willemse";
            this.handlerNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // finishTimePanel
            // 
            this.finishTimePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.finishTimePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(194)))));
            this.finishTimePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.finishTimePanel.Controls.Add(this.finishTimeLabel);
            this.finishTimePanel.Location = new System.Drawing.Point(659, 0);
            this.finishTimePanel.Name = "finishTimePanel";
            this.finishTimePanel.Size = new System.Drawing.Size(198, 50);
            this.finishTimePanel.TabIndex = 30;
            // 
            // finishTimeLabel
            // 
            this.finishTimeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.finishTimeLabel.Font = new System.Drawing.Font("Arial Narrow", 28.6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.finishTimeLabel.Location = new System.Drawing.Point(0, 0);
            this.finishTimeLabel.Name = "finishTimeLabel";
            this.finishTimeLabel.Size = new System.Drawing.Size(196, 48);
            this.finishTimeLabel.TabIndex = 3;
            this.finishTimeLabel.Text = "028.524";
            this.finishTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // faultsPanel
            // 
            this.faultsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.faultsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(194)))));
            this.faultsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.faultsPanel.Controls.Add(this.faultsLabel);
            this.faultsPanel.Location = new System.Drawing.Point(859, 0);
            this.faultsPanel.Name = "faultsPanel";
            this.faultsPanel.Size = new System.Drawing.Size(75, 50);
            this.faultsPanel.TabIndex = 31;
            // 
            // faultsLabel
            // 
            this.faultsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.faultsLabel.Font = new System.Drawing.Font("Arial Narrow", 28.6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.faultsLabel.Location = new System.Drawing.Point(0, 0);
            this.faultsLabel.Name = "faultsLabel";
            this.faultsLabel.Size = new System.Drawing.Size(73, 48);
            this.faultsLabel.TabIndex = 3;
            this.faultsLabel.Text = "00";
            this.faultsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // refusalsPanel
            // 
            this.refusalsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.refusalsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(194)))));
            this.refusalsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.refusalsPanel.Controls.Add(this.refusalsLabel);
            this.refusalsPanel.Location = new System.Drawing.Point(936, 0);
            this.refusalsPanel.Name = "refusalsPanel";
            this.refusalsPanel.Size = new System.Drawing.Size(75, 50);
            this.refusalsPanel.TabIndex = 32;
            // 
            // refusalsLabel
            // 
            this.refusalsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.refusalsLabel.Font = new System.Drawing.Font("Arial Narrow", 28.6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refusalsLabel.Location = new System.Drawing.Point(0, 0);
            this.refusalsLabel.Name = "refusalsLabel";
            this.refusalsLabel.Size = new System.Drawing.Size(73, 48);
            this.refusalsLabel.TabIndex = 3;
            this.refusalsLabel.Text = "00";
            this.refusalsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // placementPanel
            // 
            this.placementPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.placementPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(194)))));
            this.placementPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.placementPanel.Controls.Add(this.placementLabel);
            this.placementPanel.Location = new System.Drawing.Point(1013, 0);
            this.placementPanel.Name = "placementPanel";
            this.placementPanel.Size = new System.Drawing.Size(87, 50);
            this.placementPanel.TabIndex = 33;
            // 
            // placementLabel
            // 
            this.placementLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.placementLabel.Font = new System.Drawing.Font("Arial Narrow", 28.6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.placementLabel.Location = new System.Drawing.Point(0, 0);
            this.placementLabel.Name = "placementLabel";
            this.placementLabel.Size = new System.Drawing.Size(85, 48);
            this.placementLabel.TabIndex = 3;
            this.placementLabel.Text = "999";
            this.placementLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RunHistoryLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.placementPanel);
            this.Controls.Add(this.refusalsPanel);
            this.Controls.Add(this.faultsPanel);
            this.Controls.Add(this.finishTimePanel);
            this.Controls.Add(this.handlerNamePanel);
            this.Controls.Add(this.dogNamePanel);
            this.Controls.Add(this.countryCodePanel);
            this.Controls.Add(this.competitorNumberPanel);
            this.Name = "RunHistoryLine";
            this.Size = new System.Drawing.Size(1100, 50);
            this.competitorNumberPanel.ResumeLayout(false);
            this.countryCodePanel.ResumeLayout(false);
            this.dogNamePanel.ResumeLayout(false);
            this.handlerNamePanel.ResumeLayout(false);
            this.finishTimePanel.ResumeLayout(false);
            this.faultsPanel.ResumeLayout(false);
            this.refusalsPanel.ResumeLayout(false);
            this.placementPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel competitorNumberPanel;
        private SingleLineLabel competitorNumberLabel;
        private Panel countryCodePanel;
        private SingleLineLabel countryCodeLabel;
        private Panel dogNamePanel;
        private SingleLineLabel dogNameLabel;
        private Panel handlerNamePanel;
        private SingleLineLabel handlerNameLabel;
        private Panel finishTimePanel;
        private SingleLineLabel finishTimeLabel;
        private Panel faultsPanel;
        private SingleLineLabel faultsLabel;
        private Panel refusalsPanel;
        private SingleLineLabel refusalsLabel;
        private Panel placementPanel;
        private SingleLineLabel placementLabel;

    }
}
