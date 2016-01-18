using System.ComponentModel;
using System.Windows.Forms;
using DogAgilityCompetition.Controller.UI.Controls;

namespace DogAgilityCompetition.Controller.UI.Forms
{
    partial class ClassResultsDisplayForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClassResultsDisplayForm));
            this.competitorNumberCaptionPanel = new System.Windows.Forms.Panel();
            this.competitorNumberCaptionLabel = new System.Windows.Forms.Label();
            this.countryCodeCaptionPanel = new System.Windows.Forms.Panel();
            this.countryCodeCaptionLabel = new System.Windows.Forms.Label();
            this.dogNameCaptionPanel = new System.Windows.Forms.Panel();
            this.dogNameCaptionLabel = new System.Windows.Forms.Label();
            this.handlerNameCaptionPanel = new System.Windows.Forms.Panel();
            this.handlerNameCaptionLabel = new System.Windows.Forms.Label();
            this.finishTimeCaptionPanel = new System.Windows.Forms.Panel();
            this.finishTimeCaptionLabel = new System.Windows.Forms.Label();
            this.faultsCaptionPanel = new System.Windows.Forms.Panel();
            this.faultsCaptionLabel = new System.Windows.Forms.Label();
            this.refusalsCaptionPanel = new System.Windows.Forms.Panel();
            this.refusalsCaptionLabel = new System.Windows.Forms.Label();
            this.placementCaptionPanel = new System.Windows.Forms.Panel();
            this.placementCaptionLabel = new System.Windows.Forms.Label();
            this.gradePanel = new System.Windows.Forms.Panel();
            this.gradeLabel = new DogAgilityCompetition.Controller.UI.Controls.SingleLineLabel();
            this.classTypePanel = new System.Windows.Forms.Panel();
            this.classTypeLabel = new DogAgilityCompetition.Controller.UI.Controls.SingleLineLabel();
            this.standardCourseTimeCaptionPanel = new System.Windows.Forms.Panel();
            this.standardCourseTimeCaptionLabel = new DogAgilityCompetition.Controller.UI.Controls.SingleLineLabel();
            this.standardCourseTimeValuePanel = new System.Windows.Forms.Panel();
            this.standardCourseTimeValueLabel = new DogAgilityCompetition.Controller.UI.Controls.SingleLineLabel();
            this.scrollingPanel = new DogAgilityCompetition.Controller.UI.Controls.AutoScrollingPanel();
            this.runHistoryLine0003 = new DogAgilityCompetition.Controller.UI.Controls.RunHistoryLine();
            this.runHistoryLine0002 = new DogAgilityCompetition.Controller.UI.Controls.RunHistoryLine();
            this.runHistoryLine0001 = new DogAgilityCompetition.Controller.UI.Controls.RunHistoryLine();
            this.competitorNumberCaptionPanel.SuspendLayout();
            this.countryCodeCaptionPanel.SuspendLayout();
            this.dogNameCaptionPanel.SuspendLayout();
            this.handlerNameCaptionPanel.SuspendLayout();
            this.finishTimeCaptionPanel.SuspendLayout();
            this.faultsCaptionPanel.SuspendLayout();
            this.refusalsCaptionPanel.SuspendLayout();
            this.placementCaptionPanel.SuspendLayout();
            this.gradePanel.SuspendLayout();
            this.classTypePanel.SuspendLayout();
            this.standardCourseTimeCaptionPanel.SuspendLayout();
            this.standardCourseTimeValuePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // competitorNumberCaptionPanel
            // 
            this.competitorNumberCaptionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.competitorNumberCaptionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.competitorNumberCaptionPanel.Controls.Add(this.competitorNumberCaptionLabel);
            this.competitorNumberCaptionPanel.Location = new System.Drawing.Point(2, 2);
            this.competitorNumberCaptionPanel.Name = "competitorNumberCaptionPanel";
            this.competitorNumberCaptionPanel.Size = new System.Drawing.Size(131, 43);
            this.competitorNumberCaptionPanel.TabIndex = 37;
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
            // countryCodeCaptionPanel
            // 
            this.countryCodeCaptionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.countryCodeCaptionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.countryCodeCaptionPanel.Controls.Add(this.countryCodeCaptionLabel);
            this.countryCodeCaptionPanel.Location = new System.Drawing.Point(135, 2);
            this.countryCodeCaptionPanel.Name = "countryCodeCaptionPanel";
            this.countryCodeCaptionPanel.Size = new System.Drawing.Size(124, 43);
            this.countryCodeCaptionPanel.TabIndex = 38;
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
            // dogNameCaptionPanel
            // 
            this.dogNameCaptionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dogNameCaptionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dogNameCaptionPanel.Controls.Add(this.dogNameCaptionLabel);
            this.dogNameCaptionPanel.Location = new System.Drawing.Point(261, 2);
            this.dogNameCaptionPanel.Name = "dogNameCaptionPanel";
            this.dogNameCaptionPanel.Size = new System.Drawing.Size(182, 43);
            this.dogNameCaptionPanel.TabIndex = 39;
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
            // handlerNameCaptionPanel
            // 
            this.handlerNameCaptionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.handlerNameCaptionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.handlerNameCaptionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.handlerNameCaptionPanel.Controls.Add(this.handlerNameCaptionLabel);
            this.handlerNameCaptionPanel.Location = new System.Drawing.Point(445, 2);
            this.handlerNameCaptionPanel.Name = "handlerNameCaptionPanel";
            this.handlerNameCaptionPanel.Size = new System.Drawing.Size(156, 43);
            this.handlerNameCaptionPanel.TabIndex = 40;
            // 
            // handlerNameCaptionLabel
            // 
            this.handlerNameCaptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.handlerNameCaptionLabel.Font = new System.Drawing.Font("Arial Narrow", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.handlerNameCaptionLabel.ForeColor = System.Drawing.Color.DimGray;
            this.handlerNameCaptionLabel.Location = new System.Drawing.Point(0, 0);
            this.handlerNameCaptionLabel.Name = "handlerNameCaptionLabel";
            this.handlerNameCaptionLabel.Size = new System.Drawing.Size(154, 41);
            this.handlerNameCaptionLabel.TabIndex = 3;
            this.handlerNameCaptionLabel.Text = "Handler";
            this.handlerNameCaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // finishTimeCaptionPanel
            // 
            this.finishTimeCaptionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.finishTimeCaptionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.finishTimeCaptionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.finishTimeCaptionPanel.Controls.Add(this.finishTimeCaptionLabel);
            this.finishTimeCaptionPanel.Location = new System.Drawing.Point(603, 2);
            this.finishTimeCaptionPanel.Name = "finishTimeCaptionPanel";
            this.finishTimeCaptionPanel.Size = new System.Drawing.Size(198, 43);
            this.finishTimeCaptionPanel.TabIndex = 41;
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
            this.faultsCaptionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.faultsCaptionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.faultsCaptionPanel.Controls.Add(this.faultsCaptionLabel);
            this.faultsCaptionPanel.Location = new System.Drawing.Point(803, 2);
            this.faultsCaptionPanel.Name = "faultsCaptionPanel";
            this.faultsCaptionPanel.Size = new System.Drawing.Size(75, 43);
            this.faultsCaptionPanel.TabIndex = 42;
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
            this.refusalsCaptionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.refusalsCaptionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.refusalsCaptionPanel.Controls.Add(this.refusalsCaptionLabel);
            this.refusalsCaptionPanel.Location = new System.Drawing.Point(880, 2);
            this.refusalsCaptionPanel.Name = "refusalsCaptionPanel";
            this.refusalsCaptionPanel.Size = new System.Drawing.Size(75, 43);
            this.refusalsCaptionPanel.TabIndex = 43;
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
            this.placementCaptionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.placementCaptionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.placementCaptionPanel.Controls.Add(this.placementCaptionLabel);
            this.placementCaptionPanel.Location = new System.Drawing.Point(957, 2);
            this.placementCaptionPanel.Name = "placementCaptionPanel";
            this.placementCaptionPanel.Size = new System.Drawing.Size(87, 43);
            this.placementCaptionPanel.TabIndex = 44;
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
            // gradePanel
            // 
            this.gradePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gradePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.gradePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gradePanel.Controls.Add(this.gradeLabel);
            this.gradePanel.Location = new System.Drawing.Point(2, 295);
            this.gradePanel.Name = "gradePanel";
            this.gradePanel.Size = new System.Drawing.Size(380, 112);
            this.gradePanel.TabIndex = 60;
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
            this.classTypePanel.Location = new System.Drawing.Point(384, 295);
            this.classTypePanel.Name = "classTypePanel";
            this.classTypePanel.Size = new System.Drawing.Size(336, 112);
            this.classTypePanel.TabIndex = 61;
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
            // standardCourseTimeCaptionPanel
            // 
            this.standardCourseTimeCaptionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.standardCourseTimeCaptionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.standardCourseTimeCaptionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.standardCourseTimeCaptionPanel.Controls.Add(this.standardCourseTimeCaptionLabel);
            this.standardCourseTimeCaptionPanel.Location = new System.Drawing.Point(722, 295);
            this.standardCourseTimeCaptionPanel.Name = "standardCourseTimeCaptionPanel";
            this.standardCourseTimeCaptionPanel.Size = new System.Drawing.Size(168, 112);
            this.standardCourseTimeCaptionPanel.TabIndex = 62;
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
            // standardCourseTimeValuePanel
            // 
            this.standardCourseTimeValuePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.standardCourseTimeValuePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.standardCourseTimeValuePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.standardCourseTimeValuePanel.Controls.Add(this.standardCourseTimeValueLabel);
            this.standardCourseTimeValuePanel.Location = new System.Drawing.Point(892, 295);
            this.standardCourseTimeValuePanel.Name = "standardCourseTimeValuePanel";
            this.standardCourseTimeValuePanel.Size = new System.Drawing.Size(152, 112);
            this.standardCourseTimeValuePanel.TabIndex = 63;
            // 
            // standardCourseTimeValueLabel
            // 
            this.standardCourseTimeValueLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.standardCourseTimeValueLabel.Font = new System.Drawing.Font("Arial Narrow", 65.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.standardCourseTimeValueLabel.Location = new System.Drawing.Point(0, 0);
            this.standardCourseTimeValueLabel.Name = "standardCourseTimeValueLabel";
            this.standardCourseTimeValueLabel.Size = new System.Drawing.Size(150, 110);
            this.standardCourseTimeValueLabel.TabIndex = 3;
            this.standardCourseTimeValueLabel.Text = "45";
            this.standardCourseTimeValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // scrollingPanel
            // 
            this.scrollingPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scrollingPanel.Location = new System.Drawing.Point(0, 203);
            this.scrollingPanel.Name = "scrollingPanel";
            this.scrollingPanel.Size = new System.Drawing.Size(1046, 90);
            this.scrollingPanel.TabIndex = 64;
            // 
            // runHistoryLine0003
            // 
            this.runHistoryLine0003.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.runHistoryLine0003.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(71)))));
            this.runHistoryLine0003.Location = new System.Drawing.Point(2, 151);
            this.runHistoryLine0003.Name = "runHistoryLine0003";
            this.runHistoryLine0003.Size = new System.Drawing.Size(1042, 50);
            this.runHistoryLine0003.TabIndex = 46;
            // 
            // runHistoryLine0002
            // 
            this.runHistoryLine0002.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.runHistoryLine0002.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(71)))));
            this.runHistoryLine0002.Location = new System.Drawing.Point(2, 99);
            this.runHistoryLine0002.Name = "runHistoryLine0002";
            this.runHistoryLine0002.Size = new System.Drawing.Size(1042, 50);
            this.runHistoryLine0002.TabIndex = 45;
            // 
            // runHistoryLine0001
            // 
            this.runHistoryLine0001.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.runHistoryLine0001.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(71)))));
            this.runHistoryLine0001.Location = new System.Drawing.Point(2, 47);
            this.runHistoryLine0001.Name = "runHistoryLine0001";
            this.runHistoryLine0001.Size = new System.Drawing.Size(1042, 50);
            this.runHistoryLine0001.TabIndex = 2;
            // 
            // ClassResultsDisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1046, 409);
            this.Controls.Add(this.scrollingPanel);
            this.Controls.Add(this.standardCourseTimeValuePanel);
            this.Controls.Add(this.standardCourseTimeCaptionPanel);
            this.Controls.Add(this.classTypePanel);
            this.Controls.Add(this.gradePanel);
            this.Controls.Add(this.runHistoryLine0003);
            this.Controls.Add(this.runHistoryLine0002);
            this.Controls.Add(this.placementCaptionPanel);
            this.Controls.Add(this.refusalsCaptionPanel);
            this.Controls.Add(this.faultsCaptionPanel);
            this.Controls.Add(this.finishTimeCaptionPanel);
            this.Controls.Add(this.handlerNameCaptionPanel);
            this.Controls.Add(this.dogNameCaptionPanel);
            this.Controls.Add(this.countryCodeCaptionPanel);
            this.Controls.Add(this.competitorNumberCaptionPanel);
            this.Controls.Add(this.runHistoryLine0001);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ClassResultsDisplayForm";
            this.Text = "Class Results Display Form - Dog Agility Competition Controller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClassResultsDisplayForm_FormClosing);
            this.competitorNumberCaptionPanel.ResumeLayout(false);
            this.countryCodeCaptionPanel.ResumeLayout(false);
            this.dogNameCaptionPanel.ResumeLayout(false);
            this.handlerNameCaptionPanel.ResumeLayout(false);
            this.finishTimeCaptionPanel.ResumeLayout(false);
            this.faultsCaptionPanel.ResumeLayout(false);
            this.refusalsCaptionPanel.ResumeLayout(false);
            this.placementCaptionPanel.ResumeLayout(false);
            this.gradePanel.ResumeLayout(false);
            this.classTypePanel.ResumeLayout(false);
            this.standardCourseTimeCaptionPanel.ResumeLayout(false);
            this.standardCourseTimeValuePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel competitorNumberCaptionPanel;
        private Label competitorNumberCaptionLabel;
        private Panel countryCodeCaptionPanel;
        private Label countryCodeCaptionLabel;
        private Panel dogNameCaptionPanel;
        private Label dogNameCaptionLabel;
        private Panel handlerNameCaptionPanel;
        private Label handlerNameCaptionLabel;
        private Panel finishTimeCaptionPanel;
        private Label finishTimeCaptionLabel;
        private Panel faultsCaptionPanel;
        private Label faultsCaptionLabel;
        private Panel refusalsCaptionPanel;
        private Label refusalsCaptionLabel;
        private Panel placementCaptionPanel;
        private Label placementCaptionLabel;
        private RunHistoryLine runHistoryLine0001;
        private RunHistoryLine runHistoryLine0002;
        private RunHistoryLine runHistoryLine0003;
        private Panel gradePanel;
        private SingleLineLabel gradeLabel;
        private Panel classTypePanel;
        private SingleLineLabel classTypeLabel;
        private Panel standardCourseTimeCaptionPanel;
        private SingleLineLabel standardCourseTimeCaptionLabel;
        private Panel standardCourseTimeValuePanel;
        private SingleLineLabel standardCourseTimeValueLabel;
        private AutoScrollingPanel scrollingPanel;
    }
}