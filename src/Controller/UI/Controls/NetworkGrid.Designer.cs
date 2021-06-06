using System.ComponentModel;
using System.Windows.Forms;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    partial class NetworkGrid
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
            this.cleanupTimer = new System.Windows.Forms.Timer(this.components);
            this.dataGridView = new NonFlickeringDataGridView();
            this.UseColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DeviceColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BlinkColumn = new DataGridViewDisableButtonColumn();
            this.RolesColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SignalColumn = new DataGridViewProgressBarColumn();
            this.BatteryColumn = new DataGridViewProgressBarColumn();
            this.AlignedColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.SyncColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // cleanupTimer
            // 
            this.cleanupTimer.Enabled = true;
            this.cleanupTimer.Interval = 3000;
            this.cleanupTimer.Tick += new System.EventHandler(this.CleanupTimer_Tick);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToResizeColumns = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UseColumn,
            this.DeviceColumn,
            this.BlinkColumn,
            this.RolesColumn,
            this.SignalColumn,
            this.BatteryColumn,
            this.AlignedColumn,
            this.SyncColumn});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.ShowCellErrors = false;
            this.dataGridView.ShowCellToolTips = false;
            this.dataGridView.ShowEditingIcon = false;
            this.dataGridView.ShowRowErrors = false;
            this.dataGridView.Size = new System.Drawing.Size(868, 269);
            this.dataGridView.StandardTab = true;
            this.dataGridView.TabIndex = 0;
            this.dataGridView.TabStop = false;
            this.dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellContentClick);
            this.dataGridView.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridView_CellMouseClick);
            this.dataGridView.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DataGridView_CellPainting);
            this.dataGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.DataGridView_CurrentCellDirtyStateChanged);
            this.dataGridView.SelectionChanged += new System.EventHandler(this.DataGridView_SelectionChanged);
            // 
            // UseColumn
            // 
            this.UseColumn.HeaderText = "Use";
            this.UseColumn.Name = "UseColumn";
            this.UseColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.UseColumn.Width = 40;
            // 
            // DeviceColumn
            // 
            this.DeviceColumn.HeaderText = "Device";
            this.DeviceColumn.Name = "DeviceColumn";
            this.DeviceColumn.ReadOnly = true;
            this.DeviceColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DeviceColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DeviceColumn.Width = 150;
            // 
            // BlinkColumn
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.BlinkColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.BlinkColumn.Enabled = true;
            this.BlinkColumn.HeaderText = "";
            this.BlinkColumn.Name = "BlinkColumn";
            this.BlinkColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.BlinkColumn.Text = "Blink";
            this.BlinkColumn.UseColumnTextForButtonValue = true;
            this.BlinkColumn.Width = 50;
            // 
            // RolesColumn
            // 
            this.RolesColumn.HeaderText = "Roles";
            this.RolesColumn.Name = "RolesColumn";
            this.RolesColumn.ReadOnly = true;
            this.RolesColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.RolesColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.RolesColumn.Width = 320;
            // 
            // SignalColumn
            // 
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(2);
            this.SignalColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.SignalColumn.HeaderText = "Signal";
            this.SignalColumn.Maximum = 255;
            this.SignalColumn.Minimum = 0;
            this.SignalColumn.Name = "SignalColumn";
            this.SignalColumn.ReadOnly = true;
            this.SignalColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.SignalColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SignalColumn.Width = 70;
            // 
            // BatteryColumn
            // 
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(2);
            this.BatteryColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.BatteryColumn.HeaderText = "Battery";
            this.BatteryColumn.Maximum = 255;
            this.BatteryColumn.Minimum = 0;
            this.BatteryColumn.Name = "BatteryColumn";
            this.BatteryColumn.ReadOnly = true;
            this.BatteryColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.BatteryColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.BatteryColumn.Width = 70;
            // 
            // AlignedColumn
            // 
            this.AlignedColumn.HeaderText = "Aligned";
            this.AlignedColumn.Name = "AlignedColumn";
            this.AlignedColumn.ReadOnly = true;
            this.AlignedColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.AlignedColumn.Width = 70;
            // 
            // SyncColumn
            // 
            this.SyncColumn.HeaderText = "Sync";
            this.SyncColumn.Name = "SyncColumn";
            this.SyncColumn.ReadOnly = true;
            this.SyncColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.SyncColumn.Width = 50;
            // 
            // NetworkGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.dataGridView);
            this.Name = "NetworkGrid";
            this.Size = new System.Drawing.Size(868, 269);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private NonFlickeringDataGridView dataGridView;
        private Timer cleanupTimer;
        private DataGridViewCheckBoxColumn UseColumn;
        private DataGridViewTextBoxColumn DeviceColumn;
        private DataGridViewDisableButtonColumn BlinkColumn;
        private DataGridViewTextBoxColumn RolesColumn;
        private DataGridViewProgressBarColumn SignalColumn;
        private DataGridViewProgressBarColumn BatteryColumn;
        private DataGridViewCheckBoxColumn AlignedColumn;
        private DataGridViewCheckBoxColumn SyncColumn;
    }
}
