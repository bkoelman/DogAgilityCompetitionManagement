using System.ComponentModel;
using System.Windows.Forms;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    partial class RunResultEditor
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
            this.editGroupBox = new System.Windows.Forms.GroupBox();
            this.pendingChangeLabel = new System.Windows.Forms.Label();
            this.messageLabel = new System.Windows.Forms.Label();
            this.refusalsUpDown = new System.Windows.Forms.NumericUpDown();
            this.revertButton = new System.Windows.Forms.Button();
            this.faultsUpDown = new System.Windows.Forms.NumericUpDown();
            this.acceptButton = new System.Windows.Forms.Button();
            this.eliminatedCheckBox = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.finishTimeTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.competitorTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.messageHighlighter = new Highlighter();
            this.editGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.refusalsUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.faultsUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // editGroupBox
            // 
            this.editGroupBox.Controls.Add(this.pendingChangeLabel);
            this.editGroupBox.Controls.Add(this.messageLabel);
            this.editGroupBox.Controls.Add(this.refusalsUpDown);
            this.editGroupBox.Controls.Add(this.revertButton);
            this.editGroupBox.Controls.Add(this.faultsUpDown);
            this.editGroupBox.Controls.Add(this.acceptButton);
            this.editGroupBox.Controls.Add(this.eliminatedCheckBox);
            this.editGroupBox.Controls.Add(this.label5);
            this.editGroupBox.Controls.Add(this.label4);
            this.editGroupBox.Controls.Add(this.finishTimeTextBox);
            this.editGroupBox.Controls.Add(this.label3);
            this.editGroupBox.Controls.Add(this.competitorTextBox);
            this.editGroupBox.Controls.Add(this.label2);
            this.editGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editGroupBox.Location = new System.Drawing.Point(0, 0);
            this.editGroupBox.Name = "editGroupBox";
            this.editGroupBox.Size = new System.Drawing.Size(817, 73);
            this.editGroupBox.TabIndex = 0;
            this.editGroupBox.TabStop = false;
            this.editGroupBox.Text = "Edit previous run result";
            // 
            // pendingChangeLabel
            // 
            this.pendingChangeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pendingChangeLabel.AutoSize = true;
            this.pendingChangeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pendingChangeLabel.ForeColor = System.Drawing.Color.DarkRed;
            this.pendingChangeLabel.Location = new System.Drawing.Point(637, 42);
            this.pendingChangeLabel.Name = "pendingChangeLabel";
            this.pendingChangeLabel.Size = new System.Drawing.Size(12, 13);
            this.pendingChangeLabel.TabIndex = 8;
            this.pendingChangeLabel.Text = "*";
            this.pendingChangeLabel.Visible = false;
            // 
            // messageLabel
            // 
            this.messageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.messageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageLabel.Location = new System.Drawing.Point(612, 16);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(199, 13);
            this.messageLabel.TabIndex = 7;
            this.messageLabel.Text = "(message)";
            this.messageLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.messageLabel.Visible = false;
            // 
            // refusalsUpDown
            // 
            this.refusalsUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.refusalsUpDown.Location = new System.Drawing.Point(479, 40);
            this.refusalsUpDown.Name = "refusalsUpDown";
            this.refusalsUpDown.Size = new System.Drawing.Size(52, 20);
            this.refusalsUpDown.TabIndex = 5;
            this.refusalsUpDown.Validating += new System.ComponentModel.CancelEventHandler(this.RefusalsUpDown_Validating);
            this.refusalsUpDown.Validated += new System.EventHandler(this.InputField_ValueChanged);
            // 
            // revertButton
            // 
            this.revertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.revertButton.Location = new System.Drawing.Point(736, 37);
            this.revertButton.Name = "revertButton";
            this.revertButton.Size = new System.Drawing.Size(75, 23);
            this.revertButton.TabIndex = 10;
            this.revertButton.Text = "Re&vert";
            this.revertButton.UseVisualStyleBackColor = true;
            this.revertButton.Click += new System.EventHandler(this.RevertButton_Click);
            // 
            // faultsUpDown
            // 
            this.faultsUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.faultsUpDown.Location = new System.Drawing.Point(399, 40);
            this.faultsUpDown.Name = "faultsUpDown";
            this.faultsUpDown.Size = new System.Drawing.Size(52, 20);
            this.faultsUpDown.TabIndex = 3;
            this.faultsUpDown.Validating += new System.ComponentModel.CancelEventHandler(this.FaultsUpDown_Validating);
            this.faultsUpDown.Validated += new System.EventHandler(this.InputField_ValueChanged);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(655, 37);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(75, 23);
            this.acceptButton.TabIndex = 9;
            this.acceptButton.Text = "&Accept";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // eliminatedCheckBox
            // 
            this.eliminatedCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.eliminatedCheckBox.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.eliminatedCheckBox.Location = new System.Drawing.Point(549, 18);
            this.eliminatedCheckBox.Name = "eliminatedCheckBox";
            this.eliminatedCheckBox.Size = new System.Drawing.Size(59, 40);
            this.eliminatedCheckBox.TabIndex = 6;
            this.eliminatedCheckBox.Text = "&Eliminated";
            this.eliminatedCheckBox.UseVisualStyleBackColor = true;
            this.eliminatedCheckBox.CheckedChanged += new System.EventHandler(this.InputField_ValueChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(476, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "&Refusals";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(396, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "&Faults";
            // 
            // finishTimeTextBox
            // 
            this.finishTimeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.finishTimeTextBox.Location = new System.Drawing.Point(271, 39);
            this.finishTimeTextBox.Name = "finishTimeTextBox";
            this.finishTimeTextBox.Size = new System.Drawing.Size(100, 20);
            this.finishTimeTextBox.TabIndex = 1;
            this.finishTimeTextBox.TextChanged += new System.EventHandler(this.InputField_ValueChanged);
            this.finishTimeTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.FinishTimeTextBox_Validating);
            this.finishTimeTextBox.Validated += new System.EventHandler(this.InputField_ValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(268, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Finis&h";
            // 
            // competitorTextBox
            // 
            this.competitorTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.competitorTextBox.Location = new System.Drawing.Point(9, 39);
            this.competitorTextBox.Name = "competitorTextBox";
            this.competitorTextBox.ReadOnly = true;
            this.competitorTextBox.Size = new System.Drawing.Size(236, 20);
            this.competitorTextBox.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Competitor";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // messageHighlighter
            // 
            this.messageHighlighter.HighlightColor = System.Drawing.Color.Lime;
            this.messageHighlighter.HighlightSpeed = 125;
            this.messageHighlighter.TargetControl = this.messageLabel;
            this.messageHighlighter.HighlightCycleFinished += new System.EventHandler(this.MessageHighlighter_HighlightCycleFinished);
            // 
            // RunResultEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.editGroupBox);
            this.Name = "RunResultEditor";
            this.Size = new System.Drawing.Size(817, 73);
            this.editGroupBox.ResumeLayout(false);
            this.editGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.refusalsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.faultsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox editGroupBox;
        private NumericUpDown refusalsUpDown;
        private Button revertButton;
        private NumericUpDown faultsUpDown;
        private Button acceptButton;
        private CheckBox eliminatedCheckBox;
        private Label label5;
        private Label label4;
        private TextBox finishTimeTextBox;
        private Label label3;
        private TextBox competitorTextBox;
        private Label label2;
        private ErrorProvider errorProvider;
        private Label messageLabel;
        private Highlighter messageHighlighter;
        private Label pendingChangeLabel;
    }
}
