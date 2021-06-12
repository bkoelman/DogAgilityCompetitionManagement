using System.ComponentModel;
using System.Windows.Forms;

namespace DogAgilityCompetition.MediatorEmulator.UI.Controls
{
    partial class DisplayStatusControl
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
            this.displayGroupBox = new System.Windows.Forms.GroupBox();
            this.primaryTimeSecondsLabel = new System.Windows.Forms.Label();
            this.currentFaultsValueLabel = new System.Windows.Forms.Label();
            this.previousCompetitorPlacementLabel = new System.Windows.Forms.Label();
            this.currentRefusalsValueLabel = new System.Windows.Forms.Label();
            this.primaryTimeMillisecondsLabel = new System.Windows.Forms.Label();
            this.currentCompetitorNumberLabel = new System.Windows.Forms.Label();
            this.nextCompetitorNumberLabel = new System.Windows.Forms.Label();
            this.displayRefreshTimer = new System.Windows.Forms.Timer(this.components);
            this.displayGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // displayGroupBox
            // 
            this.displayGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.displayGroupBox.Controls.Add(this.primaryTimeSecondsLabel);
            this.displayGroupBox.Controls.Add(this.currentFaultsValueLabel);
            this.displayGroupBox.Controls.Add(this.previousCompetitorPlacementLabel);
            this.displayGroupBox.Controls.Add(this.currentRefusalsValueLabel);
            this.displayGroupBox.Controls.Add(this.primaryTimeMillisecondsLabel);
            this.displayGroupBox.Controls.Add(this.currentCompetitorNumberLabel);
            this.displayGroupBox.Controls.Add(this.nextCompetitorNumberLabel);
            this.displayGroupBox.Location = new System.Drawing.Point(3, 3);
            this.displayGroupBox.Name = "displayGroupBox";
            this.displayGroupBox.Size = new System.Drawing.Size(258, 139);
            this.displayGroupBox.TabIndex = 0;
            this.displayGroupBox.TabStop = false;
            this.displayGroupBox.Text = "Display";
            // 
            // primaryTimeSecondsLabel
            // 
            this.primaryTimeSecondsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.primaryTimeSecondsLabel.Location = new System.Drawing.Point(6, 16);
            this.primaryTimeSecondsLabel.Name = "primaryTimeSecondsLabel";
            this.primaryTimeSecondsLabel.Size = new System.Drawing.Size(110, 55);
            this.primaryTimeSecondsLabel.TabIndex = 0;
            this.primaryTimeSecondsLabel.Text = "000";
            this.primaryTimeSecondsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // currentFaultsValueLabel
            // 
            this.currentFaultsValueLabel.AutoSize = true;
            this.currentFaultsValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentFaultsValueLabel.ForeColor = System.Drawing.Color.LimeGreen;
            this.currentFaultsValueLabel.Location = new System.Drawing.Point(21, 71);
            this.currentFaultsValueLabel.Name = "currentFaultsValueLabel";
            this.currentFaultsValueLabel.Size = new System.Drawing.Size(44, 31);
            this.currentFaultsValueLabel.TabIndex = 2;
            this.currentFaultsValueLabel.Text = "00";
            // 
            // previousCompetitorPlacementLabel
            // 
            this.previousCompetitorPlacementLabel.AutoSize = true;
            this.previousCompetitorPlacementLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.previousCompetitorPlacementLabel.ForeColor = System.Drawing.Color.Red;
            this.previousCompetitorPlacementLabel.Location = new System.Drawing.Point(97, 102);
            this.previousCompetitorPlacementLabel.Name = "previousCompetitorPlacementLabel";
            this.previousCompetitorPlacementLabel.Size = new System.Drawing.Size(59, 31);
            this.previousCompetitorPlacementLabel.TabIndex = 5;
            this.previousCompetitorPlacementLabel.Text = "000";
            // 
            // currentRefusalsValueLabel
            // 
            this.currentRefusalsValueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.currentRefusalsValueLabel.AutoSize = true;
            this.currentRefusalsValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentRefusalsValueLabel.ForeColor = System.Drawing.Color.LimeGreen;
            this.currentRefusalsValueLabel.Location = new System.Drawing.Point(193, 71);
            this.currentRefusalsValueLabel.Name = "currentRefusalsValueLabel";
            this.currentRefusalsValueLabel.Size = new System.Drawing.Size(44, 31);
            this.currentRefusalsValueLabel.TabIndex = 3;
            this.currentRefusalsValueLabel.Text = "00";
            // 
            // primaryTimeMillisecondsLabel
            // 
            this.primaryTimeMillisecondsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.primaryTimeMillisecondsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.primaryTimeMillisecondsLabel.Location = new System.Drawing.Point(142, 16);
            this.primaryTimeMillisecondsLabel.Name = "primaryTimeMillisecondsLabel";
            this.primaryTimeMillisecondsLabel.Size = new System.Drawing.Size(110, 55);
            this.primaryTimeMillisecondsLabel.TabIndex = 1;
            this.primaryTimeMillisecondsLabel.Text = "---";
            this.primaryTimeMillisecondsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // currentCompetitorNumberLabel
            // 
            this.currentCompetitorNumberLabel.AutoSize = true;
            this.currentCompetitorNumberLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentCompetitorNumberLabel.ForeColor = System.Drawing.Color.Red;
            this.currentCompetitorNumberLabel.Location = new System.Drawing.Point(6, 102);
            this.currentCompetitorNumberLabel.Name = "currentCompetitorNumberLabel";
            this.currentCompetitorNumberLabel.Size = new System.Drawing.Size(59, 31);
            this.currentCompetitorNumberLabel.TabIndex = 4;
            this.currentCompetitorNumberLabel.Text = "000";
            // 
            // nextCompetitorNumberLabel
            // 
            this.nextCompetitorNumberLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nextCompetitorNumberLabel.AutoSize = true;
            this.nextCompetitorNumberLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextCompetitorNumberLabel.ForeColor = System.Drawing.Color.Red;
            this.nextCompetitorNumberLabel.Location = new System.Drawing.Point(193, 102);
            this.nextCompetitorNumberLabel.Name = "nextCompetitorNumberLabel";
            this.nextCompetitorNumberLabel.Size = new System.Drawing.Size(59, 31);
            this.nextCompetitorNumberLabel.TabIndex = 6;
            this.nextCompetitorNumberLabel.Text = "000";
            // 
            // displayRefreshTimer
            // 
            this.displayRefreshTimer.Interval = 250;
            this.displayRefreshTimer.Tick += new System.EventHandler(this.DisplayRefreshTimer_Tick);
            // 
            // DisplayStatusControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.displayGroupBox);
            this.Name = "DisplayStatusControl";
            this.Size = new System.Drawing.Size(264, 145);
            this.Resize += new System.EventHandler(this.DisplayStatusControl_Resize);
            this.displayGroupBox.ResumeLayout(false);
            this.displayGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox displayGroupBox;
        private Label primaryTimeSecondsLabel;
        private Label currentFaultsValueLabel;
        private Label previousCompetitorPlacementLabel;
        private Label currentRefusalsValueLabel;
        private Label primaryTimeMillisecondsLabel;
        private Label currentCompetitorNumberLabel;
        private Label nextCompetitorNumberLabel;
        private Timer displayRefreshTimer;
    }
}
