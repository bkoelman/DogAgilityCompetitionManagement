using System.ComponentModel;
using System.Windows.Forms;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    partial class CompetitionRunResultsGrid
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            this.runResultsGrid = new System.Windows.Forms.DataGridView();
            this.PlacementColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.competitorNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.handlerNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DogNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CountryCodeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IntermediateTime1Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IntermediateTime2Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IntermediateTime3Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FinishTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FaultCountColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RefusalCountColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsEliminatedColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.competitionRunResultRowBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.runResultsGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.competitionRunResultRowBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // runResultsGrid
            // 
            this.runResultsGrid.AllowUserToAddRows = false;
            this.runResultsGrid.AllowUserToDeleteRows = false;
            this.runResultsGrid.AllowUserToResizeRows = false;
            this.runResultsGrid.AutoGenerateColumns = false;
            this.runResultsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.runResultsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PlacementColumn,
            this.competitorNumberDataGridViewTextBoxColumn,
            this.handlerNameDataGridViewTextBoxColumn,
            this.DogNameColumn,
            this.CountryCodeColumn,
            this.IntermediateTime1Column,
            this.IntermediateTime2Column,
            this.IntermediateTime3Column,
            this.FinishTimeColumn,
            this.FaultCountColumn,
            this.RefusalCountColumn,
            this.IsEliminatedColumn});
            this.runResultsGrid.DataSource = this.competitionRunResultRowBindingSource;
            this.runResultsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runResultsGrid.Location = new System.Drawing.Point(0, 0);
            this.runResultsGrid.Name = "runResultsGrid";
            this.runResultsGrid.ReadOnly = true;
            this.runResultsGrid.RowHeadersVisible = false;
            this.runResultsGrid.Size = new System.Drawing.Size(919, 176);
            this.runResultsGrid.TabIndex = 0;
            this.runResultsGrid.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.RunResultsGrid_CellBeginEdit);
            this.runResultsGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.RunResultsGrid_CellEndEdit);
            this.runResultsGrid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.RunResultsGrid_CellFormatting);
            this.runResultsGrid.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.RunResultsGrid_CellValidating);
            // 
            // PlacementColumn
            // 
            this.PlacementColumn.DataPropertyName = "PlacementText";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.PlacementColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.PlacementColumn.HeaderText = "Place";
            this.PlacementColumn.Name = "PlacementColumn";
            this.PlacementColumn.ReadOnly = true;
            this.PlacementColumn.Width = 50;
            // 
            // competitorNumberDataGridViewTextBoxColumn
            // 
            this.competitorNumberDataGridViewTextBoxColumn.DataPropertyName = "CompetitorNumber";
            this.competitorNumberDataGridViewTextBoxColumn.HeaderText = "Competitor";
            this.competitorNumberDataGridViewTextBoxColumn.Name = "competitorNumberDataGridViewTextBoxColumn";
            this.competitorNumberDataGridViewTextBoxColumn.ReadOnly = true;
            this.competitorNumberDataGridViewTextBoxColumn.Width = 70;
            // 
            // handlerNameDataGridViewTextBoxColumn
            // 
            this.handlerNameDataGridViewTextBoxColumn.DataPropertyName = "HandlerName";
            this.handlerNameDataGridViewTextBoxColumn.HeaderText = "Handler name";
            this.handlerNameDataGridViewTextBoxColumn.Name = "handlerNameDataGridViewTextBoxColumn";
            this.handlerNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.handlerNameDataGridViewTextBoxColumn.Width = 150;
            // 
            // DogNameColumn
            // 
            this.DogNameColumn.DataPropertyName = "DogName";
            this.DogNameColumn.HeaderText = "Dog name";
            this.DogNameColumn.Name = "DogNameColumn";
            this.DogNameColumn.ReadOnly = true;
            this.DogNameColumn.Width = 65;
            // 
            // CountryCodeColumn
            // 
            this.CountryCodeColumn.DataPropertyName = "CountryCode";
            this.CountryCodeColumn.HeaderText = "Country";
            this.CountryCodeColumn.Name = "CountryCodeColumn";
            this.CountryCodeColumn.ReadOnly = true;
            this.CountryCodeColumn.Width = 55;
            // 
            // IntermediateTime1Column
            // 
            this.IntermediateTime1Column.DataPropertyName = "IntermediateTime1";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.IntermediateTime1Column.DefaultCellStyle = dataGridViewCellStyle2;
            this.IntermediateTime1Column.HeaderText = "Intermediate 1";
            this.IntermediateTime1Column.Name = "IntermediateTime1Column";
            this.IntermediateTime1Column.ReadOnly = true;
            this.IntermediateTime1Column.Width = 80;
            // 
            // IntermediateTime2Column
            // 
            this.IntermediateTime2Column.DataPropertyName = "IntermediateTime2";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.IntermediateTime2Column.DefaultCellStyle = dataGridViewCellStyle3;
            this.IntermediateTime2Column.HeaderText = "Intermediate 2";
            this.IntermediateTime2Column.Name = "IntermediateTime2Column";
            this.IntermediateTime2Column.ReadOnly = true;
            this.IntermediateTime2Column.Width = 80;
            // 
            // IntermediateTime3Column
            // 
            this.IntermediateTime3Column.DataPropertyName = "IntermediateTime3";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.IntermediateTime3Column.DefaultCellStyle = dataGridViewCellStyle4;
            this.IntermediateTime3Column.HeaderText = "Intermediate 3";
            this.IntermediateTime3Column.Name = "IntermediateTime3Column";
            this.IntermediateTime3Column.ReadOnly = true;
            this.IntermediateTime3Column.Width = 80;
            // 
            // FinishTimeColumn
            // 
            this.FinishTimeColumn.DataPropertyName = "FinishTime";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.FinishTimeColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.FinishTimeColumn.HeaderText = "Finish";
            this.FinishTimeColumn.Name = "FinishTimeColumn";
            this.FinishTimeColumn.ReadOnly = true;
            this.FinishTimeColumn.Width = 80;
            // 
            // FaultCountColumn
            // 
            this.FaultCountColumn.DataPropertyName = "FaultCount";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.FaultCountColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.FaultCountColumn.HeaderText = "Faults";
            this.FaultCountColumn.Name = "FaultCountColumn";
            this.FaultCountColumn.ReadOnly = true;
            this.FaultCountColumn.Width = 50;
            // 
            // RefusalCountColumn
            // 
            this.RefusalCountColumn.DataPropertyName = "RefusalCount";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.RefusalCountColumn.DefaultCellStyle = dataGridViewCellStyle7;
            this.RefusalCountColumn.HeaderText = "Refusals";
            this.RefusalCountColumn.Name = "RefusalCountColumn";
            this.RefusalCountColumn.ReadOnly = true;
            this.RefusalCountColumn.Width = 60;
            // 
            // IsEliminatedColumn
            // 
            this.IsEliminatedColumn.DataPropertyName = "IsEliminated";
            this.IsEliminatedColumn.HeaderText = "Eliminated";
            this.IsEliminatedColumn.Name = "IsEliminatedColumn";
            this.IsEliminatedColumn.ReadOnly = true;
            this.IsEliminatedColumn.Width = 70;
            // 
            // competitionRunResultRowBindingSource
            // 
            this.competitionRunResultRowBindingSource.AllowNew = false;
            this.competitionRunResultRowBindingSource.DataSource = typeof(CompetitionRunResultRowInGrid);
            // 
            // CompetitionRunResultsGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.runResultsGrid);
            this.Name = "CompetitionRunResultsGrid";
            this.Size = new System.Drawing.Size(919, 176);
            ((System.ComponentModel.ISupportInitialize)(this.runResultsGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.competitionRunResultRowBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView runResultsGrid;
        private BindingSource competitionRunResultRowBindingSource;
        private DataGridViewTextBoxColumn PlacementColumn;
        private DataGridViewTextBoxColumn competitorNumberDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn handlerNameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn DogNameColumn;
        private DataGridViewTextBoxColumn CountryCodeColumn;
        private DataGridViewTextBoxColumn IntermediateTime1Column;
        private DataGridViewTextBoxColumn IntermediateTime2Column;
        private DataGridViewTextBoxColumn IntermediateTime3Column;
        private DataGridViewTextBoxColumn FinishTimeColumn;
        private DataGridViewTextBoxColumn FaultCountColumn;
        private DataGridViewTextBoxColumn RefusalCountColumn;
        private DataGridViewCheckBoxColumn IsEliminatedColumn;
    }
}
