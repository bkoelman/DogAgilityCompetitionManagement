using System.ComponentModel;
using System.Windows.Forms;

namespace DogAgilityCompetition.MediatorEmulator.UI.Controls
{
    partial class KeypadControl
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
            this.playSoundAButton = new System.Windows.Forms.Button();
            this.passIntermediateButton = new System.Windows.Forms.Button();
            this.toggleEliminationButton = new System.Windows.Forms.Button();
            this.fourButton = new System.Windows.Forms.Button();
            this.decreaseRefusalsButton = new System.Windows.Forms.Button();
            this.increaseRefusalsButton = new System.Windows.Forms.Button();
            this.sevenButton = new System.Windows.Forms.Button();
            this.decreaseFaultsButton = new System.Windows.Forms.Button();
            this.increaseFaultsButton = new System.Windows.Forms.Button();
            this.muteSoundButton = new System.Windows.Forms.Button();
            this.enterNextCompetitorCheckBox = new System.Windows.Forms.CheckBox();
            this.timeCheckBox = new System.Windows.Forms.CheckBox();
            this.enterCurrentCompetitorCheckBox = new System.Windows.Forms.CheckBox();
            this.passStartButton = new System.Windows.Forms.Button();
            this.passFinishButton = new System.Windows.Forms.Button();
            this.readyButton = new System.Windows.Forms.Button();
            this.resetRunButton = new System.Windows.Forms.Button();
            this.numericCheckBox = new System.Windows.Forms.CheckBox();
            this.coreCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // playSoundAButton
            // 
            this.playSoundAButton.Location = new System.Drawing.Point(3, 3);
            this.playSoundAButton.Name = "playSoundAButton";
            this.playSoundAButton.Size = new System.Drawing.Size(75, 75);
            this.playSoundAButton.TabIndex = 0;
            this.playSoundAButton.Text = "1";
            this.playSoundAButton.UseVisualStyleBackColor = true;
            this.playSoundAButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseDown);
            this.playSoundAButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseUp);
            // 
            // passIntermediateButton
            // 
            this.passIntermediateButton.Location = new System.Drawing.Point(84, 3);
            this.passIntermediateButton.Name = "passIntermediateButton";
            this.passIntermediateButton.Size = new System.Drawing.Size(75, 75);
            this.passIntermediateButton.TabIndex = 1;
            this.passIntermediateButton.Text = "2=PassIntermediate";
            this.passIntermediateButton.UseVisualStyleBackColor = true;
            this.passIntermediateButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseDown);
            this.passIntermediateButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseUp);
            // 
            // toggleEliminationButton
            // 
            this.toggleEliminationButton.Location = new System.Drawing.Point(165, 3);
            this.toggleEliminationButton.Name = "toggleEliminationButton";
            this.toggleEliminationButton.Size = new System.Drawing.Size(75, 75);
            this.toggleEliminationButton.TabIndex = 2;
            this.toggleEliminationButton.Text = "3=ToggleElimination";
            this.toggleEliminationButton.UseVisualStyleBackColor = true;
            this.toggleEliminationButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseDown);
            this.toggleEliminationButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseUp);
            // 
            // fourButton
            // 
            this.fourButton.Location = new System.Drawing.Point(3, 84);
            this.fourButton.Name = "fourButton";
            this.fourButton.Size = new System.Drawing.Size(75, 75);
            this.fourButton.TabIndex = 3;
            this.fourButton.Text = "4";
            this.fourButton.UseVisualStyleBackColor = true;
            this.fourButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseDown);
            this.fourButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseUp);
            // 
            // decreaseRefusalsButton
            // 
            this.decreaseRefusalsButton.Location = new System.Drawing.Point(84, 84);
            this.decreaseRefusalsButton.Name = "decreaseRefusalsButton";
            this.decreaseRefusalsButton.Size = new System.Drawing.Size(75, 75);
            this.decreaseRefusalsButton.TabIndex = 4;
            this.decreaseRefusalsButton.Text = "5=Refusals-";
            this.decreaseRefusalsButton.UseVisualStyleBackColor = true;
            this.decreaseRefusalsButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseDown);
            this.decreaseRefusalsButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseUp);
            // 
            // increaseRefusalsButton
            // 
            this.increaseRefusalsButton.Location = new System.Drawing.Point(165, 84);
            this.increaseRefusalsButton.Name = "increaseRefusalsButton";
            this.increaseRefusalsButton.Size = new System.Drawing.Size(75, 75);
            this.increaseRefusalsButton.TabIndex = 5;
            this.increaseRefusalsButton.Text = "6=Refusals+";
            this.increaseRefusalsButton.UseVisualStyleBackColor = true;
            this.increaseRefusalsButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseDown);
            this.increaseRefusalsButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseUp);
            // 
            // sevenButton
            // 
            this.sevenButton.Location = new System.Drawing.Point(3, 165);
            this.sevenButton.Name = "sevenButton";
            this.sevenButton.Size = new System.Drawing.Size(75, 75);
            this.sevenButton.TabIndex = 6;
            this.sevenButton.Text = "7";
            this.sevenButton.UseVisualStyleBackColor = true;
            this.sevenButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseDown);
            this.sevenButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseUp);
            // 
            // decreaseFaultsButton
            // 
            this.decreaseFaultsButton.Location = new System.Drawing.Point(84, 165);
            this.decreaseFaultsButton.Name = "decreaseFaultsButton";
            this.decreaseFaultsButton.Size = new System.Drawing.Size(75, 75);
            this.decreaseFaultsButton.TabIndex = 7;
            this.decreaseFaultsButton.Text = "8=Faults-";
            this.decreaseFaultsButton.UseVisualStyleBackColor = true;
            this.decreaseFaultsButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseDown);
            this.decreaseFaultsButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseUp);
            // 
            // increaseFaultsButton
            // 
            this.increaseFaultsButton.Location = new System.Drawing.Point(165, 165);
            this.increaseFaultsButton.Name = "increaseFaultsButton";
            this.increaseFaultsButton.Size = new System.Drawing.Size(75, 75);
            this.increaseFaultsButton.TabIndex = 8;
            this.increaseFaultsButton.Text = "9=Faults +";
            this.increaseFaultsButton.UseVisualStyleBackColor = true;
            this.increaseFaultsButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseDown);
            this.increaseFaultsButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseUp);
            // 
            // muteSoundButton
            // 
            this.muteSoundButton.Location = new System.Drawing.Point(165, 246);
            this.muteSoundButton.Name = "muteSoundButton";
            this.muteSoundButton.Size = new System.Drawing.Size(75, 75);
            this.muteSoundButton.TabIndex = 11;
            this.muteSoundButton.Text = "0";
            this.muteSoundButton.UseVisualStyleBackColor = true;
            this.muteSoundButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseDown);
            this.muteSoundButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseUp);
            // 
            // enterNextCompetitorCheckBox
            // 
            this.enterNextCompetitorCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.enterNextCompetitorCheckBox.Location = new System.Drawing.Point(84, 246);
            this.enterNextCompetitorCheckBox.Name = "enterNextCompetitorCheckBox";
            this.enterNextCompetitorCheckBox.Size = new System.Drawing.Size(75, 75);
            this.enterNextCompetitorCheckBox.TabIndex = 10;
            this.enterNextCompetitorCheckBox.Text = "EnterNextCompetitor";
            this.enterNextCompetitorCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.enterNextCompetitorCheckBox.UseVisualStyleBackColor = true;
            this.enterNextCompetitorCheckBox.CheckedChanged += new System.EventHandler(this.CompetitorCheckBox_CheckedChanged);
            // 
            // timeCheckBox
            // 
            this.timeCheckBox.AutoSize = true;
            this.timeCheckBox.Location = new System.Drawing.Point(3, 466);
            this.timeCheckBox.Name = "timeCheckBox";
            this.timeCheckBox.Size = new System.Drawing.Size(49, 17);
            this.timeCheckBox.TabIndex = 18;
            this.timeCheckBox.Text = "Time";
            this.timeCheckBox.UseVisualStyleBackColor = true;
            this.timeCheckBox.CheckedChanged += new System.EventHandler(this.FeatureCheckBox_CheckedChanged);
            // 
            // enterCurrentCompetitorCheckBox
            // 
            this.enterCurrentCompetitorCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.enterCurrentCompetitorCheckBox.Location = new System.Drawing.Point(3, 246);
            this.enterCurrentCompetitorCheckBox.Name = "enterCurrentCompetitorCheckBox";
            this.enterCurrentCompetitorCheckBox.Size = new System.Drawing.Size(75, 75);
            this.enterCurrentCompetitorCheckBox.TabIndex = 9;
            this.enterCurrentCompetitorCheckBox.Text = "EnterCurrentCompetitor";
            this.enterCurrentCompetitorCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.enterCurrentCompetitorCheckBox.UseVisualStyleBackColor = true;
            this.enterCurrentCompetitorCheckBox.CheckedChanged += new System.EventHandler(this.CompetitorCheckBox_CheckedChanged);
            // 
            // passStartButton
            // 
            this.passStartButton.Location = new System.Drawing.Point(165, 327);
            this.passStartButton.Name = "passStartButton";
            this.passStartButton.Size = new System.Drawing.Size(75, 75);
            this.passStartButton.TabIndex = 13;
            this.passStartButton.Text = "PassStart";
            this.passStartButton.UseVisualStyleBackColor = true;
            this.passStartButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseDown);
            this.passStartButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseUp);
            // 
            // passFinishButton
            // 
            this.passFinishButton.Location = new System.Drawing.Point(84, 327);
            this.passFinishButton.Name = "passFinishButton";
            this.passFinishButton.Size = new System.Drawing.Size(75, 75);
            this.passFinishButton.TabIndex = 12;
            this.passFinishButton.Text = "PassFinish";
            this.passFinishButton.UseVisualStyleBackColor = true;
            this.passFinishButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseDown);
            this.passFinishButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseUp);
            // 
            // readyButton
            // 
            this.readyButton.Location = new System.Drawing.Point(165, 408);
            this.readyButton.Name = "readyButton";
            this.readyButton.Size = new System.Drawing.Size(75, 75);
            this.readyButton.TabIndex = 15;
            this.readyButton.Text = "Ready";
            this.readyButton.UseVisualStyleBackColor = true;
            this.readyButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseDown);
            this.readyButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseUp);
            // 
            // resetRunButton
            // 
            this.resetRunButton.Location = new System.Drawing.Point(84, 408);
            this.resetRunButton.Name = "resetRunButton";
            this.resetRunButton.Size = new System.Drawing.Size(75, 75);
            this.resetRunButton.TabIndex = 14;
            this.resetRunButton.Text = "ResetRun";
            this.resetRunButton.UseVisualStyleBackColor = true;
            this.resetRunButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseDown);
            this.resetRunButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.KeyButton_MouseUp);
            // 
            // numericCheckBox
            // 
            this.numericCheckBox.AutoSize = true;
            this.numericCheckBox.Location = new System.Drawing.Point(3, 443);
            this.numericCheckBox.Name = "numericCheckBox";
            this.numericCheckBox.Size = new System.Drawing.Size(65, 17);
            this.numericCheckBox.TabIndex = 17;
            this.numericCheckBox.Text = "Numeric";
            this.numericCheckBox.UseVisualStyleBackColor = true;
            this.numericCheckBox.CheckedChanged += new System.EventHandler(this.FeatureCheckBox_CheckedChanged);
            // 
            // coreCheckBox
            // 
            this.coreCheckBox.AutoSize = true;
            this.coreCheckBox.Location = new System.Drawing.Point(4, 420);
            this.coreCheckBox.Name = "coreCheckBox";
            this.coreCheckBox.Size = new System.Drawing.Size(48, 17);
            this.coreCheckBox.TabIndex = 16;
            this.coreCheckBox.Text = "Core";
            this.coreCheckBox.UseVisualStyleBackColor = true;
            this.coreCheckBox.CheckedChanged += new System.EventHandler(this.FeatureCheckBox_CheckedChanged);
            // 
            // KeypadControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.coreCheckBox);
            this.Controls.Add(this.numericCheckBox);
            this.Controls.Add(this.resetRunButton);
            this.Controls.Add(this.readyButton);
            this.Controls.Add(this.passFinishButton);
            this.Controls.Add(this.passStartButton);
            this.Controls.Add(this.enterCurrentCompetitorCheckBox);
            this.Controls.Add(this.timeCheckBox);
            this.Controls.Add(this.enterNextCompetitorCheckBox);
            this.Controls.Add(this.muteSoundButton);
            this.Controls.Add(this.increaseFaultsButton);
            this.Controls.Add(this.decreaseFaultsButton);
            this.Controls.Add(this.sevenButton);
            this.Controls.Add(this.increaseRefusalsButton);
            this.Controls.Add(this.decreaseRefusalsButton);
            this.Controls.Add(this.fourButton);
            this.Controls.Add(this.toggleEliminationButton);
            this.Controls.Add(this.passIntermediateButton);
            this.Controls.Add(this.playSoundAButton);
            this.MinimumSize = new System.Drawing.Size(243, 486);
            this.Name = "KeypadControl";
            this.Size = new System.Drawing.Size(243, 486);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button playSoundAButton;
        private Button passIntermediateButton;
        private Button toggleEliminationButton;
        private Button fourButton;
        private Button decreaseRefusalsButton;
        private Button increaseRefusalsButton;
        private Button sevenButton;
        private Button decreaseFaultsButton;
        private Button increaseFaultsButton;
        private Button muteSoundButton;
        private CheckBox enterNextCompetitorCheckBox;
        private CheckBox timeCheckBox;
        private CheckBox enterCurrentCompetitorCheckBox;
        private Button passStartButton;
        private Button passFinishButton;
        private Button readyButton;
        private Button resetRunButton;
        private CheckBox numericCheckBox;
        private CheckBox coreCheckBox;
    }
}
