﻿using System.ComponentModel;
using System.Windows.Forms;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    partial class CompetitionPictureAlert
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompetitionPictureAlert));
            this.enabledCheckBox = new System.Windows.Forms.CheckBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.picturePreviewButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // enabledCheckBox
            // 
            this.enabledCheckBox.AutoSize = true;
            this.enabledCheckBox.Location = new System.Drawing.Point(4, 5);
            this.enabledCheckBox.Name = "enabledCheckBox";
            this.enabledCheckBox.Size = new System.Drawing.Size(15, 14);
            this.enabledCheckBox.TabIndex = 0;
            this.enabledCheckBox.UseVisualStyleBackColor = true;
            this.enabledCheckBox.CheckedChanged += new System.EventHandler(this.EnabledCheckBox_CheckedChanged);
            // 
            // browseButton
            // 
            this.browseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseButton.Enabled = false;
            this.browseButton.Location = new System.Drawing.Point(197, 1);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(42, 20);
            this.browseButton.TabIndex = 3;
            this.browseButton.Text = "...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // pathTextBox
            // 
            this.pathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathTextBox.Enabled = false;
            this.pathTextBox.Location = new System.Drawing.Point(51, 1);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new System.Drawing.Size(125, 20);
            this.pathTextBox.TabIndex = 2;
            this.pathTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.PathTextBox_Validating);
            // 
            // picturePreviewButton
            // 
            this.picturePreviewButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picturePreviewButton.BackgroundImage")));
            this.picturePreviewButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picturePreviewButton.Enabled = false;
            this.picturePreviewButton.Location = new System.Drawing.Point(21, -1);
            this.picturePreviewButton.Name = "picturePreviewButton";
            this.picturePreviewButton.Size = new System.Drawing.Size(24, 24);
            this.picturePreviewButton.TabIndex = 1;
            this.picturePreviewButton.UseVisualStyleBackColor = true;
            this.picturePreviewButton.Click += new System.EventHandler(this.PicturePreviewButton_Click);
            // 
            // CompetitionPictureAlert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.enabledCheckBox);
            this.Controls.Add(this.picturePreviewButton);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.pathTextBox);
            this.Name = "CompetitionPictureAlert";
            this.Size = new System.Drawing.Size(239, 22);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CheckBox enabledCheckBox;
        private Button picturePreviewButton;
        private Button browseButton;
        private TextBox pathTextBox;

    }
}
