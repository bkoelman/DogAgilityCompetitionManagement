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
            this.competitorNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.competitorNameDataGridViewTextBoxColumn,
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
            // competitorNameDataGridViewTextBoxColumn
            // 
            this.competitorNameDataGridViewTextBoxColumn.DataPropertyName = "CompetitorName";
            this.competitorNameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.competitorNameDataGridViewTextBoxColumn.Name = "competitorNameDataGridViewTextBoxColumn";
            this.competitorNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.competitorNameDataGridViewTextBoxColumn.Width = 150;
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
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "DogName";
            this.dataGridViewTextBoxColumn1.HeaderText = "Competitor";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 65;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "CountryCode";
            this.dataGridViewTextBoxColumn2.HeaderText = "Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 55;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "IntermediateTime1";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewTextBoxColumn3.HeaderText = "Dog name";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 80;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "IntermediateTime2";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridViewTextBoxColumn4.HeaderText = "Country";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 55;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "IntermediateTime3";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridViewTextBoxColumn5.HeaderText = "Intermediate1";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 80;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "FinishTime";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn6.DefaultCellStyle = dataGridViewCellStyle11;
            this.dataGridViewTextBoxColumn6.HeaderText = "Intermediate 2";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 80;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "FaultCount";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn7.DefaultCellStyle = dataGridViewCellStyle12;
            this.dataGridViewTextBoxColumn7.HeaderText = "Intermediate 3";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 80;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "RefusalCount";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn8.DefaultCellStyle = dataGridViewCellStyle13;
            this.dataGridViewTextBoxColumn8.HeaderText = "Finish";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Width = 80;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "PlacementText";
            this.dataGridViewTextBoxColumn9.HeaderText = "Faults";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            this.dataGridViewTextBoxColumn9.Width = 50;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.HeaderText = "Refusals";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            this.dataGridViewTextBoxColumn10.Width = 60;
            // 
            // CompetitionRunResultsGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.runResultsGrid);
            this.Name = "CompetitionRunResultsGrid";
            this.Size = new System.Drawing.Size(919, 176);
            ((System.ComponentModel.ISupportInitialize)(this.runResultsGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.competitionRunResultRowBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView runResultsGrid;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private BindingSource competitionRunResultRowBindingSource;
        private DataGridViewTextBoxColumn PlacementColumn;
        private DataGridViewTextBoxColumn competitorNumberDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn competitorNameDataGridViewTextBoxColumn;
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
