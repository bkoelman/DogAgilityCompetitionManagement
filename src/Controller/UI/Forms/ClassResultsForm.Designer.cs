using System.ComponentModel;
using System.Windows.Forms;
using DogAgilityCompetition.Controller.UI.Controls;

namespace DogAgilityCompetition.Controller.UI.Forms
{
    partial class ClassResultsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClassResultsForm));
            this.exportButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.goToCompetitorButton = new System.Windows.Forms.Button();
            this.finishedPanel = new System.Windows.Forms.Panel();
            this.finishedLabel = new System.Windows.Forms.Label();
            this.eliminatedPanel = new System.Windows.Forms.Panel();
            this.eliminatedLabel = new System.Windows.Forms.Label();
            this.uncompletedPanel = new System.Windows.Forms.Panel();
            this.uncompletedLabel = new System.Windows.Forms.Label();
            this.legendaPanel = new System.Windows.Forms.Panel();
            this.runResultsGrid = new DogAgilityCompetition.Controller.UI.Controls.CompetitionRunResultsGrid();
            this.finishedPanel.SuspendLayout();
            this.eliminatedPanel.SuspendLayout();
            this.uncompletedPanel.SuspendLayout();
            this.legendaPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // exportButton
            // 
            this.exportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.exportButton.Location = new System.Drawing.Point(12, 317);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(75, 23);
            this.exportButton.TabIndex = 1;
            this.exportButton.Text = "&Export";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(812, 317);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 5;
            this.okButton.Text = "&Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(893, 317);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // refreshButton
            // 
            this.refreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.refreshButton.Location = new System.Drawing.Point(93, 317);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(142, 23);
            this.refreshButton.TabIndex = 2;
            this.refreshButton.Text = "&Recalculate placements";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // goToCompetitorButton
            // 
            this.goToCompetitorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.goToCompetitorButton.Location = new System.Drawing.Point(241, 317);
            this.goToCompetitorButton.Name = "goToCompetitorButton";
            this.goToCompetitorButton.Size = new System.Drawing.Size(100, 23);
            this.goToCompetitorButton.TabIndex = 3;
            this.goToCompetitorButton.Text = "&Go to competitor";
            this.goToCompetitorButton.UseVisualStyleBackColor = true;
            this.goToCompetitorButton.Click += new System.EventHandler(this.GoToCompetitorButton_Click);
            // 
            // finishedPanel
            // 
            this.finishedPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(255)))), ((int)(((byte)(194)))));
            this.finishedPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.finishedPanel.Controls.Add(this.finishedLabel);
            this.finishedPanel.Location = new System.Drawing.Point(99, 3);
            this.finishedPanel.Name = "finishedPanel";
            this.finishedPanel.Size = new System.Drawing.Size(90, 23);
            this.finishedPanel.TabIndex = 8;
            // 
            // finishedLabel
            // 
            this.finishedLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.finishedLabel.Location = new System.Drawing.Point(0, 0);
            this.finishedLabel.Name = "finishedLabel";
            this.finishedLabel.Size = new System.Drawing.Size(88, 21);
            this.finishedLabel.TabIndex = 0;
            this.finishedLabel.Text = "Finished";
            this.finishedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // eliminatedPanel
            // 
            this.eliminatedPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(194)))));
            this.eliminatedPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.eliminatedPanel.Controls.Add(this.eliminatedLabel);
            this.eliminatedPanel.Location = new System.Drawing.Point(195, 3);
            this.eliminatedPanel.Name = "eliminatedPanel";
            this.eliminatedPanel.Size = new System.Drawing.Size(90, 23);
            this.eliminatedPanel.TabIndex = 9;
            // 
            // eliminatedLabel
            // 
            this.eliminatedLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eliminatedLabel.Location = new System.Drawing.Point(0, 0);
            this.eliminatedLabel.Name = "eliminatedLabel";
            this.eliminatedLabel.Size = new System.Drawing.Size(88, 21);
            this.eliminatedLabel.TabIndex = 0;
            this.eliminatedLabel.Text = "Eliminated";
            this.eliminatedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uncompletedPanel
            // 
            this.uncompletedPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.uncompletedPanel.Controls.Add(this.uncompletedLabel);
            this.uncompletedPanel.Location = new System.Drawing.Point(3, 3);
            this.uncompletedPanel.Name = "uncompletedPanel";
            this.uncompletedPanel.Size = new System.Drawing.Size(90, 23);
            this.uncompletedPanel.TabIndex = 10;
            // 
            // uncompletedLabel
            // 
            this.uncompletedLabel.BackColor = System.Drawing.Color.White;
            this.uncompletedLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uncompletedLabel.Location = new System.Drawing.Point(0, 0);
            this.uncompletedLabel.Name = "uncompletedLabel";
            this.uncompletedLabel.Size = new System.Drawing.Size(88, 21);
            this.uncompletedLabel.TabIndex = 0;
            this.uncompletedLabel.Text = "Uncompleted";
            this.uncompletedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // legendaPanel
            // 
            this.legendaPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.legendaPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.legendaPanel.Controls.Add(this.uncompletedPanel);
            this.legendaPanel.Controls.Add(this.finishedPanel);
            this.legendaPanel.Controls.Add(this.eliminatedPanel);
            this.legendaPanel.Location = new System.Drawing.Point(479, 314);
            this.legendaPanel.Name = "legendaPanel";
            this.legendaPanel.Size = new System.Drawing.Size(288, 29);
            this.legendaPanel.TabIndex = 4;
            // 
            // runResultsGrid
            // 
            this.runResultsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.runResultsGrid.HideBackColors = false;
            this.runResultsGrid.IsEditable = true;
            this.runResultsGrid.Location = new System.Drawing.Point(12, 12);
            this.runResultsGrid.Name = "runResultsGrid";
            this.runResultsGrid.ShowPlacement = true;
            this.runResultsGrid.Size = new System.Drawing.Size(956, 289);
            this.runResultsGrid.TabIndex = 0;
            // 
            // ClassResultsForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(980, 352);
            this.Controls.Add(this.goToCompetitorButton);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.runResultsGrid);
            this.Controls.Add(this.legendaPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(550, 200);
            this.Name = "ClassResultsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Class Results - Dog Agility Competition Controller";
            this.Load += new System.EventHandler(this.ClassResultsForm_Load);
            this.finishedPanel.ResumeLayout(false);
            this.eliminatedPanel.ResumeLayout(false);
            this.uncompletedPanel.ResumeLayout(false);
            this.legendaPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CompetitionRunResultsGrid runResultsGrid;
        private Button exportButton;
        private Button okButton;
        private Button cancelButton;
        private Button refreshButton;
        private Button goToCompetitorButton;
        private Panel finishedPanel;
        private Label finishedLabel;
        private Panel eliminatedPanel;
        private Label eliminatedLabel;
        private Panel uncompletedPanel;
        private Label uncompletedLabel;
        private Panel legendaPanel;
    }
}