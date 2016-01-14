using System.ComponentModel;
using System.Windows.Forms;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    partial class CompetitionStateOverview
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
            this.prevRefusalsPanel = new System.Windows.Forms.Panel();
            this.prevRefusalsValuePanel = new System.Windows.Forms.Panel();
            this.prevRefusalsCaptionPanel = new System.Windows.Forms.Panel();
            this.prevRefusalsCaptionLabel = new System.Windows.Forms.Label();
            this.prevFaultsPanel = new System.Windows.Forms.Panel();
            this.prevFaultsValuePanel = new System.Windows.Forms.Panel();
            this.prevFaultsCaptionPanel = new System.Windows.Forms.Panel();
            this.prevFaultsCaptionLabel = new System.Windows.Forms.Label();
            this.prevPlacementPanel = new System.Windows.Forms.Panel();
            this.prevTimePanel = new System.Windows.Forms.Panel();
            this.currentRefusalsPanel = new System.Windows.Forms.Panel();
            this.currentRefusalsValuePanel = new System.Windows.Forms.Panel();
            this.currentRefusalsCaptionPanel = new System.Windows.Forms.Panel();
            this.currentRefusalsCaptionLabel = new System.Windows.Forms.Label();
            this.eliminationPanel = new System.Windows.Forms.Panel();
            this.eliminationCaptionPanel = new System.Windows.Forms.Panel();
            this.eliminationCaptionLabel = new System.Windows.Forms.Label();
            this.currentCompetitorNumberPanel = new System.Windows.Forms.Panel();
            this.primaryTimePanel = new System.Windows.Forms.Panel();
            this.nextCompetitorNumberPanel = new System.Windows.Forms.Panel();
            this.prevCompetitorNumberPanel = new System.Windows.Forms.Panel();
            this.prevPanel = new System.Windows.Forms.Panel();
            this.currentFaultsPanel = new System.Windows.Forms.Panel();
            this.currentFaultsValuePanel = new System.Windows.Forms.Panel();
            this.currentFaultsCaptionPanel = new System.Windows.Forms.Panel();
            this.currentFaultsCaptionLabel = new System.Windows.Forms.Label();
            this.nowPanel = new System.Windows.Forms.Panel();
            this.nextPanel = new System.Windows.Forms.Panel();
            this.displayRefreshTimer = new System.Windows.Forms.Timer(this.components);
            this.prevRefusalsValueLabel = new ScaleTextToFitLabel();
            this.prevFaultsValueLabel = new ScaleTextToFitLabel();
            this.prevPlacementLabel = new ScaleTextToFitLabel();
            this.prevTimeLabel = new ScaleTextToFitLabel();
            this.currentRefusalsValueLabel = new ScaleTextToFitLabel();
            this.currentCompetitorNumberLabel = new ScaleTextToFitLabel();
            this.primaryTimeLabel = new ScaleTextToFitLabel();
            this.nextCompetitorNumberLabel = new ScaleTextToFitLabel();
            this.prevCompetitorNumberLabel = new ScaleTextToFitLabel();
            this.prevLabel = new ScaleTextToFitLabel();
            this.currentFaultsValueLabel = new ScaleTextToFitLabel();
            this.nowLabel = new ScaleTextToFitLabel();
            this.nextLabel = new ScaleTextToFitLabel();
            this.currentCompetitorNumberHighlighter = new Highlighter();
            this.nextCompetitorNumberHighlighter = new Highlighter();
            this.prevRefusalsPanel.SuspendLayout();
            this.prevRefusalsValuePanel.SuspendLayout();
            this.prevRefusalsCaptionPanel.SuspendLayout();
            this.prevFaultsPanel.SuspendLayout();
            this.prevFaultsValuePanel.SuspendLayout();
            this.prevFaultsCaptionPanel.SuspendLayout();
            this.prevPlacementPanel.SuspendLayout();
            this.prevTimePanel.SuspendLayout();
            this.currentRefusalsPanel.SuspendLayout();
            this.currentRefusalsValuePanel.SuspendLayout();
            this.currentRefusalsCaptionPanel.SuspendLayout();
            this.eliminationPanel.SuspendLayout();
            this.eliminationCaptionPanel.SuspendLayout();
            this.currentCompetitorNumberPanel.SuspendLayout();
            this.primaryTimePanel.SuspendLayout();
            this.nextCompetitorNumberPanel.SuspendLayout();
            this.prevCompetitorNumberPanel.SuspendLayout();
            this.prevPanel.SuspendLayout();
            this.currentFaultsPanel.SuspendLayout();
            this.currentFaultsValuePanel.SuspendLayout();
            this.currentFaultsCaptionPanel.SuspendLayout();
            this.nowPanel.SuspendLayout();
            this.nextPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // prevRefusalsPanel
            // 
            this.prevRefusalsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(255)))), ((int)(((byte)(194)))));
            this.prevRefusalsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.prevRefusalsPanel.Controls.Add(this.prevRefusalsValuePanel);
            this.prevRefusalsPanel.Controls.Add(this.prevRefusalsCaptionPanel);
            this.prevRefusalsPanel.Location = new System.Drawing.Point(600, 0);
            this.prevRefusalsPanel.Name = "prevRefusalsPanel";
            this.prevRefusalsPanel.Size = new System.Drawing.Size(112, 112);
            this.prevRefusalsPanel.TabIndex = 39;
            // 
            // prevRefusalsValuePanel
            // 
            this.prevRefusalsValuePanel.Controls.Add(this.prevRefusalsValueLabel);
            this.prevRefusalsValuePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prevRefusalsValuePanel.Location = new System.Drawing.Point(0, 0);
            this.prevRefusalsValuePanel.Name = "prevRefusalsValuePanel";
            this.prevRefusalsValuePanel.Padding = new System.Windows.Forms.Padding(20);
            this.prevRefusalsValuePanel.Size = new System.Drawing.Size(110, 75);
            this.prevRefusalsValuePanel.TabIndex = 11;
            // 
            // prevRefusalsCaptionPanel
            // 
            this.prevRefusalsCaptionPanel.Controls.Add(this.prevRefusalsCaptionLabel);
            this.prevRefusalsCaptionPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.prevRefusalsCaptionPanel.Location = new System.Drawing.Point(0, 75);
            this.prevRefusalsCaptionPanel.Name = "prevRefusalsCaptionPanel";
            this.prevRefusalsCaptionPanel.Size = new System.Drawing.Size(110, 35);
            this.prevRefusalsCaptionPanel.TabIndex = 5;
            // 
            // prevRefusalsCaptionLabel
            // 
            this.prevRefusalsCaptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prevRefusalsCaptionLabel.Font = new System.Drawing.Font("Arial Narrow", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prevRefusalsCaptionLabel.ForeColor = System.Drawing.Color.DimGray;
            this.prevRefusalsCaptionLabel.Location = new System.Drawing.Point(0, 0);
            this.prevRefusalsCaptionLabel.Name = "prevRefusalsCaptionLabel";
            this.prevRefusalsCaptionLabel.Size = new System.Drawing.Size(110, 35);
            this.prevRefusalsCaptionLabel.TabIndex = 0;
            this.prevRefusalsCaptionLabel.Text = "Refusals";
            this.prevRefusalsCaptionLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // prevFaultsPanel
            // 
            this.prevFaultsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(255)))), ((int)(((byte)(194)))));
            this.prevFaultsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.prevFaultsPanel.Controls.Add(this.prevFaultsValuePanel);
            this.prevFaultsPanel.Controls.Add(this.prevFaultsCaptionPanel);
            this.prevFaultsPanel.Location = new System.Drawing.Point(486, 0);
            this.prevFaultsPanel.Name = "prevFaultsPanel";
            this.prevFaultsPanel.Size = new System.Drawing.Size(112, 112);
            this.prevFaultsPanel.TabIndex = 38;
            // 
            // prevFaultsValuePanel
            // 
            this.prevFaultsValuePanel.Controls.Add(this.prevFaultsValueLabel);
            this.prevFaultsValuePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prevFaultsValuePanel.Location = new System.Drawing.Point(0, 0);
            this.prevFaultsValuePanel.Name = "prevFaultsValuePanel";
            this.prevFaultsValuePanel.Padding = new System.Windows.Forms.Padding(20);
            this.prevFaultsValuePanel.Size = new System.Drawing.Size(110, 75);
            this.prevFaultsValuePanel.TabIndex = 11;
            // 
            // prevFaultsCaptionPanel
            // 
            this.prevFaultsCaptionPanel.Controls.Add(this.prevFaultsCaptionLabel);
            this.prevFaultsCaptionPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.prevFaultsCaptionPanel.Location = new System.Drawing.Point(0, 75);
            this.prevFaultsCaptionPanel.Name = "prevFaultsCaptionPanel";
            this.prevFaultsCaptionPanel.Size = new System.Drawing.Size(110, 35);
            this.prevFaultsCaptionPanel.TabIndex = 5;
            // 
            // prevFaultsCaptionLabel
            // 
            this.prevFaultsCaptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prevFaultsCaptionLabel.Font = new System.Drawing.Font("Arial Narrow", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prevFaultsCaptionLabel.ForeColor = System.Drawing.Color.DimGray;
            this.prevFaultsCaptionLabel.Location = new System.Drawing.Point(0, 0);
            this.prevFaultsCaptionLabel.Name = "prevFaultsCaptionLabel";
            this.prevFaultsCaptionLabel.Size = new System.Drawing.Size(110, 35);
            this.prevFaultsCaptionLabel.TabIndex = 0;
            this.prevFaultsCaptionLabel.Text = "Faults";
            this.prevFaultsCaptionLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // prevPlacementPanel
            // 
            this.prevPlacementPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(255)))), ((int)(((byte)(194)))));
            this.prevPlacementPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.prevPlacementPanel.Controls.Add(this.prevPlacementLabel);
            this.prevPlacementPanel.Location = new System.Drawing.Point(625, 114);
            this.prevPlacementPanel.Name = "prevPlacementPanel";
            this.prevPlacementPanel.Padding = new System.Windows.Forms.Padding(10);
            this.prevPlacementPanel.Size = new System.Drawing.Size(87, 72);
            this.prevPlacementPanel.TabIndex = 35;
            // 
            // prevTimePanel
            // 
            this.prevTimePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(255)))), ((int)(((byte)(194)))));
            this.prevTimePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.prevTimePanel.Controls.Add(this.prevTimeLabel);
            this.prevTimePanel.Location = new System.Drawing.Point(400, 114);
            this.prevTimePanel.Name = "prevTimePanel";
            this.prevTimePanel.Size = new System.Drawing.Size(223, 72);
            this.prevTimePanel.TabIndex = 39;
            // 
            // currentRefusalsPanel
            // 
            this.currentRefusalsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(71)))));
            this.currentRefusalsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.currentRefusalsPanel.Controls.Add(this.currentRefusalsValuePanel);
            this.currentRefusalsPanel.Controls.Add(this.currentRefusalsCaptionPanel);
            this.currentRefusalsPanel.Location = new System.Drawing.Point(200, 0);
            this.currentRefusalsPanel.Name = "currentRefusalsPanel";
            this.currentRefusalsPanel.Size = new System.Drawing.Size(112, 112);
            this.currentRefusalsPanel.TabIndex = 38;
            // 
            // currentRefusalsValuePanel
            // 
            this.currentRefusalsValuePanel.Controls.Add(this.currentRefusalsValueLabel);
            this.currentRefusalsValuePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currentRefusalsValuePanel.Location = new System.Drawing.Point(0, 0);
            this.currentRefusalsValuePanel.Name = "currentRefusalsValuePanel";
            this.currentRefusalsValuePanel.Padding = new System.Windows.Forms.Padding(20);
            this.currentRefusalsValuePanel.Size = new System.Drawing.Size(110, 75);
            this.currentRefusalsValuePanel.TabIndex = 11;
            // 
            // currentRefusalsCaptionPanel
            // 
            this.currentRefusalsCaptionPanel.Controls.Add(this.currentRefusalsCaptionLabel);
            this.currentRefusalsCaptionPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.currentRefusalsCaptionPanel.Location = new System.Drawing.Point(0, 75);
            this.currentRefusalsCaptionPanel.Name = "currentRefusalsCaptionPanel";
            this.currentRefusalsCaptionPanel.Size = new System.Drawing.Size(110, 35);
            this.currentRefusalsCaptionPanel.TabIndex = 5;
            // 
            // currentRefusalsCaptionLabel
            // 
            this.currentRefusalsCaptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currentRefusalsCaptionLabel.Font = new System.Drawing.Font("Arial Narrow", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentRefusalsCaptionLabel.ForeColor = System.Drawing.Color.DimGray;
            this.currentRefusalsCaptionLabel.Location = new System.Drawing.Point(0, 0);
            this.currentRefusalsCaptionLabel.Name = "currentRefusalsCaptionLabel";
            this.currentRefusalsCaptionLabel.Size = new System.Drawing.Size(110, 35);
            this.currentRefusalsCaptionLabel.TabIndex = 0;
            this.currentRefusalsCaptionLabel.Text = "Refusals";
            this.currentRefusalsCaptionLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // eliminationPanel
            // 
            this.eliminationPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(71)))));
            this.eliminationPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.eliminationPanel.Controls.Add(this.eliminationCaptionPanel);
            this.eliminationPanel.Location = new System.Drawing.Point(266, 114);
            this.eliminationPanel.Name = "eliminationPanel";
            this.eliminationPanel.Size = new System.Drawing.Size(46, 72);
            this.eliminationPanel.TabIndex = 14;
            // 
            // eliminationCaptionPanel
            // 
            this.eliminationCaptionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eliminationCaptionPanel.Controls.Add(this.eliminationCaptionLabel);
            this.eliminationCaptionPanel.Location = new System.Drawing.Point(-3, -3);
            this.eliminationCaptionPanel.Name = "eliminationCaptionPanel";
            this.eliminationCaptionPanel.Size = new System.Drawing.Size(44, 70);
            this.eliminationCaptionPanel.TabIndex = 5;
            // 
            // eliminationCaptionLabel
            // 
            this.eliminationCaptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eliminationCaptionLabel.Font = new System.Drawing.Font("Arial Narrow", 46.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eliminationCaptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.eliminationCaptionLabel.Location = new System.Drawing.Point(0, 0);
            this.eliminationCaptionLabel.Name = "eliminationCaptionLabel";
            this.eliminationCaptionLabel.Size = new System.Drawing.Size(44, 70);
            this.eliminationCaptionLabel.TabIndex = 0;
            this.eliminationCaptionLabel.Text = "E";
            this.eliminationCaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // currentCompetitorNumberPanel
            // 
            this.currentCompetitorNumberPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(71)))));
            this.currentCompetitorNumberPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.currentCompetitorNumberPanel.Controls.Add(this.currentCompetitorNumberLabel);
            this.currentCompetitorNumberPanel.Location = new System.Drawing.Point(0, 57);
            this.currentCompetitorNumberPanel.Name = "currentCompetitorNumberPanel";
            this.currentCompetitorNumberPanel.Padding = new System.Windows.Forms.Padding(10);
            this.currentCompetitorNumberPanel.Size = new System.Drawing.Size(84, 55);
            this.currentCompetitorNumberPanel.TabIndex = 15;
            // 
            // primaryTimePanel
            // 
            this.primaryTimePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(71)))));
            this.primaryTimePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.primaryTimePanel.Controls.Add(this.primaryTimeLabel);
            this.primaryTimePanel.Location = new System.Drawing.Point(0, 114);
            this.primaryTimePanel.Name = "primaryTimePanel";
            this.primaryTimePanel.Size = new System.Drawing.Size(264, 72);
            this.primaryTimePanel.TabIndex = 38;
            // 
            // nextCompetitorNumberPanel
            // 
            this.nextCompetitorNumberPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(194)))));
            this.nextCompetitorNumberPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nextCompetitorNumberPanel.Controls.Add(this.nextCompetitorNumberLabel);
            this.nextCompetitorNumberPanel.Location = new System.Drawing.Point(314, 57);
            this.nextCompetitorNumberPanel.Name = "nextCompetitorNumberPanel";
            this.nextCompetitorNumberPanel.Padding = new System.Windows.Forms.Padding(10);
            this.nextCompetitorNumberPanel.Size = new System.Drawing.Size(84, 55);
            this.nextCompetitorNumberPanel.TabIndex = 21;
            // 
            // prevCompetitorNumberPanel
            // 
            this.prevCompetitorNumberPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(255)))), ((int)(((byte)(194)))));
            this.prevCompetitorNumberPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.prevCompetitorNumberPanel.Controls.Add(this.prevCompetitorNumberLabel);
            this.prevCompetitorNumberPanel.Location = new System.Drawing.Point(400, 57);
            this.prevCompetitorNumberPanel.Name = "prevCompetitorNumberPanel";
            this.prevCompetitorNumberPanel.Padding = new System.Windows.Forms.Padding(10);
            this.prevCompetitorNumberPanel.Size = new System.Drawing.Size(84, 55);
            this.prevCompetitorNumberPanel.TabIndex = 24;
            // 
            // prevPanel
            // 
            this.prevPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(255)))), ((int)(((byte)(194)))));
            this.prevPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.prevPanel.Controls.Add(this.prevLabel);
            this.prevPanel.Location = new System.Drawing.Point(400, 0);
            this.prevPanel.Name = "prevPanel";
            this.prevPanel.Padding = new System.Windows.Forms.Padding(10);
            this.prevPanel.Size = new System.Drawing.Size(84, 55);
            this.prevPanel.TabIndex = 25;
            // 
            // currentFaultsPanel
            // 
            this.currentFaultsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(71)))));
            this.currentFaultsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.currentFaultsPanel.Controls.Add(this.currentFaultsValuePanel);
            this.currentFaultsPanel.Controls.Add(this.currentFaultsCaptionPanel);
            this.currentFaultsPanel.Location = new System.Drawing.Point(86, 0);
            this.currentFaultsPanel.Name = "currentFaultsPanel";
            this.currentFaultsPanel.Size = new System.Drawing.Size(112, 112);
            this.currentFaultsPanel.TabIndex = 10;
            // 
            // currentFaultsValuePanel
            // 
            this.currentFaultsValuePanel.Controls.Add(this.currentFaultsValueLabel);
            this.currentFaultsValuePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currentFaultsValuePanel.Location = new System.Drawing.Point(0, 0);
            this.currentFaultsValuePanel.Name = "currentFaultsValuePanel";
            this.currentFaultsValuePanel.Padding = new System.Windows.Forms.Padding(20);
            this.currentFaultsValuePanel.Size = new System.Drawing.Size(110, 75);
            this.currentFaultsValuePanel.TabIndex = 11;
            // 
            // currentFaultsCaptionPanel
            // 
            this.currentFaultsCaptionPanel.Controls.Add(this.currentFaultsCaptionLabel);
            this.currentFaultsCaptionPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.currentFaultsCaptionPanel.Location = new System.Drawing.Point(0, 75);
            this.currentFaultsCaptionPanel.Name = "currentFaultsCaptionPanel";
            this.currentFaultsCaptionPanel.Size = new System.Drawing.Size(110, 35);
            this.currentFaultsCaptionPanel.TabIndex = 5;
            // 
            // currentFaultsCaptionLabel
            // 
            this.currentFaultsCaptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currentFaultsCaptionLabel.Font = new System.Drawing.Font("Arial Narrow", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentFaultsCaptionLabel.ForeColor = System.Drawing.Color.DimGray;
            this.currentFaultsCaptionLabel.Location = new System.Drawing.Point(0, 0);
            this.currentFaultsCaptionLabel.Name = "currentFaultsCaptionLabel";
            this.currentFaultsCaptionLabel.Size = new System.Drawing.Size(110, 35);
            this.currentFaultsCaptionLabel.TabIndex = 0;
            this.currentFaultsCaptionLabel.Text = "Faults";
            this.currentFaultsCaptionLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // nowPanel
            // 
            this.nowPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(71)))));
            this.nowPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nowPanel.Controls.Add(this.nowLabel);
            this.nowPanel.Location = new System.Drawing.Point(0, 0);
            this.nowPanel.Name = "nowPanel";
            this.nowPanel.Padding = new System.Windows.Forms.Padding(10);
            this.nowPanel.Size = new System.Drawing.Size(84, 55);
            this.nowPanel.TabIndex = 16;
            // 
            // nextPanel
            // 
            this.nextPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(194)))));
            this.nextPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nextPanel.Controls.Add(this.nextLabel);
            this.nextPanel.Location = new System.Drawing.Point(314, 0);
            this.nextPanel.Name = "nextPanel";
            this.nextPanel.Padding = new System.Windows.Forms.Padding(10);
            this.nextPanel.Size = new System.Drawing.Size(84, 55);
            this.nextPanel.TabIndex = 18;
            // 
            // displayRefreshTimer
            // 
            this.displayRefreshTimer.Interval = 25;
            this.displayRefreshTimer.Tick += new System.EventHandler(this.DisplayRefreshTimer_Tick);
            // 
            // prevRefusalsValueLabel
            // 
            this.prevRefusalsValueLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prevRefusalsValueLabel.EnableStabilization = true;
            this.prevRefusalsValueLabel.Font = new System.Drawing.Font("Arial Narrow", 81F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prevRefusalsValueLabel.Location = new System.Drawing.Point(20, 20);
            this.prevRefusalsValueLabel.Name = "prevRefusalsValueLabel";
            this.prevRefusalsValueLabel.Size = new System.Drawing.Size(70, 35);
            this.prevRefusalsValueLabel.TabIndex = 7;
            this.prevRefusalsValueLabel.Text = "00";
            // 
            // prevFaultsValueLabel
            // 
            this.prevFaultsValueLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prevFaultsValueLabel.EnableStabilization = true;
            this.prevFaultsValueLabel.Font = new System.Drawing.Font("Arial Narrow", 81F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prevFaultsValueLabel.Location = new System.Drawing.Point(20, 20);
            this.prevFaultsValueLabel.Name = "prevFaultsValueLabel";
            this.prevFaultsValueLabel.Size = new System.Drawing.Size(70, 35);
            this.prevFaultsValueLabel.TabIndex = 7;
            this.prevFaultsValueLabel.Text = "00";
            // 
            // prevPlacementLabel
            // 
            this.prevPlacementLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prevPlacementLabel.EnableStabilization = true;
            this.prevPlacementLabel.Font = new System.Drawing.Font("Arial Narrow", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prevPlacementLabel.Location = new System.Drawing.Point(10, 10);
            this.prevPlacementLabel.Name = "prevPlacementLabel";
            this.prevPlacementLabel.Size = new System.Drawing.Size(65, 50);
            this.prevPlacementLabel.TabIndex = 3;
            this.prevPlacementLabel.Text = "999";
            // 
            // prevTimeLabel
            // 
            this.prevTimeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prevTimeLabel.EnableStabilization = true;
            this.prevTimeLabel.Location = new System.Drawing.Point(0, 0);
            this.prevTimeLabel.Name = "prevTimeLabel";
            this.prevTimeLabel.Padding = new System.Windows.Forms.Padding(20);
            this.prevTimeLabel.Size = new System.Drawing.Size(221, 70);
            this.prevTimeLabel.TabIndex = 1;
            this.prevTimeLabel.Text = "XXX.XXX";
            // 
            // currentRefusalsValueLabel
            // 
            this.currentRefusalsValueLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currentRefusalsValueLabel.EnableStabilization = true;
            this.currentRefusalsValueLabel.Font = new System.Drawing.Font("Arial Narrow", 81F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentRefusalsValueLabel.Location = new System.Drawing.Point(20, 20);
            this.currentRefusalsValueLabel.Name = "currentRefusalsValueLabel";
            this.currentRefusalsValueLabel.Size = new System.Drawing.Size(70, 35);
            this.currentRefusalsValueLabel.TabIndex = 7;
            this.currentRefusalsValueLabel.Text = "00";
            // 
            // currentCompetitorNumberLabel
            // 
            this.currentCompetitorNumberLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currentCompetitorNumberLabel.EnableStabilization = true;
            this.currentCompetitorNumberLabel.Font = new System.Drawing.Font("Arial Narrow", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentCompetitorNumberLabel.Location = new System.Drawing.Point(10, 10);
            this.currentCompetitorNumberLabel.Name = "currentCompetitorNumberLabel";
            this.currentCompetitorNumberLabel.Size = new System.Drawing.Size(62, 33);
            this.currentCompetitorNumberLabel.TabIndex = 3;
            this.currentCompetitorNumberLabel.Text = "(curr)";
            // 
            // primaryTimeLabel
            // 
            this.primaryTimeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.primaryTimeLabel.EnableStabilization = true;
            this.primaryTimeLabel.Location = new System.Drawing.Point(0, 0);
            this.primaryTimeLabel.Name = "primaryTimeLabel";
            this.primaryTimeLabel.Padding = new System.Windows.Forms.Padding(20);
            this.primaryTimeLabel.Size = new System.Drawing.Size(262, 70);
            this.primaryTimeLabel.TabIndex = 1;
            this.primaryTimeLabel.Text = "XXX.XXX";
            // 
            // nextCompetitorNumberLabel
            // 
            this.nextCompetitorNumberLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nextCompetitorNumberLabel.EnableStabilization = true;
            this.nextCompetitorNumberLabel.Font = new System.Drawing.Font("Arial Narrow", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextCompetitorNumberLabel.Location = new System.Drawing.Point(10, 10);
            this.nextCompetitorNumberLabel.Name = "nextCompetitorNumberLabel";
            this.nextCompetitorNumberLabel.Size = new System.Drawing.Size(62, 33);
            this.nextCompetitorNumberLabel.TabIndex = 3;
            this.nextCompetitorNumberLabel.Text = "(next)";
            // 
            // prevCompetitorNumberLabel
            // 
            this.prevCompetitorNumberLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prevCompetitorNumberLabel.EnableStabilization = true;
            this.prevCompetitorNumberLabel.Font = new System.Drawing.Font("Arial Narrow", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prevCompetitorNumberLabel.Location = new System.Drawing.Point(10, 10);
            this.prevCompetitorNumberLabel.Name = "prevCompetitorNumberLabel";
            this.prevCompetitorNumberLabel.Size = new System.Drawing.Size(62, 33);
            this.prevCompetitorNumberLabel.TabIndex = 3;
            this.prevCompetitorNumberLabel.Text = "(prev)";
            // 
            // prevLabel
            // 
            this.prevLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prevLabel.EnableStabilization = true;
            this.prevLabel.Font = new System.Drawing.Font("Arial Narrow", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prevLabel.ForeColor = System.Drawing.Color.DimGray;
            this.prevLabel.Location = new System.Drawing.Point(10, 10);
            this.prevLabel.Name = "prevLabel";
            this.prevLabel.Size = new System.Drawing.Size(62, 33);
            this.prevLabel.TabIndex = 3;
            this.prevLabel.Text = "Last";
            // 
            // currentFaultsValueLabel
            // 
            this.currentFaultsValueLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currentFaultsValueLabel.EnableStabilization = true;
            this.currentFaultsValueLabel.Font = new System.Drawing.Font("Arial Narrow", 81F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentFaultsValueLabel.Location = new System.Drawing.Point(20, 20);
            this.currentFaultsValueLabel.Name = "currentFaultsValueLabel";
            this.currentFaultsValueLabel.Size = new System.Drawing.Size(70, 35);
            this.currentFaultsValueLabel.TabIndex = 7;
            this.currentFaultsValueLabel.Text = "00";
            // 
            // nowLabel
            // 
            this.nowLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nowLabel.EnableStabilization = true;
            this.nowLabel.Font = new System.Drawing.Font("Arial Narrow", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nowLabel.ForeColor = System.Drawing.Color.DimGray;
            this.nowLabel.Location = new System.Drawing.Point(10, 10);
            this.nowLabel.Name = "nowLabel";
            this.nowLabel.Size = new System.Drawing.Size(62, 33);
            this.nowLabel.TabIndex = 3;
            this.nowLabel.Text = "Now";
            // 
            // nextLabel
            // 
            this.nextLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nextLabel.EnableStabilization = true;
            this.nextLabel.Font = new System.Drawing.Font("Arial Narrow", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextLabel.ForeColor = System.Drawing.Color.DimGray;
            this.nextLabel.Location = new System.Drawing.Point(10, 10);
            this.nextLabel.Name = "nextLabel";
            this.nextLabel.Size = new System.Drawing.Size(62, 33);
            this.nextLabel.TabIndex = 3;
            this.nextLabel.Text = "Next";
            // 
            // currentCompetitorNumberHighlighter
            // 
            this.currentCompetitorNumberHighlighter.HighlightColor = System.Drawing.Color.DeepSkyBlue;
            this.currentCompetitorNumberHighlighter.HighlightSpeed = 100;
            this.currentCompetitorNumberHighlighter.TargetControl = this.currentCompetitorNumberLabel;
            // 
            // nextCompetitorNumberHighlighter
            // 
            this.nextCompetitorNumberHighlighter.HighlightColor = System.Drawing.Color.DeepSkyBlue;
            this.nextCompetitorNumberHighlighter.HighlightSpeed = 100;
            this.nextCompetitorNumberHighlighter.TargetControl = this.nextCompetitorNumberLabel;
            // 
            // CompetitionStateOverview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.prevRefusalsPanel);
            this.Controls.Add(this.prevFaultsPanel);
            this.Controls.Add(this.prevPlacementPanel);
            this.Controls.Add(this.prevTimePanel);
            this.Controls.Add(this.currentRefusalsPanel);
            this.Controls.Add(this.eliminationPanel);
            this.Controls.Add(this.currentCompetitorNumberPanel);
            this.Controls.Add(this.primaryTimePanel);
            this.Controls.Add(this.nextCompetitorNumberPanel);
            this.Controls.Add(this.prevCompetitorNumberPanel);
            this.Controls.Add(this.prevPanel);
            this.Controls.Add(this.currentFaultsPanel);
            this.Controls.Add(this.nowPanel);
            this.Controls.Add(this.nextPanel);
            this.Name = "CompetitionStateOverview";
            this.Size = new System.Drawing.Size(712, 186);
            this.Load += new System.EventHandler(this.CompetitionStateOverview_Load);
            this.prevRefusalsPanel.ResumeLayout(false);
            this.prevRefusalsValuePanel.ResumeLayout(false);
            this.prevRefusalsCaptionPanel.ResumeLayout(false);
            this.prevFaultsPanel.ResumeLayout(false);
            this.prevFaultsValuePanel.ResumeLayout(false);
            this.prevFaultsCaptionPanel.ResumeLayout(false);
            this.prevPlacementPanel.ResumeLayout(false);
            this.prevTimePanel.ResumeLayout(false);
            this.currentRefusalsPanel.ResumeLayout(false);
            this.currentRefusalsValuePanel.ResumeLayout(false);
            this.currentRefusalsCaptionPanel.ResumeLayout(false);
            this.eliminationPanel.ResumeLayout(false);
            this.eliminationCaptionPanel.ResumeLayout(false);
            this.currentCompetitorNumberPanel.ResumeLayout(false);
            this.primaryTimePanel.ResumeLayout(false);
            this.nextCompetitorNumberPanel.ResumeLayout(false);
            this.prevCompetitorNumberPanel.ResumeLayout(false);
            this.prevPanel.ResumeLayout(false);
            this.currentFaultsPanel.ResumeLayout(false);
            this.currentFaultsValuePanel.ResumeLayout(false);
            this.currentFaultsCaptionPanel.ResumeLayout(false);
            this.nowPanel.ResumeLayout(false);
            this.nextPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel prevRefusalsPanel;
        private Panel prevRefusalsValuePanel;
        private ScaleTextToFitLabel prevRefusalsValueLabel;
        private Panel prevRefusalsCaptionPanel;
        private Label prevRefusalsCaptionLabel;
        private Panel prevFaultsPanel;
        private Panel prevFaultsValuePanel;
        private ScaleTextToFitLabel prevFaultsValueLabel;
        private Panel prevFaultsCaptionPanel;
        private Label prevFaultsCaptionLabel;
        private Panel prevPlacementPanel;
        private ScaleTextToFitLabel prevPlacementLabel;
        private Panel prevTimePanel;
        private ScaleTextToFitLabel prevTimeLabel;
        private Panel currentRefusalsPanel;
        private Panel currentRefusalsValuePanel;
        private ScaleTextToFitLabel currentRefusalsValueLabel;
        private Panel currentRefusalsCaptionPanel;
        private Label currentRefusalsCaptionLabel;
        private Panel eliminationPanel;
        private Panel eliminationCaptionPanel;
        private Label eliminationCaptionLabel;
        private Panel currentCompetitorNumberPanel;
        private ScaleTextToFitLabel currentCompetitorNumberLabel;
        private Panel primaryTimePanel;
        private ScaleTextToFitLabel primaryTimeLabel;
        private Panel nextCompetitorNumberPanel;
        private ScaleTextToFitLabel nextCompetitorNumberLabel;
        private Panel prevCompetitorNumberPanel;
        private ScaleTextToFitLabel prevCompetitorNumberLabel;
        private Panel prevPanel;
        private ScaleTextToFitLabel prevLabel;
        private Panel currentFaultsPanel;
        private Panel currentFaultsValuePanel;
        private ScaleTextToFitLabel currentFaultsValueLabel;
        private Panel currentFaultsCaptionPanel;
        private Label currentFaultsCaptionLabel;
        private Panel nowPanel;
        private ScaleTextToFitLabel nowLabel;
        private Panel nextPanel;
        private ScaleTextToFitLabel nextLabel;
        private Timer displayRefreshTimer;
        private Highlighter currentCompetitorNumberHighlighter;
        private Highlighter nextCompetitorNumberHighlighter;
    }
}
