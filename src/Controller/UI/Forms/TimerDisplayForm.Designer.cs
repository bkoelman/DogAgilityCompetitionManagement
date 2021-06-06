using System.ComponentModel;
using System.Windows.Forms;
using DogAgilityCompetition.Controller.UI.Controls;

namespace DogAgilityCompetition.Controller.UI.Forms
{
    partial class TimerDisplayForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimerDisplayForm));
            this.displayRefreshTimer = new System.Windows.Forms.Timer(this.components);
            this.primaryTimePanel = new System.Windows.Forms.Panel();
            this.primaryTimeLabel = new DogAgilityCompetition.Controller.UI.Controls.ScaleTextToFitLabel();
            this.currentRunPanel = new System.Windows.Forms.Panel();
            this.currentRefusalsPanel = new System.Windows.Forms.Panel();
            this.currentRefusalsValuePanel = new System.Windows.Forms.Panel();
            this.currentRefusalsValueLabel = new DogAgilityCompetition.Controller.UI.Controls.ScaleTextToFitLabel();
            this.currentRefusalsCaptionPanel = new System.Windows.Forms.Panel();
            this.currentRefusalsCaptionLabel = new System.Windows.Forms.Label();
            this.prevPlacementPanel = new System.Windows.Forms.Panel();
            this.prevPlacementLabel = new DogAgilityCompetition.Controller.UI.Controls.SingleLineLabel();
            this.prevRefusalsPanel = new System.Windows.Forms.Panel();
            this.prevRefusalsValueLabel = new DogAgilityCompetition.Controller.UI.Controls.SingleLineLabel();
            this.prevFaultsPanel = new System.Windows.Forms.Panel();
            this.prevFaultsValueLabel = new DogAgilityCompetition.Controller.UI.Controls.SingleLineLabel();
            this.prevTimePanel = new System.Windows.Forms.Panel();
            this.prevTimeLabel = new DogAgilityCompetition.Controller.UI.Controls.SingleLineLabel();
            this.infoPanel = new System.Windows.Forms.Panel();
            this.prevHandlerNamePanel = new System.Windows.Forms.Panel();
            this.prevHandlerNameLabel = new DogAgilityCompetition.Controller.UI.Controls.SingleLineLabel();
            this.prevDogNamePanel = new System.Windows.Forms.Panel();
            this.prevDogNameLabel = new DogAgilityCompetition.Controller.UI.Controls.SingleLineLabel();
            this.prevPanel = new System.Windows.Forms.Panel();
            this.prevLabel = new DogAgilityCompetition.Controller.UI.Controls.SingleLineLabel();
            this.prevCompetitorNumberPanel = new System.Windows.Forms.Panel();
            this.prevCompetitorNumberLabel = new DogAgilityCompetition.Controller.UI.Controls.SingleLineLabel();
            this.nextHandlerNamePanel = new System.Windows.Forms.Panel();
            this.nextHandlerNameLabel = new DogAgilityCompetition.Controller.UI.Controls.SingleLineLabel();
            this.nextDogNamePanel = new System.Windows.Forms.Panel();
            this.nextDogNameLabel = new DogAgilityCompetition.Controller.UI.Controls.SingleLineLabel();
            this.nextPanel = new System.Windows.Forms.Panel();
            this.nextLabel = new DogAgilityCompetition.Controller.UI.Controls.SingleLineLabel();
            this.nextCompetitorNumberPanel = new System.Windows.Forms.Panel();
            this.nextCompetitorNumberLabel = new DogAgilityCompetition.Controller.UI.Controls.SingleLineLabel();
            this.currentHandlerNamePanel = new System.Windows.Forms.Panel();
            this.currentHandlerNameLabel = new DogAgilityCompetition.Controller.UI.Controls.SingleLineLabel();
            this.currentDogNamePanel = new System.Windows.Forms.Panel();
            this.currentDogNameLabel = new DogAgilityCompetition.Controller.UI.Controls.SingleLineLabel();
            this.nowPanel = new System.Windows.Forms.Panel();
            this.nowLabel = new DogAgilityCompetition.Controller.UI.Controls.SingleLineLabel();
            this.currentCompetitorNumberPanel = new System.Windows.Forms.Panel();
            this.currentCompetitorNumberLabel = new DogAgilityCompetition.Controller.UI.Controls.SingleLineLabel();
            this.currentFaultsPanel = new System.Windows.Forms.Panel();
            this.currentFaultsValuePanel = new System.Windows.Forms.Panel();
            this.currentFaultsValueLabel = new DogAgilityCompetition.Controller.UI.Controls.ScaleTextToFitLabel();
            this.currentFaultsCaptionPanel = new System.Windows.Forms.Panel();
            this.currentFaultsCaptionLabel = new System.Windows.Forms.Label();
            this.eliminationPanel = new System.Windows.Forms.Panel();
            this.eliminationCaptionPanel = new System.Windows.Forms.Panel();
            this.eliminationCaptionLabel = new System.Windows.Forms.Label();
            this.secondaryTimePanel = new System.Windows.Forms.Panel();
            this.secondaryTimeLabel = new DogAgilityCompetition.Controller.UI.Controls.ScaleTextToFitLabel();
            this.standardCourseTimeValuePanel = new System.Windows.Forms.Panel();
            this.standardCourseTimeValueLabel = new DogAgilityCompetition.Controller.UI.Controls.SingleLineLabel();
            this.standardCourseTimeCaptionPanel = new System.Windows.Forms.Panel();
            this.standardCourseTimeCaptionLabel = new DogAgilityCompetition.Controller.UI.Controls.SingleLineLabel();
            this.gradePanel = new System.Windows.Forms.Panel();
            this.gradeLabel = new DogAgilityCompetition.Controller.UI.Controls.SingleLineLabel();
            this.classTypePanel = new System.Windows.Forms.Panel();
            this.classTypeLabel = new DogAgilityCompetition.Controller.UI.Controls.SingleLineLabel();
            this.rankingsPanel = new System.Windows.Forms.Panel();
            this.runHistoryLine5 = new DogAgilityCompetition.Controller.UI.Controls.RunHistoryLine();
            this.runHistoryLine4 = new DogAgilityCompetition.Controller.UI.Controls.RunHistoryLine();
            this.runHistoryLine3 = new DogAgilityCompetition.Controller.UI.Controls.RunHistoryLine();
            this.runHistoryLine2 = new DogAgilityCompetition.Controller.UI.Controls.RunHistoryLine();
            this.runHistoryLine1 = new DogAgilityCompetition.Controller.UI.Controls.RunHistoryLine();
            this.countryCodeCaptionPanel = new System.Windows.Forms.Panel();
            this.countryCodeCaptionLabel = new System.Windows.Forms.Label();
            this.competitorNumberCaptionPanel = new System.Windows.Forms.Panel();
            this.competitorNumberCaptionLabel = new System.Windows.Forms.Label();
            this.dogNameCaptionPanel = new System.Windows.Forms.Panel();
            this.dogNameCaptionLabel = new System.Windows.Forms.Label();
            this.handlerameCaptionPanel = new System.Windows.Forms.Panel();
            this.handlerNameCaptionLabel = new System.Windows.Forms.Label();
            this.finishTimeCaptionPanel = new System.Windows.Forms.Panel();
            this.finishTimeCaptionLabel = new System.Windows.Forms.Label();
            this.faultsCaptionPanel = new System.Windows.Forms.Panel();
            this.faultsCaptionLabel = new System.Windows.Forms.Label();
            this.refusalsCaptionPanel = new System.Windows.Forms.Panel();
            this.refusalsCaptionLabel = new System.Windows.Forms.Label();
            this.placementCaptionPanel = new System.Windows.Forms.Panel();
            this.placementCaptionLabel = new System.Windows.Forms.Label();
            this.currentCompetitorNumberHighlighter = new DogAgilityCompetition.Controller.UI.Controls.Highlighter();
            this.nextCompetitorNumberHighlighter = new DogAgilityCompetition.Controller.UI.Controls.Highlighter();
            this.secondaryTimeHighlighter = new DogAgilityCompetition.Controller.UI.Controls.Highlighter();
            this.primaryTimePanel.SuspendLayout();
            this.currentRunPanel.SuspendLayout();
            this.currentRefusalsPanel.SuspendLayout();
            this.currentRefusalsValuePanel.SuspendLayout();
            this.currentRefusalsCaptionPanel.SuspendLayout();
            this.prevPlacementPanel.SuspendLayout();
            this.prevRefusalsPanel.SuspendLayout();
            this.prevFaultsPanel.SuspendLayout();
            this.prevTimePanel.SuspendLayout();
            this.prevHandlerNamePanel.SuspendLayout();
            this.prevDogNamePanel.SuspendLayout();
            this.prevPanel.SuspendLayout();
            this.prevCompetitorNumberPanel.SuspendLayout();
            this.nextHandlerNamePanel.SuspendLayout();
            this.nextDogNamePanel.SuspendLayout();
            this.nextPanel.SuspendLayout();
            this.nextCompetitorNumberPanel.SuspendLayout();
            this.currentHandlerNamePanel.SuspendLayout();
            this.currentDogNamePanel.SuspendLayout();
            this.nowPanel.SuspendLayout();
            this.currentCompetitorNumberPanel.SuspendLayout();
            this.currentFaultsPanel.SuspendLayout();
            this.currentFaultsValuePanel.SuspendLayout();
            this.currentFaultsCaptionPanel.SuspendLayout();
            this.eliminationPanel.SuspendLayout();
            this.eliminationCaptionPanel.SuspendLayout();
            this.secondaryTimePanel.SuspendLayout();
            this.standardCourseTimeValuePanel.SuspendLayout();
            this.standardCourseTimeCaptionPanel.SuspendLayout();
            this.gradePanel.SuspendLayout();
            this.classTypePanel.SuspendLayout();
            this.rankingsPanel.SuspendLayout();
            this.countryCodeCaptionPanel.SuspendLayout();
            this.competitorNumberCaptionPanel.SuspendLayout();
            this.dogNameCaptionPanel.SuspendLayout();
            this.handlerameCaptionPanel.SuspendLayout();
            this.finishTimeCaptionPanel.SuspendLayout();
            this.faultsCaptionPanel.SuspendLayout();
            this.refusalsCaptionPanel.SuspendLayout();
            this.placementCaptionPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // displayRefreshTimer
            // 
            this.displayRefreshTimer.Interval = 25;
            this.displayRefreshTimer.Tick += new System.EventHandler(this.DisplayRefreshTimer_Tick);
            // 
            // primaryTimePanel
            // 
            this.primaryTimePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.primaryTimePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(71)))));
            this.primaryTimePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.primaryTimePanel.Controls.Add(this.primaryTimeLabel);
            this.primaryTimePanel.Location = new System.Drawing.Point(3, 3);
            this.primaryTimePanel.Name = "primaryTimePanel";
            this.primaryTimePanel.Size = new System.Drawing.Size(1043, 191);
            this.primaryTimePanel.TabIndex = 2;
            // 
            // primaryTimeLabel
            // 
            this.primaryTimeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.primaryTimeLabel.EnableStabilization = true;
            this.primaryTimeLabel.Location = new System.Drawing.Point(0, 0);
            this.primaryTimeLabel.Name = "primaryTimeLabel";
            this.primaryTimeLabel.Padding = new System.Windows.Forms.Padding(20);
            this.primaryTimeLabel.Size = new System.Drawing.Size(1041, 189);
            this.primaryTimeLabel.TabIndex = 1;
            this.primaryTimeLabel.Text = "XXX.XXX";
            // 
            // currentRunPanel
            // 
            this.currentRunPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.currentRunPanel.Controls.Add(this.currentRefusalsPanel);
            this.currentRunPanel.Controls.Add(this.prevPlacementPanel);
            this.currentRunPanel.Controls.Add(this.prevRefusalsPanel);
            this.currentRunPanel.Controls.Add(this.prevFaultsPanel);
            this.currentRunPanel.Controls.Add(this.prevTimePanel);
            this.currentRunPanel.Controls.Add(this.infoPanel);
            this.currentRunPanel.Controls.Add(this.prevHandlerNamePanel);
            this.currentRunPanel.Controls.Add(this.prevDogNamePanel);
            this.currentRunPanel.Controls.Add(this.prevPanel);
            this.currentRunPanel.Controls.Add(this.prevCompetitorNumberPanel);
            this.currentRunPanel.Controls.Add(this.nextHandlerNamePanel);
            this.currentRunPanel.Controls.Add(this.nextDogNamePanel);
            this.currentRunPanel.Controls.Add(this.nextPanel);
            this.currentRunPanel.Controls.Add(this.nextCompetitorNumberPanel);
            this.currentRunPanel.Controls.Add(this.currentHandlerNamePanel);
            this.currentRunPanel.Controls.Add(this.currentDogNamePanel);
            this.currentRunPanel.Controls.Add(this.nowPanel);
            this.currentRunPanel.Controls.Add(this.currentCompetitorNumberPanel);
            this.currentRunPanel.Controls.Add(this.currentFaultsPanel);
            this.currentRunPanel.Controls.Add(this.eliminationPanel);
            this.currentRunPanel.Controls.Add(this.primaryTimePanel);
            this.currentRunPanel.Controls.Add(this.secondaryTimePanel);
            this.currentRunPanel.Location = new System.Drawing.Point(-1, -1);
            this.currentRunPanel.Name = "currentRunPanel";
            this.currentRunPanel.Size = new System.Drawing.Size(1049, 581);
            this.currentRunPanel.TabIndex = 9;
            // 
            // currentRefusalsPanel
            // 
            this.currentRefusalsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.currentRefusalsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(71)))));
            this.currentRefusalsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.currentRefusalsPanel.Controls.Add(this.currentRefusalsValuePanel);
            this.currentRefusalsPanel.Controls.Add(this.currentRefusalsCaptionPanel);
            this.currentRefusalsPanel.Location = new System.Drawing.Point(570, 196);
            this.currentRefusalsPanel.Name = "currentRefusalsPanel";
            this.currentRefusalsPanel.Size = new System.Drawing.Size(155, 152);
            this.currentRefusalsPanel.TabIndex = 12;
            // 
            // currentRefusalsValuePanel
            // 
            this.currentRefusalsValuePanel.Controls.Add(this.currentRefusalsValueLabel);
            this.currentRefusalsValuePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currentRefusalsValuePanel.Location = new System.Drawing.Point(0, 0);
            this.currentRefusalsValuePanel.Name = "currentRefusalsValuePanel";
            this.currentRefusalsValuePanel.Padding = new System.Windows.Forms.Padding(20);
            this.currentRefusalsValuePanel.Size = new System.Drawing.Size(153, 115);
            this.currentRefusalsValuePanel.TabIndex = 11;
            // 
            // currentRefusalsValueLabel
            // 
            this.currentRefusalsValueLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currentRefusalsValueLabel.EnableStabilization = true;
            this.currentRefusalsValueLabel.Font = new System.Drawing.Font("Arial Narrow", 81F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentRefusalsValueLabel.Location = new System.Drawing.Point(20, 20);
            this.currentRefusalsValueLabel.Name = "currentRefusalsValueLabel";
            this.currentRefusalsValueLabel.Size = new System.Drawing.Size(113, 75);
            this.currentRefusalsValueLabel.TabIndex = 7;
            this.currentRefusalsValueLabel.Text = "00";
            // 
            // currentRefusalsCaptionPanel
            // 
            this.currentRefusalsCaptionPanel.Controls.Add(this.currentRefusalsCaptionLabel);
            this.currentRefusalsCaptionPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.currentRefusalsCaptionPanel.Location = new System.Drawing.Point(0, 115);
            this.currentRefusalsCaptionPanel.Name = "currentRefusalsCaptionPanel";
            this.currentRefusalsCaptionPanel.Size = new System.Drawing.Size(153, 35);
            this.currentRefusalsCaptionPanel.TabIndex = 5;
            // 
            // currentRefusalsCaptionLabel
            // 
            this.currentRefusalsCaptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currentRefusalsCaptionLabel.Font = new System.Drawing.Font("Arial Narrow", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentRefusalsCaptionLabel.ForeColor = System.Drawing.Color.DimGray;
            this.currentRefusalsCaptionLabel.Location = new System.Drawing.Point(0, 0);
            this.currentRefusalsCaptionLabel.Name = "currentRefusalsCaptionLabel";
            this.currentRefusalsCaptionLabel.Size = new System.Drawing.Size(153, 35);
            this.currentRefusalsCaptionLabel.TabIndex = 0;
            this.currentRefusalsCaptionLabel.Text = "Refusals";
            this.currentRefusalsCaptionLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // prevPlacementPanel
            // 
            this.prevPlacementPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.prevPlacementPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(255)))), ((int)(((byte)(194)))));
            this.prevPlacementPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.prevPlacementPanel.Controls.Add(this.prevPlacementLabel);
            this.prevPlacementPanel.Location = new System.Drawing.Point(959, 504);
            this.prevPlacementPanel.Name = "prevPlacementPanel";
            this.prevPlacementPanel.Size = new System.Drawing.Size(87, 75);
            this.prevPlacementPanel.TabIndex = 35;
            // 
            // prevPlacementLabel
            // 
            this.prevPlacementLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prevPlacementLabel.Font = new System.Drawing.Font("Arial Narrow", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prevPlacementLabel.Location = new System.Drawing.Point(0, 0);
            this.prevPlacementLabel.Name = "prevPlacementLabel";
            this.prevPlacementLabel.Size = new System.Drawing.Size(85, 73);
            this.prevPlacementLabel.TabIndex = 3;
            this.prevPlacementLabel.Text = "999";
            this.prevPlacementLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // prevRefusalsPanel
            // 
            this.prevRefusalsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.prevRefusalsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(255)))), ((int)(((byte)(194)))));
            this.prevRefusalsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.prevRefusalsPanel.Controls.Add(this.prevRefusalsValueLabel);
            this.prevRefusalsPanel.Location = new System.Drawing.Point(882, 504);
            this.prevRefusalsPanel.Name = "prevRefusalsPanel";
            this.prevRefusalsPanel.Size = new System.Drawing.Size(75, 75);
            this.prevRefusalsPanel.TabIndex = 33;
            // 
            // prevRefusalsValueLabel
            // 
            this.prevRefusalsValueLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prevRefusalsValueLabel.Font = new System.Drawing.Font("Arial Narrow", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prevRefusalsValueLabel.Location = new System.Drawing.Point(0, 0);
            this.prevRefusalsValueLabel.Name = "prevRefusalsValueLabel";
            this.prevRefusalsValueLabel.Size = new System.Drawing.Size(73, 73);
            this.prevRefusalsValueLabel.TabIndex = 3;
            this.prevRefusalsValueLabel.Text = "00";
            this.prevRefusalsValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // prevFaultsPanel
            // 
            this.prevFaultsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.prevFaultsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(255)))), ((int)(((byte)(194)))));
            this.prevFaultsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.prevFaultsPanel.Controls.Add(this.prevFaultsValueLabel);
            this.prevFaultsPanel.Location = new System.Drawing.Point(805, 504);
            this.prevFaultsPanel.Name = "prevFaultsPanel";
            this.prevFaultsPanel.Size = new System.Drawing.Size(75, 75);
            this.prevFaultsPanel.TabIndex = 31;
            // 
            // prevFaultsValueLabel
            // 
            this.prevFaultsValueLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prevFaultsValueLabel.Font = new System.Drawing.Font("Arial Narrow", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prevFaultsValueLabel.Location = new System.Drawing.Point(0, 0);
            this.prevFaultsValueLabel.Name = "prevFaultsValueLabel";
            this.prevFaultsValueLabel.Size = new System.Drawing.Size(73, 73);
            this.prevFaultsValueLabel.TabIndex = 3;
            this.prevFaultsValueLabel.Text = "00";
            this.prevFaultsValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // prevTimePanel
            // 
            this.prevTimePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.prevTimePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(255)))), ((int)(((byte)(194)))));
            this.prevTimePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.prevTimePanel.Controls.Add(this.prevTimeLabel);
            this.prevTimePanel.Location = new System.Drawing.Point(605, 504);
            this.prevTimePanel.Name = "prevTimePanel";
            this.prevTimePanel.Size = new System.Drawing.Size(198, 75);
            this.prevTimePanel.TabIndex = 29;
            // 
            // prevTimeLabel
            // 
            this.prevTimeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prevTimeLabel.Font = new System.Drawing.Font("Arial Narrow", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prevTimeLabel.Location = new System.Drawing.Point(0, 0);
            this.prevTimeLabel.Name = "prevTimeLabel";
            this.prevTimeLabel.Size = new System.Drawing.Size(196, 73);
            this.prevTimeLabel.TabIndex = 3;
            this.prevTimeLabel.Text = "XXX.XXX";
            this.prevTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // infoPanel
            // 
            this.infoPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.infoPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(71)))));
            this.infoPanel.BackgroundImage = global::DogAgilityCompetition.Controller.Properties.Resources.timer_logo;
            this.infoPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.infoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.infoPanel.Location = new System.Drawing.Point(805, 196);
            this.infoPanel.Name = "infoPanel";
            this.infoPanel.Size = new System.Drawing.Size(241, 306);
            this.infoPanel.TabIndex = 28;
            // 
            // prevHandlerNamePanel
            // 
            this.prevHandlerNamePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prevHandlerNamePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(255)))), ((int)(((byte)(194)))));
            this.prevHandlerNamePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.prevHandlerNamePanel.Controls.Add(this.prevHandlerNameLabel);
            this.prevHandlerNamePanel.Location = new System.Drawing.Point(446, 504);
            this.prevHandlerNamePanel.Name = "prevHandlerNamePanel";
            this.prevHandlerNamePanel.Size = new System.Drawing.Size(157, 75);
            this.prevHandlerNamePanel.TabIndex = 27;
            // 
            // prevHandlerNameLabel
            // 
            this.prevHandlerNameLabel.AutoEllipsis = true;
            this.prevHandlerNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prevHandlerNameLabel.Font = new System.Drawing.Font("Arial Narrow", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prevHandlerNameLabel.Location = new System.Drawing.Point(0, 0);
            this.prevHandlerNameLabel.Name = "prevHandlerNameLabel";
            this.prevHandlerNameLabel.Size = new System.Drawing.Size(155, 73);
            this.prevHandlerNameLabel.TabIndex = 3;
            this.prevHandlerNameLabel.Text = "Wim Langstrate";
            this.prevHandlerNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // prevDogNamePanel
            // 
            this.prevDogNamePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.prevDogNamePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(255)))), ((int)(((byte)(194)))));
            this.prevDogNamePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.prevDogNamePanel.Controls.Add(this.prevDogNameLabel);
            this.prevDogNamePanel.Location = new System.Drawing.Point(262, 504);
            this.prevDogNamePanel.Name = "prevDogNamePanel";
            this.prevDogNamePanel.Size = new System.Drawing.Size(182, 75);
            this.prevDogNamePanel.TabIndex = 26;
            // 
            // prevDogNameLabel
            // 
            this.prevDogNameLabel.AutoEllipsis = true;
            this.prevDogNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prevDogNameLabel.Font = new System.Drawing.Font("Arial Narrow", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prevDogNameLabel.Location = new System.Drawing.Point(0, 0);
            this.prevDogNameLabel.Name = "prevDogNameLabel";
            this.prevDogNameLabel.Size = new System.Drawing.Size(180, 73);
            this.prevDogNameLabel.TabIndex = 3;
            this.prevDogNameLabel.Text = "Bensja";
            this.prevDogNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // prevPanel
            // 
            this.prevPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.prevPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(255)))), ((int)(((byte)(194)))));
            this.prevPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.prevPanel.Controls.Add(this.prevLabel);
            this.prevPanel.Location = new System.Drawing.Point(136, 504);
            this.prevPanel.Name = "prevPanel";
            this.prevPanel.Size = new System.Drawing.Size(124, 75);
            this.prevPanel.TabIndex = 25;
            // 
            // prevLabel
            // 
            this.prevLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prevLabel.Font = new System.Drawing.Font("Arial Narrow", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prevLabel.ForeColor = System.Drawing.Color.DimGray;
            this.prevLabel.Location = new System.Drawing.Point(0, 0);
            this.prevLabel.Name = "prevLabel";
            this.prevLabel.Size = new System.Drawing.Size(122, 73);
            this.prevLabel.TabIndex = 3;
            this.prevLabel.Text = "Last";
            this.prevLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // prevCompetitorNumberPanel
            // 
            this.prevCompetitorNumberPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.prevCompetitorNumberPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(255)))), ((int)(((byte)(194)))));
            this.prevCompetitorNumberPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.prevCompetitorNumberPanel.Controls.Add(this.prevCompetitorNumberLabel);
            this.prevCompetitorNumberPanel.Location = new System.Drawing.Point(3, 504);
            this.prevCompetitorNumberPanel.Name = "prevCompetitorNumberPanel";
            this.prevCompetitorNumberPanel.Size = new System.Drawing.Size(131, 75);
            this.prevCompetitorNumberPanel.TabIndex = 24;
            // 
            // prevCompetitorNumberLabel
            // 
            this.prevCompetitorNumberLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prevCompetitorNumberLabel.Font = new System.Drawing.Font("Arial Narrow", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prevCompetitorNumberLabel.Location = new System.Drawing.Point(0, 0);
            this.prevCompetitorNumberLabel.Name = "prevCompetitorNumberLabel";
            this.prevCompetitorNumberLabel.Size = new System.Drawing.Size(129, 73);
            this.prevCompetitorNumberLabel.TabIndex = 3;
            this.prevCompetitorNumberLabel.Text = "(prev)";
            this.prevCompetitorNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nextHandlerNamePanel
            // 
            this.nextHandlerNamePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nextHandlerNamePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(194)))));
            this.nextHandlerNamePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nextHandlerNamePanel.Controls.Add(this.nextHandlerNameLabel);
            this.nextHandlerNamePanel.Location = new System.Drawing.Point(446, 427);
            this.nextHandlerNamePanel.Name = "nextHandlerNamePanel";
            this.nextHandlerNamePanel.Size = new System.Drawing.Size(357, 75);
            this.nextHandlerNamePanel.TabIndex = 23;
            // 
            // nextHandlerNameLabel
            // 
            this.nextHandlerNameLabel.AutoEllipsis = true;
            this.nextHandlerNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nextHandlerNameLabel.Font = new System.Drawing.Font("Arial Narrow", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextHandlerNameLabel.Location = new System.Drawing.Point(0, 0);
            this.nextHandlerNameLabel.Name = "nextHandlerNameLabel";
            this.nextHandlerNameLabel.Size = new System.Drawing.Size(355, 73);
            this.nextHandlerNameLabel.TabIndex = 3;
            this.nextHandlerNameLabel.Text = "Bianca van Gastellander";
            this.nextHandlerNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nextDogNamePanel
            // 
            this.nextDogNamePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nextDogNamePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(194)))));
            this.nextDogNamePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nextDogNamePanel.Controls.Add(this.nextDogNameLabel);
            this.nextDogNamePanel.Location = new System.Drawing.Point(262, 427);
            this.nextDogNamePanel.Name = "nextDogNamePanel";
            this.nextDogNamePanel.Size = new System.Drawing.Size(182, 75);
            this.nextDogNamePanel.TabIndex = 22;
            // 
            // nextDogNameLabel
            // 
            this.nextDogNameLabel.AutoEllipsis = true;
            this.nextDogNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nextDogNameLabel.Font = new System.Drawing.Font("Arial Narrow", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextDogNameLabel.Location = new System.Drawing.Point(0, 0);
            this.nextDogNameLabel.Name = "nextDogNameLabel";
            this.nextDogNameLabel.Size = new System.Drawing.Size(180, 73);
            this.nextDogNameLabel.TabIndex = 3;
            this.nextDogNameLabel.Text = "Donner";
            this.nextDogNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nextPanel
            // 
            this.nextPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nextPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(194)))));
            this.nextPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nextPanel.Controls.Add(this.nextLabel);
            this.nextPanel.Location = new System.Drawing.Point(136, 427);
            this.nextPanel.Name = "nextPanel";
            this.nextPanel.Size = new System.Drawing.Size(124, 75);
            this.nextPanel.TabIndex = 18;
            // 
            // nextLabel
            // 
            this.nextLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nextLabel.Font = new System.Drawing.Font("Arial Narrow", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextLabel.ForeColor = System.Drawing.Color.DimGray;
            this.nextLabel.Location = new System.Drawing.Point(0, 0);
            this.nextLabel.Name = "nextLabel";
            this.nextLabel.Size = new System.Drawing.Size(122, 73);
            this.nextLabel.TabIndex = 3;
            this.nextLabel.Text = "Next";
            this.nextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nextCompetitorNumberPanel
            // 
            this.nextCompetitorNumberPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nextCompetitorNumberPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(194)))));
            this.nextCompetitorNumberPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nextCompetitorNumberPanel.Controls.Add(this.nextCompetitorNumberLabel);
            this.nextCompetitorNumberPanel.Location = new System.Drawing.Point(3, 427);
            this.nextCompetitorNumberPanel.Name = "nextCompetitorNumberPanel";
            this.nextCompetitorNumberPanel.Size = new System.Drawing.Size(131, 75);
            this.nextCompetitorNumberPanel.TabIndex = 21;
            // 
            // nextCompetitorNumberLabel
            // 
            this.nextCompetitorNumberLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nextCompetitorNumberLabel.Font = new System.Drawing.Font("Arial Narrow", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextCompetitorNumberLabel.Location = new System.Drawing.Point(0, 0);
            this.nextCompetitorNumberLabel.Name = "nextCompetitorNumberLabel";
            this.nextCompetitorNumberLabel.Size = new System.Drawing.Size(129, 73);
            this.nextCompetitorNumberLabel.TabIndex = 3;
            this.nextCompetitorNumberLabel.Text = "(next)";
            this.nextCompetitorNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // currentHandlerNamePanel
            // 
            this.currentHandlerNamePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.currentHandlerNamePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(71)))));
            this.currentHandlerNamePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.currentHandlerNamePanel.Controls.Add(this.currentHandlerNameLabel);
            this.currentHandlerNamePanel.Location = new System.Drawing.Point(446, 350);
            this.currentHandlerNamePanel.Name = "currentHandlerNamePanel";
            this.currentHandlerNamePanel.Size = new System.Drawing.Size(357, 75);
            this.currentHandlerNamePanel.TabIndex = 20;
            // 
            // currentHandlerNameLabel
            // 
            this.currentHandlerNameLabel.AutoEllipsis = true;
            this.currentHandlerNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currentHandlerNameLabel.Font = new System.Drawing.Font("Arial Narrow", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentHandlerNameLabel.Location = new System.Drawing.Point(0, 0);
            this.currentHandlerNameLabel.Name = "currentHandlerNameLabel";
            this.currentHandlerNameLabel.Size = new System.Drawing.Size(355, 73);
            this.currentHandlerNameLabel.TabIndex = 3;
            this.currentHandlerNameLabel.Text = "Tommy Verhagen-Janesen";
            this.currentHandlerNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // currentDogNamePanel
            // 
            this.currentDogNamePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.currentDogNamePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(71)))));
            this.currentDogNamePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.currentDogNamePanel.Controls.Add(this.currentDogNameLabel);
            this.currentDogNamePanel.Location = new System.Drawing.Point(262, 350);
            this.currentDogNamePanel.Name = "currentDogNamePanel";
            this.currentDogNamePanel.Size = new System.Drawing.Size(182, 75);
            this.currentDogNamePanel.TabIndex = 18;
            // 
            // currentDogNameLabel
            // 
            this.currentDogNameLabel.AutoEllipsis = true;
            this.currentDogNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currentDogNameLabel.Font = new System.Drawing.Font("Arial Narrow", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentDogNameLabel.Location = new System.Drawing.Point(0, 0);
            this.currentDogNameLabel.Name = "currentDogNameLabel";
            this.currentDogNameLabel.Size = new System.Drawing.Size(180, 73);
            this.currentDogNameLabel.TabIndex = 3;
            this.currentDogNameLabel.Text = "Schally";
            this.currentDogNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nowPanel
            // 
            this.nowPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nowPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(71)))));
            this.nowPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nowPanel.Controls.Add(this.nowLabel);
            this.nowPanel.Location = new System.Drawing.Point(136, 350);
            this.nowPanel.Name = "nowPanel";
            this.nowPanel.Size = new System.Drawing.Size(124, 75);
            this.nowPanel.TabIndex = 16;
            // 
            // nowLabel
            // 
            this.nowLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nowLabel.Font = new System.Drawing.Font("Arial Narrow", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nowLabel.ForeColor = System.Drawing.Color.DimGray;
            this.nowLabel.Location = new System.Drawing.Point(0, 0);
            this.nowLabel.Name = "nowLabel";
            this.nowLabel.Size = new System.Drawing.Size(122, 73);
            this.nowLabel.TabIndex = 3;
            this.nowLabel.Text = "Now";
            this.nowLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // currentCompetitorNumberPanel
            // 
            this.currentCompetitorNumberPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.currentCompetitorNumberPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(71)))));
            this.currentCompetitorNumberPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.currentCompetitorNumberPanel.Controls.Add(this.currentCompetitorNumberLabel);
            this.currentCompetitorNumberPanel.Location = new System.Drawing.Point(3, 350);
            this.currentCompetitorNumberPanel.Name = "currentCompetitorNumberPanel";
            this.currentCompetitorNumberPanel.Size = new System.Drawing.Size(131, 75);
            this.currentCompetitorNumberPanel.TabIndex = 15;
            // 
            // currentCompetitorNumberLabel
            // 
            this.currentCompetitorNumberLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currentCompetitorNumberLabel.Font = new System.Drawing.Font("Arial Narrow", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentCompetitorNumberLabel.Location = new System.Drawing.Point(0, 0);
            this.currentCompetitorNumberLabel.Name = "currentCompetitorNumberLabel";
            this.currentCompetitorNumberLabel.Size = new System.Drawing.Size(129, 73);
            this.currentCompetitorNumberLabel.TabIndex = 3;
            this.currentCompetitorNumberLabel.Text = "(curr)";
            this.currentCompetitorNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // currentFaultsPanel
            // 
            this.currentFaultsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.currentFaultsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(71)))));
            this.currentFaultsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.currentFaultsPanel.Controls.Add(this.currentFaultsValuePanel);
            this.currentFaultsPanel.Controls.Add(this.currentFaultsCaptionPanel);
            this.currentFaultsPanel.Location = new System.Drawing.Point(413, 196);
            this.currentFaultsPanel.Name = "currentFaultsPanel";
            this.currentFaultsPanel.Size = new System.Drawing.Size(155, 152);
            this.currentFaultsPanel.TabIndex = 10;
            // 
            // currentFaultsValuePanel
            // 
            this.currentFaultsValuePanel.Controls.Add(this.currentFaultsValueLabel);
            this.currentFaultsValuePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currentFaultsValuePanel.Location = new System.Drawing.Point(0, 0);
            this.currentFaultsValuePanel.Name = "currentFaultsValuePanel";
            this.currentFaultsValuePanel.Padding = new System.Windows.Forms.Padding(20);
            this.currentFaultsValuePanel.Size = new System.Drawing.Size(153, 115);
            this.currentFaultsValuePanel.TabIndex = 11;
            // 
            // currentFaultsValueLabel
            // 
            this.currentFaultsValueLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currentFaultsValueLabel.EnableStabilization = true;
            this.currentFaultsValueLabel.Font = new System.Drawing.Font("Arial Narrow", 81F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentFaultsValueLabel.Location = new System.Drawing.Point(20, 20);
            this.currentFaultsValueLabel.Name = "currentFaultsValueLabel";
            this.currentFaultsValueLabel.Size = new System.Drawing.Size(113, 75);
            this.currentFaultsValueLabel.TabIndex = 7;
            this.currentFaultsValueLabel.Text = "00";
            // 
            // currentFaultsCaptionPanel
            // 
            this.currentFaultsCaptionPanel.Controls.Add(this.currentFaultsCaptionLabel);
            this.currentFaultsCaptionPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.currentFaultsCaptionPanel.Location = new System.Drawing.Point(0, 115);
            this.currentFaultsCaptionPanel.Name = "currentFaultsCaptionPanel";
            this.currentFaultsCaptionPanel.Size = new System.Drawing.Size(153, 35);
            this.currentFaultsCaptionPanel.TabIndex = 5;
            // 
            // currentFaultsCaptionLabel
            // 
            this.currentFaultsCaptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currentFaultsCaptionLabel.Font = new System.Drawing.Font("Arial Narrow", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentFaultsCaptionLabel.ForeColor = System.Drawing.Color.DimGray;
            this.currentFaultsCaptionLabel.Location = new System.Drawing.Point(0, 0);
            this.currentFaultsCaptionLabel.Name = "currentFaultsCaptionLabel";
            this.currentFaultsCaptionLabel.Size = new System.Drawing.Size(153, 35);
            this.currentFaultsCaptionLabel.TabIndex = 0;
            this.currentFaultsCaptionLabel.Text = "Faults";
            this.currentFaultsCaptionLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // eliminationPanel
            // 
            this.eliminationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.eliminationPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(71)))));
            this.eliminationPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.eliminationPanel.Controls.Add(this.eliminationCaptionPanel);
            this.eliminationPanel.Location = new System.Drawing.Point(727, 196);
            this.eliminationPanel.Name = "eliminationPanel";
            this.eliminationPanel.Size = new System.Drawing.Size(76, 152);
            this.eliminationPanel.TabIndex = 14;
            // 
            // eliminationCaptionPanel
            // 
            this.eliminationCaptionPanel.Controls.Add(this.eliminationCaptionLabel);
            this.eliminationCaptionPanel.Location = new System.Drawing.Point(3, 36);
            this.eliminationCaptionPanel.Name = "eliminationCaptionPanel";
            this.eliminationCaptionPanel.Size = new System.Drawing.Size(69, 78);
            this.eliminationCaptionPanel.TabIndex = 5;
            // 
            // eliminationCaptionLabel
            // 
            this.eliminationCaptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eliminationCaptionLabel.Font = new System.Drawing.Font("Arial Narrow", 46.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eliminationCaptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.eliminationCaptionLabel.Location = new System.Drawing.Point(0, 0);
            this.eliminationCaptionLabel.Name = "eliminationCaptionLabel";
            this.eliminationCaptionLabel.Size = new System.Drawing.Size(69, 78);
            this.eliminationCaptionLabel.TabIndex = 0;
            this.eliminationCaptionLabel.Text = "E";
            this.eliminationCaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // secondaryTimePanel
            // 
            this.secondaryTimePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.secondaryTimePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(71)))));
            this.secondaryTimePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.secondaryTimePanel.Controls.Add(this.secondaryTimeLabel);
            this.secondaryTimePanel.Location = new System.Drawing.Point(3, 196);
            this.secondaryTimePanel.Name = "secondaryTimePanel";
            this.secondaryTimePanel.Size = new System.Drawing.Size(408, 152);
            this.secondaryTimePanel.TabIndex = 3;
            // 
            // secondaryTimeLabel
            // 
            this.secondaryTimeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.secondaryTimeLabel.EnableStabilization = true;
            this.secondaryTimeLabel.Location = new System.Drawing.Point(0, 0);
            this.secondaryTimeLabel.Name = "secondaryTimeLabel";
            this.secondaryTimeLabel.Padding = new System.Windows.Forms.Padding(20, 36, 20, 36);
            this.secondaryTimeLabel.Size = new System.Drawing.Size(406, 150);
            this.secondaryTimeLabel.TabIndex = 0;
            this.secondaryTimeLabel.Text = "XXX.XXX";
            // 
            // standardCourseTimeValuePanel
            // 
            this.standardCourseTimeValuePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.standardCourseTimeValuePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.standardCourseTimeValuePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.standardCourseTimeValuePanel.Controls.Add(this.standardCourseTimeValueLabel);
            this.standardCourseTimeValuePanel.Location = new System.Drawing.Point(892, 631);
            this.standardCourseTimeValuePanel.Name = "standardCourseTimeValuePanel";
            this.standardCourseTimeValuePanel.Size = new System.Drawing.Size(153, 112);
            this.standardCourseTimeValuePanel.TabIndex = 33;
            // 
            // standardCourseTimeValueLabel
            // 
            this.standardCourseTimeValueLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.standardCourseTimeValueLabel.Font = new System.Drawing.Font("Arial Narrow", 65.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.standardCourseTimeValueLabel.Location = new System.Drawing.Point(0, 0);
            this.standardCourseTimeValueLabel.Name = "standardCourseTimeValueLabel";
            this.standardCourseTimeValueLabel.Size = new System.Drawing.Size(151, 110);
            this.standardCourseTimeValueLabel.TabIndex = 3;
            this.standardCourseTimeValueLabel.Text = "45";
            this.standardCourseTimeValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // standardCourseTimeCaptionPanel
            // 
            this.standardCourseTimeCaptionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.standardCourseTimeCaptionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.standardCourseTimeCaptionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.standardCourseTimeCaptionPanel.Controls.Add(this.standardCourseTimeCaptionLabel);
            this.standardCourseTimeCaptionPanel.Location = new System.Drawing.Point(722, 631);
            this.standardCourseTimeCaptionPanel.Name = "standardCourseTimeCaptionPanel";
            this.standardCourseTimeCaptionPanel.Size = new System.Drawing.Size(168, 112);
            this.standardCourseTimeCaptionPanel.TabIndex = 32;
            // 
            // standardCourseTimeCaptionLabel
            // 
            this.standardCourseTimeCaptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.standardCourseTimeCaptionLabel.Font = new System.Drawing.Font("Arial Narrow", 65.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.standardCourseTimeCaptionLabel.ForeColor = System.Drawing.Color.DimGray;
            this.standardCourseTimeCaptionLabel.Location = new System.Drawing.Point(0, 0);
            this.standardCourseTimeCaptionLabel.Name = "standardCourseTimeCaptionLabel";
            this.standardCourseTimeCaptionLabel.Size = new System.Drawing.Size(166, 110);
            this.standardCourseTimeCaptionLabel.TabIndex = 3;
            this.standardCourseTimeCaptionLabel.Text = "SCT";
            this.standardCourseTimeCaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradePanel
            // 
            this.gradePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gradePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.gradePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gradePanel.Controls.Add(this.gradeLabel);
            this.gradePanel.Location = new System.Drawing.Point(2, 631);
            this.gradePanel.Name = "gradePanel";
            this.gradePanel.Size = new System.Drawing.Size(380, 112);
            this.gradePanel.TabIndex = 34;
            // 
            // gradeLabel
            // 
            this.gradeLabel.AutoEllipsis = true;
            this.gradeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradeLabel.Font = new System.Drawing.Font("Arial Narrow", 65.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gradeLabel.Location = new System.Drawing.Point(0, 0);
            this.gradeLabel.Name = "gradeLabel";
            this.gradeLabel.Size = new System.Drawing.Size(378, 110);
            this.gradeLabel.TabIndex = 3;
            this.gradeLabel.Text = "B1 small";
            this.gradeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // classTypePanel
            // 
            this.classTypePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.classTypePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.classTypePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.classTypePanel.Controls.Add(this.classTypeLabel);
            this.classTypePanel.Location = new System.Drawing.Point(384, 631);
            this.classTypePanel.Name = "classTypePanel";
            this.classTypePanel.Size = new System.Drawing.Size(336, 112);
            this.classTypePanel.TabIndex = 35;
            // 
            // classTypeLabel
            // 
            this.classTypeLabel.AutoEllipsis = true;
            this.classTypeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.classTypeLabel.Font = new System.Drawing.Font("Arial Narrow", 65.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.classTypeLabel.Location = new System.Drawing.Point(0, 0);
            this.classTypeLabel.Name = "classTypeLabel";
            this.classTypeLabel.Size = new System.Drawing.Size(334, 110);
            this.classTypeLabel.TabIndex = 3;
            this.classTypeLabel.Text = "Agility";
            this.classTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rankingsPanel
            // 
            this.rankingsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rankingsPanel.Controls.Add(this.runHistoryLine5);
            this.rankingsPanel.Controls.Add(this.runHistoryLine4);
            this.rankingsPanel.Controls.Add(this.runHistoryLine3);
            this.rankingsPanel.Controls.Add(this.runHistoryLine2);
            this.rankingsPanel.Controls.Add(this.runHistoryLine1);
            this.rankingsPanel.Controls.Add(this.countryCodeCaptionPanel);
            this.rankingsPanel.Controls.Add(this.competitorNumberCaptionPanel);
            this.rankingsPanel.Controls.Add(this.dogNameCaptionPanel);
            this.rankingsPanel.Controls.Add(this.handlerameCaptionPanel);
            this.rankingsPanel.Controls.Add(this.finishTimeCaptionPanel);
            this.rankingsPanel.Controls.Add(this.faultsCaptionPanel);
            this.rankingsPanel.Controls.Add(this.refusalsCaptionPanel);
            this.rankingsPanel.Controls.Add(this.placementCaptionPanel);
            this.rankingsPanel.Location = new System.Drawing.Point(-1, 580);
            this.rankingsPanel.Name = "rankingsPanel";
            this.rankingsPanel.Size = new System.Drawing.Size(1049, 49);
            this.rankingsPanel.TabIndex = 44;
            // 
            // runHistoryLine5
            // 
            this.runHistoryLine5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.runHistoryLine5.Location = new System.Drawing.Point(3, 273);
            this.runHistoryLine5.Name = "runHistoryLine5";
            this.runHistoryLine5.Size = new System.Drawing.Size(1043, 55);
            this.runHistoryLine5.TabIndex = 56;
            // 
            // runHistoryLine4
            // 
            this.runHistoryLine4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.runHistoryLine4.Location = new System.Drawing.Point(3, 216);
            this.runHistoryLine4.Name = "runHistoryLine4";
            this.runHistoryLine4.Size = new System.Drawing.Size(1043, 55);
            this.runHistoryLine4.TabIndex = 55;
            // 
            // runHistoryLine3
            // 
            this.runHistoryLine3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.runHistoryLine3.Location = new System.Drawing.Point(3, 159);
            this.runHistoryLine3.Name = "runHistoryLine3";
            this.runHistoryLine3.Size = new System.Drawing.Size(1043, 55);
            this.runHistoryLine3.TabIndex = 54;
            // 
            // runHistoryLine2
            // 
            this.runHistoryLine2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.runHistoryLine2.Location = new System.Drawing.Point(3, 102);
            this.runHistoryLine2.Name = "runHistoryLine2";
            this.runHistoryLine2.Size = new System.Drawing.Size(1043, 55);
            this.runHistoryLine2.TabIndex = 53;
            // 
            // runHistoryLine1
            // 
            this.runHistoryLine1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.runHistoryLine1.Location = new System.Drawing.Point(3, 45);
            this.runHistoryLine1.Name = "runHistoryLine1";
            this.runHistoryLine1.Size = new System.Drawing.Size(1043, 55);
            this.runHistoryLine1.TabIndex = 52;
            // 
            // countryCodeCaptionPanel
            // 
            this.countryCodeCaptionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(194)))));
            this.countryCodeCaptionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.countryCodeCaptionPanel.Controls.Add(this.countryCodeCaptionLabel);
            this.countryCodeCaptionPanel.Location = new System.Drawing.Point(136, 0);
            this.countryCodeCaptionPanel.Name = "countryCodeCaptionPanel";
            this.countryCodeCaptionPanel.Size = new System.Drawing.Size(124, 43);
            this.countryCodeCaptionPanel.TabIndex = 45;
            // 
            // countryCodeCaptionLabel
            // 
            this.countryCodeCaptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.countryCodeCaptionLabel.Font = new System.Drawing.Font("Arial Narrow", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.countryCodeCaptionLabel.ForeColor = System.Drawing.Color.DimGray;
            this.countryCodeCaptionLabel.Location = new System.Drawing.Point(0, 0);
            this.countryCodeCaptionLabel.Name = "countryCodeCaptionLabel";
            this.countryCodeCaptionLabel.Size = new System.Drawing.Size(122, 41);
            this.countryCodeCaptionLabel.TabIndex = 3;
            this.countryCodeCaptionLabel.Text = "Country";
            this.countryCodeCaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // competitorNumberCaptionPanel
            // 
            this.competitorNumberCaptionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(194)))));
            this.competitorNumberCaptionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.competitorNumberCaptionPanel.Controls.Add(this.competitorNumberCaptionLabel);
            this.competitorNumberCaptionPanel.Location = new System.Drawing.Point(3, 0);
            this.competitorNumberCaptionPanel.Name = "competitorNumberCaptionPanel";
            this.competitorNumberCaptionPanel.Size = new System.Drawing.Size(131, 43);
            this.competitorNumberCaptionPanel.TabIndex = 44;
            // 
            // competitorNumberCaptionLabel
            // 
            this.competitorNumberCaptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.competitorNumberCaptionLabel.Font = new System.Drawing.Font("Arial Narrow", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.competitorNumberCaptionLabel.ForeColor = System.Drawing.Color.DimGray;
            this.competitorNumberCaptionLabel.Location = new System.Drawing.Point(0, 0);
            this.competitorNumberCaptionLabel.Name = "competitorNumberCaptionLabel";
            this.competitorNumberCaptionLabel.Size = new System.Drawing.Size(129, 41);
            this.competitorNumberCaptionLabel.TabIndex = 3;
            this.competitorNumberCaptionLabel.Text = "Nr.";
            this.competitorNumberCaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dogNameCaptionPanel
            // 
            this.dogNameCaptionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(194)))));
            this.dogNameCaptionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dogNameCaptionPanel.Controls.Add(this.dogNameCaptionLabel);
            this.dogNameCaptionPanel.Location = new System.Drawing.Point(262, 0);
            this.dogNameCaptionPanel.Name = "dogNameCaptionPanel";
            this.dogNameCaptionPanel.Size = new System.Drawing.Size(182, 43);
            this.dogNameCaptionPanel.TabIndex = 46;
            // 
            // dogNameCaptionLabel
            // 
            this.dogNameCaptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dogNameCaptionLabel.Font = new System.Drawing.Font("Arial Narrow", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dogNameCaptionLabel.ForeColor = System.Drawing.Color.DimGray;
            this.dogNameCaptionLabel.Location = new System.Drawing.Point(0, 0);
            this.dogNameCaptionLabel.Name = "dogNameCaptionLabel";
            this.dogNameCaptionLabel.Size = new System.Drawing.Size(180, 41);
            this.dogNameCaptionLabel.TabIndex = 3;
            this.dogNameCaptionLabel.Text = "Dog";
            this.dogNameCaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // handlerameCaptionPanel
            // 
            this.handlerameCaptionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.handlerameCaptionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(194)))));
            this.handlerameCaptionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.handlerameCaptionPanel.Controls.Add(this.handlerNameCaptionLabel);
            this.handlerameCaptionPanel.Location = new System.Drawing.Point(446, 0);
            this.handlerameCaptionPanel.Name = "handlerameCaptionPanel";
            this.handlerameCaptionPanel.Size = new System.Drawing.Size(157, 43);
            this.handlerameCaptionPanel.TabIndex = 47;
            // 
            // handlerNameCaptionLabel
            // 
            this.handlerNameCaptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.handlerNameCaptionLabel.Font = new System.Drawing.Font("Arial Narrow", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.handlerNameCaptionLabel.ForeColor = System.Drawing.Color.DimGray;
            this.handlerNameCaptionLabel.Location = new System.Drawing.Point(0, 0);
            this.handlerNameCaptionLabel.Name = "handlerNameCaptionLabel";
            this.handlerNameCaptionLabel.Size = new System.Drawing.Size(155, 41);
            this.handlerNameCaptionLabel.TabIndex = 3;
            this.handlerNameCaptionLabel.Text = "Handler";
            this.handlerNameCaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // finishTimeCaptionPanel
            // 
            this.finishTimeCaptionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.finishTimeCaptionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(194)))));
            this.finishTimeCaptionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.finishTimeCaptionPanel.Controls.Add(this.finishTimeCaptionLabel);
            this.finishTimeCaptionPanel.Location = new System.Drawing.Point(605, 0);
            this.finishTimeCaptionPanel.Name = "finishTimeCaptionPanel";
            this.finishTimeCaptionPanel.Size = new System.Drawing.Size(198, 43);
            this.finishTimeCaptionPanel.TabIndex = 48;
            // 
            // finishTimeCaptionLabel
            // 
            this.finishTimeCaptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.finishTimeCaptionLabel.Font = new System.Drawing.Font("Arial Narrow", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.finishTimeCaptionLabel.ForeColor = System.Drawing.Color.DimGray;
            this.finishTimeCaptionLabel.Location = new System.Drawing.Point(0, 0);
            this.finishTimeCaptionLabel.Name = "finishTimeCaptionLabel";
            this.finishTimeCaptionLabel.Size = new System.Drawing.Size(196, 41);
            this.finishTimeCaptionLabel.TabIndex = 3;
            this.finishTimeCaptionLabel.Text = "Time";
            this.finishTimeCaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // faultsCaptionPanel
            // 
            this.faultsCaptionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.faultsCaptionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(194)))));
            this.faultsCaptionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.faultsCaptionPanel.Controls.Add(this.faultsCaptionLabel);
            this.faultsCaptionPanel.Location = new System.Drawing.Point(805, 0);
            this.faultsCaptionPanel.Name = "faultsCaptionPanel";
            this.faultsCaptionPanel.Size = new System.Drawing.Size(75, 43);
            this.faultsCaptionPanel.TabIndex = 49;
            // 
            // faultsCaptionLabel
            // 
            this.faultsCaptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.faultsCaptionLabel.Font = new System.Drawing.Font("Arial Narrow", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.faultsCaptionLabel.ForeColor = System.Drawing.Color.DimGray;
            this.faultsCaptionLabel.Location = new System.Drawing.Point(0, 0);
            this.faultsCaptionLabel.Name = "faultsCaptionLabel";
            this.faultsCaptionLabel.Size = new System.Drawing.Size(73, 41);
            this.faultsCaptionLabel.TabIndex = 3;
            this.faultsCaptionLabel.Text = "F";
            this.faultsCaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // refusalsCaptionPanel
            // 
            this.refusalsCaptionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.refusalsCaptionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(194)))));
            this.refusalsCaptionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.refusalsCaptionPanel.Controls.Add(this.refusalsCaptionLabel);
            this.refusalsCaptionPanel.Location = new System.Drawing.Point(882, 0);
            this.refusalsCaptionPanel.Name = "refusalsCaptionPanel";
            this.refusalsCaptionPanel.Size = new System.Drawing.Size(75, 43);
            this.refusalsCaptionPanel.TabIndex = 50;
            // 
            // refusalsCaptionLabel
            // 
            this.refusalsCaptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.refusalsCaptionLabel.Font = new System.Drawing.Font("Arial Narrow", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refusalsCaptionLabel.ForeColor = System.Drawing.Color.DimGray;
            this.refusalsCaptionLabel.Location = new System.Drawing.Point(0, 0);
            this.refusalsCaptionLabel.Name = "refusalsCaptionLabel";
            this.refusalsCaptionLabel.Size = new System.Drawing.Size(73, 41);
            this.refusalsCaptionLabel.TabIndex = 3;
            this.refusalsCaptionLabel.Text = "R";
            this.refusalsCaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // placementCaptionPanel
            // 
            this.placementCaptionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.placementCaptionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(194)))));
            this.placementCaptionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.placementCaptionPanel.Controls.Add(this.placementCaptionLabel);
            this.placementCaptionPanel.Location = new System.Drawing.Point(959, 0);
            this.placementCaptionPanel.Name = "placementCaptionPanel";
            this.placementCaptionPanel.Size = new System.Drawing.Size(87, 43);
            this.placementCaptionPanel.TabIndex = 51;
            // 
            // placementCaptionLabel
            // 
            this.placementCaptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.placementCaptionLabel.Font = new System.Drawing.Font("Arial Narrow", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.placementCaptionLabel.ForeColor = System.Drawing.Color.DimGray;
            this.placementCaptionLabel.Location = new System.Drawing.Point(0, 0);
            this.placementCaptionLabel.Name = "placementCaptionLabel";
            this.placementCaptionLabel.Size = new System.Drawing.Size(85, 41);
            this.placementCaptionLabel.TabIndex = 3;
            this.placementCaptionLabel.Text = "Place";
            this.placementCaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            // secondaryTimeHighlighter
            // 
            this.secondaryTimeHighlighter.HighlightColor = System.Drawing.Color.DeepSkyBlue;
            this.secondaryTimeHighlighter.HighlightSpeed = 500;
            this.secondaryTimeHighlighter.TargetControl = this.secondaryTimeLabel;
            this.secondaryTimeHighlighter.HighlightCycleFinished += new System.EventHandler(this.SecondaryTimeHighlighter_HighlightCycleFinished);
            // 
            // TimerDisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1047, 745);
            this.Controls.Add(this.rankingsPanel);
            this.Controls.Add(this.classTypePanel);
            this.Controls.Add(this.gradePanel);
            this.Controls.Add(this.standardCourseTimeValuePanel);
            this.Controls.Add(this.standardCourseTimeCaptionPanel);
            this.Controls.Add(this.currentRunPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TimerDisplayForm";
            this.Text = "Timer - Dog Agility Competition Controller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TimerDisplayForm_FormClosing);
            this.Load += new System.EventHandler(this.TimerDisplayForm_Load);
            this.primaryTimePanel.ResumeLayout(false);
            this.currentRunPanel.ResumeLayout(false);
            this.currentRefusalsPanel.ResumeLayout(false);
            this.currentRefusalsValuePanel.ResumeLayout(false);
            this.currentRefusalsCaptionPanel.ResumeLayout(false);
            this.prevPlacementPanel.ResumeLayout(false);
            this.prevRefusalsPanel.ResumeLayout(false);
            this.prevFaultsPanel.ResumeLayout(false);
            this.prevTimePanel.ResumeLayout(false);
            this.prevHandlerNamePanel.ResumeLayout(false);
            this.prevDogNamePanel.ResumeLayout(false);
            this.prevPanel.ResumeLayout(false);
            this.prevCompetitorNumberPanel.ResumeLayout(false);
            this.nextHandlerNamePanel.ResumeLayout(false);
            this.nextDogNamePanel.ResumeLayout(false);
            this.nextPanel.ResumeLayout(false);
            this.nextCompetitorNumberPanel.ResumeLayout(false);
            this.currentHandlerNamePanel.ResumeLayout(false);
            this.currentDogNamePanel.ResumeLayout(false);
            this.nowPanel.ResumeLayout(false);
            this.currentCompetitorNumberPanel.ResumeLayout(false);
            this.currentFaultsPanel.ResumeLayout(false);
            this.currentFaultsValuePanel.ResumeLayout(false);
            this.currentFaultsCaptionPanel.ResumeLayout(false);
            this.eliminationPanel.ResumeLayout(false);
            this.eliminationCaptionPanel.ResumeLayout(false);
            this.secondaryTimePanel.ResumeLayout(false);
            this.standardCourseTimeValuePanel.ResumeLayout(false);
            this.standardCourseTimeCaptionPanel.ResumeLayout(false);
            this.gradePanel.ResumeLayout(false);
            this.classTypePanel.ResumeLayout(false);
            this.rankingsPanel.ResumeLayout(false);
            this.countryCodeCaptionPanel.ResumeLayout(false);
            this.competitorNumberCaptionPanel.ResumeLayout(false);
            this.dogNameCaptionPanel.ResumeLayout(false);
            this.handlerameCaptionPanel.ResumeLayout(false);
            this.finishTimeCaptionPanel.ResumeLayout(false);
            this.faultsCaptionPanel.ResumeLayout(false);
            this.refusalsCaptionPanel.ResumeLayout(false);
            this.placementCaptionPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Timer displayRefreshTimer;
        private Panel primaryTimePanel;
        private Panel currentRunPanel;
        private Panel secondaryTimePanel;
        private Panel currentFaultsPanel;
        private Panel currentFaultsCaptionPanel;
        private Label currentFaultsCaptionLabel;
        private Panel eliminationPanel;
        private Panel eliminationCaptionPanel;
        private Label eliminationCaptionLabel;
        private Panel currentCompetitorNumberPanel;
        private SingleLineLabel currentCompetitorNumberLabel;
        private Panel nowPanel;
        private SingleLineLabel nowLabel;
        private Panel currentDogNamePanel;
        private SingleLineLabel currentDogNameLabel;
        private Panel currentHandlerNamePanel;
        private SingleLineLabel currentHandlerNameLabel;
        private Panel nextCompetitorNumberPanel;
        private SingleLineLabel nextCompetitorNumberLabel;
        private Panel nextPanel;
        private SingleLineLabel nextLabel;
        private Panel nextDogNamePanel;
        private SingleLineLabel nextDogNameLabel;
        private Panel nextHandlerNamePanel;
        private SingleLineLabel nextHandlerNameLabel;
        private Panel prevCompetitorNumberPanel;
        private SingleLineLabel prevCompetitorNumberLabel;
        private Panel prevPanel;
        private SingleLineLabel prevLabel;
        private Panel prevDogNamePanel;
        private SingleLineLabel prevDogNameLabel;
        private Panel prevHandlerNamePanel;
        private SingleLineLabel prevHandlerNameLabel;
        private Panel infoPanel;
        private Panel prevTimePanel;
        private SingleLineLabel prevTimeLabel;
        private Panel prevFaultsPanel;
        private SingleLineLabel prevFaultsValueLabel;
        private Panel prevRefusalsPanel;
        private SingleLineLabel prevRefusalsValueLabel;
        private Panel prevPlacementPanel;
        private SingleLineLabel prevPlacementLabel;
        private Panel standardCourseTimeValuePanel;
        private SingleLineLabel standardCourseTimeValueLabel;
        private Panel standardCourseTimeCaptionPanel;
        private SingleLineLabel standardCourseTimeCaptionLabel;
        private Panel gradePanel;
        private SingleLineLabel gradeLabel;
        private Panel classTypePanel;
        private SingleLineLabel classTypeLabel;
        private ScaleTextToFitLabel primaryTimeLabel;
        private ScaleTextToFitLabel secondaryTimeLabel;
        private Panel currentFaultsValuePanel;
        private ScaleTextToFitLabel currentFaultsValueLabel;
        private Panel currentRefusalsPanel;
        private Panel currentRefusalsValuePanel;
        private ScaleTextToFitLabel currentRefusalsValueLabel;
        private Panel currentRefusalsCaptionPanel;
        private Label currentRefusalsCaptionLabel;
        private Highlighter currentCompetitorNumberHighlighter;
        private Highlighter nextCompetitorNumberHighlighter;
        private Highlighter secondaryTimeHighlighter;
        private Panel rankingsPanel;
        private RunHistoryLine runHistoryLine1;
        private Panel countryCodeCaptionPanel;
        private Label countryCodeCaptionLabel;
        private Panel competitorNumberCaptionPanel;
        private Label competitorNumberCaptionLabel;
        private Panel dogNameCaptionPanel;
        private Label dogNameCaptionLabel;
        private Panel handlerameCaptionPanel;
        private Label handlerNameCaptionLabel;
        private Panel finishTimeCaptionPanel;
        private Label finishTimeCaptionLabel;
        private Panel faultsCaptionPanel;
        private Label faultsCaptionLabel;
        private Panel refusalsCaptionPanel;
        private Label refusalsCaptionLabel;
        private Panel placementCaptionPanel;
        private Label placementCaptionLabel;
        private RunHistoryLine runHistoryLine3;
        private RunHistoryLine runHistoryLine2;
        private RunHistoryLine runHistoryLine5;
        private RunHistoryLine runHistoryLine4;
    }
}