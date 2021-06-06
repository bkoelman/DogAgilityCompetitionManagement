using System.ComponentModel;
using System.Windows.Forms;

namespace DogAgilityCompetition.Controller.UI.Forms
{
    partial class CustomDisplaySetupForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomDisplaySetupForm));
            this.pictureRadioButton = new System.Windows.Forms.RadioButton();
            this.textRadioButton = new System.Windows.Forms.RadioButton();
            this.picturePathTextBox = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.textGroupBox = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.thirdLineTextBox = new System.Windows.Forms.TextBox();
            this.secondLineTextBox = new System.Windows.Forms.TextBox();
            this.firstLineTextBox = new System.Windows.Forms.TextBox();
            this.firstLineRadioButton = new System.Windows.Forms.RadioButton();
            this.systemTimeRadioButton = new System.Windows.Forms.RadioButton();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.textGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureRadioButton
            // 
            this.pictureRadioButton.AutoSize = true;
            this.pictureRadioButton.Location = new System.Drawing.Point(12, 12);
            this.pictureRadioButton.Name = "pictureRadioButton";
            this.pictureRadioButton.Size = new System.Drawing.Size(58, 17);
            this.pictureRadioButton.TabIndex = 0;
            this.pictureRadioButton.Text = "&Picture";
            this.pictureRadioButton.UseVisualStyleBackColor = true;
            this.pictureRadioButton.CheckedChanged += new System.EventHandler(this.PictureRadioButton_CheckedChanged);
            // 
            // textRadioButton
            // 
            this.textRadioButton.AutoSize = true;
            this.textRadioButton.Checked = true;
            this.textRadioButton.Location = new System.Drawing.Point(12, 71);
            this.textRadioButton.Name = "textRadioButton";
            this.textRadioButton.Size = new System.Drawing.Size(46, 17);
            this.textRadioButton.TabIndex = 3;
            this.textRadioButton.TabStop = true;
            this.textRadioButton.Text = "&Text";
            this.textRadioButton.UseVisualStyleBackColor = true;
            // 
            // picturePathTextBox
            // 
            this.picturePathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picturePathTextBox.Enabled = false;
            this.picturePathTextBox.Location = new System.Drawing.Point(31, 35);
            this.picturePathTextBox.Name = "picturePathTextBox";
            this.picturePathTextBox.Size = new System.Drawing.Size(389, 20);
            this.picturePathTextBox.TabIndex = 1;
            // 
            // browseButton
            // 
            this.browseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseButton.Enabled = false;
            this.browseButton.Location = new System.Drawing.Point(426, 35);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(34, 20);
            this.browseButton.TabIndex = 2;
            this.browseButton.Text = "...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // textGroupBox
            // 
            this.textGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textGroupBox.Controls.Add(this.label2);
            this.textGroupBox.Controls.Add(this.label1);
            this.textGroupBox.Controls.Add(this.thirdLineTextBox);
            this.textGroupBox.Controls.Add(this.secondLineTextBox);
            this.textGroupBox.Controls.Add(this.firstLineTextBox);
            this.textGroupBox.Controls.Add(this.firstLineRadioButton);
            this.textGroupBox.Controls.Add(this.systemTimeRadioButton);
            this.textGroupBox.Location = new System.Drawing.Point(31, 94);
            this.textGroupBox.Name = "textGroupBox";
            this.textGroupBox.Size = new System.Drawing.Size(429, 129);
            this.textGroupBox.TabIndex = 4;
            this.textGroupBox.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Line &3";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Line &2";
            // 
            // thirdLineTextBox
            // 
            this.thirdLineTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.thirdLineTextBox.Location = new System.Drawing.Point(66, 94);
            this.thirdLineTextBox.Name = "thirdLineTextBox";
            this.thirdLineTextBox.Size = new System.Drawing.Size(357, 20);
            this.thirdLineTextBox.TabIndex = 6;
            // 
            // secondLineTextBox
            // 
            this.secondLineTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.secondLineTextBox.Location = new System.Drawing.Point(66, 68);
            this.secondLineTextBox.Name = "secondLineTextBox";
            this.secondLineTextBox.Size = new System.Drawing.Size(357, 20);
            this.secondLineTextBox.TabIndex = 4;
            // 
            // firstLineTextBox
            // 
            this.firstLineTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.firstLineTextBox.Enabled = false;
            this.firstLineTextBox.Location = new System.Drawing.Point(66, 41);
            this.firstLineTextBox.Name = "firstLineTextBox";
            this.firstLineTextBox.Size = new System.Drawing.Size(357, 20);
            this.firstLineTextBox.TabIndex = 2;
            // 
            // firstLineRadioButton
            // 
            this.firstLineRadioButton.AutoSize = true;
            this.firstLineRadioButton.Location = new System.Drawing.Point(6, 42);
            this.firstLineRadioButton.Name = "firstLineRadioButton";
            this.firstLineRadioButton.Size = new System.Drawing.Size(54, 17);
            this.firstLineRadioButton.TabIndex = 1;
            this.firstLineRadioButton.Text = "Line &1";
            this.firstLineRadioButton.UseVisualStyleBackColor = true;
            this.firstLineRadioButton.CheckedChanged += new System.EventHandler(this.TopLineRadioButton_CheckedChanged);
            // 
            // systemTimeRadioButton
            // 
            this.systemTimeRadioButton.AutoSize = true;
            this.systemTimeRadioButton.Checked = true;
            this.systemTimeRadioButton.Location = new System.Drawing.Point(6, 19);
            this.systemTimeRadioButton.Name = "systemTimeRadioButton";
            this.systemTimeRadioButton.Size = new System.Drawing.Size(81, 17);
            this.systemTimeRadioButton.TabIndex = 0;
            this.systemTimeRadioButton.TabStop = true;
            this.systemTimeRadioButton.Text = "&System time";
            this.systemTimeRadioButton.UseVisualStyleBackColor = true;
            this.systemTimeRadioButton.CheckedChanged += new System.EventHandler(this.TopLineRadioButton_CheckedChanged);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(304, 239);
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
            this.cancelButton.Location = new System.Drawing.Point(385, 239);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // CustomDisplaySetupForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(472, 274);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.textGroupBox);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.picturePathTextBox);
            this.Controls.Add(this.textRadioButton);
            this.Controls.Add(this.pictureRadioButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(488, 313);
            this.Name = "CustomDisplaySetupForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Custom Display Setup - Dog Agility Competition Controller";
            this.Load += new System.EventHandler(this.CustomDisplaySetupForm_Load);
            this.textGroupBox.ResumeLayout(false);
            this.textGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RadioButton pictureRadioButton;
        private RadioButton textRadioButton;
        private TextBox picturePathTextBox;
        private Button browseButton;
        private GroupBox textGroupBox;
        private Label label2;
        private Label label1;
        private TextBox thirdLineTextBox;
        private TextBox secondLineTextBox;
        private TextBox firstLineTextBox;
        private RadioButton firstLineRadioButton;
        private RadioButton systemTimeRadioButton;
        private Button okButton;
        private Button cancelButton;
    }
}