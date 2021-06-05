using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Session;
using DogAgilityCompetition.Controller.Engine;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary>
    /// A grid of wireless network devices.
    /// </summary>
    public sealed partial class NetworkGrid : UserControl
    {
        private const TextFormatFlags TextFlags = TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix |
            TextFormatFlags.PreserveGraphicsClipping;

        private const DeviceRoles IntermediateRoles = DeviceRoles.IntermediateTimer1 | DeviceRoles.IntermediateTimer2 | DeviceRoles.IntermediateTimer3;

        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod()!.DeclaringType!);
        private static readonly TimeSpan TaskTimeout = TimeSpan.FromSeconds(3);
        private static readonly Color LightBlue = Color.FromArgb(255, 96, 192, 192);
        private static readonly Color LightGray = Color.FromArgb(255, 192, 192, 192);

        private static readonly Dictionary<DeviceRoles, string> RoleToDisplayNameLookup = new()
        {
            { DeviceRoles.StartTimer, "Start" },
            { DeviceRoles.IntermediateTimer1, "Int1" },
            { DeviceRoles.IntermediateTimer2, "Int2" },
            { DeviceRoles.IntermediateTimer3, "Int3" },
            { DeviceRoles.FinishTimer, "Finish" },
            { DeviceRoles.Keypad, "Keypad" },
            { DeviceRoles.Display, "Display" }
        };

        private readonly Dictionary<int, Dictionary<Rectangle, DeviceRoles>> rolesCellCheckboxOffsetMap = new();
        private readonly DeviceAddressToRowIndexCache rowCache;

        private bool inStatusMode;
        private bool isConnected;

        public bool IsConnected
        {
            get => isConnected;
            set
            {
                if (isConnected != value)
                {
                    isConnected = value;

                    foreach (DataGridViewRow gridViewRow in dataGridView.Rows)
                    {
                        UpdateRowFromState(gridViewRow.Index);
                    }
                }
            }
        }

        public NetworkComposition NetworkComposition { get; private set; }

        public event EventHandler<NetworkSetupEventArgs>? NetworkSetupRequested;
        public event EventHandler<AlertEventArgs>? AlertRequested;

        public NetworkGrid()
        {
            InitializeComponent();

            NetworkComposition = NetworkComposition.Empty;
            rowCache = new DeviceAddressToRowIndexCache(dataGridView);
        }

        // In 'status' mode, this grid is read-only and only devices in the current logical network are shown.
        public void SwitchToStatusMode()
        {
            inStatusMode = true;

            UseColumn.Visible = false;
            BlinkColumn.Visible = false;

            ImmediatelyRemoveDevicesNotInNetwork();
        }

        private void ImmediatelyRemoveDevicesNotInNetwork()
        {
            var indexesOfRowsToRemove = new List<int>();

            foreach (DataGridViewRow gridViewRow in dataGridView.Rows)
            {
                var rowState = (DeviceRowState)gridViewRow.Tag!;

                if (!rowState.Status.IsInNetwork)
                {
                    int rowIndex = gridViewRow.Index;
                    indexesOfRowsToRemove.Add(rowIndex);
                }
            }

            rowCache.Clear();

            foreach (int rowIndex in Enumerable.Reverse(indexesOfRowsToRemove))
            {
                dataGridView.Rows.RemoveAt(rowIndex);
            }

            rowCache.Clear();
        }

        public void AddOrUpdate(DeviceStatus status)
        {
            Guard.NotNull(status, nameof(status));

            if (inStatusMode && !status.IsInNetwork)
            {
                // Device may have left our network. Remove immediately when found.
                if (rowCache.Contains(status.DeviceAddress))
                {
                    int rowIndex = rowCache[status.DeviceAddress];
                    dataGridView.Rows.RemoveAt(rowIndex);
                    rowCache.Clear();

                    RefreshNetworkComposition();
                }
            }
            else
            {
                Task? existingSetupTask = null;
                Task? existingAlertTask = null;

                int rowIndex;

                if (!rowCache.Contains(status.DeviceAddress))
                {
                    rowIndex = dataGridView.Rows.Add();
                    rowCache[status.DeviceAddress] = rowIndex;
                }
                else
                {
                    rowIndex = rowCache[status.DeviceAddress];

                    var existingRowState = (DeviceRowState)dataGridView.Rows[rowIndex].Tag!;
                    existingSetupTask = existingRowState.SetupTask;
                    existingAlertTask = existingRowState.AlertTask;
                }

                var newRowState = new DeviceRowState(status)
                {
                    SetupTask = existingSetupTask,
                    AlertTask = existingAlertTask
                };

                dataGridView.Rows[rowIndex].Tag = newRowState;

                UpdateRowFromState(rowIndex);
            }
        }

        public void Remove(WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            if (rowCache.Contains(deviceAddress))
            {
                int rowIndex = rowCache[deviceAddress];

                var rowState = (DeviceRowState)dataGridView.Rows[rowIndex].Tag!;
                rowState.IsDeleted = true;

                UpdateRowFromState(rowIndex);
            }
        }

        private void UpdateRowFromState(int rowIndex)
        {
            var rowState = (DeviceRowState)dataGridView.Rows[rowIndex].Tag!;
            DataGridViewRow gridViewRow = dataGridView.Rows[rowIndex];

            gridViewRow.Cells[UseColumn.Index].Value = rowState.Status.IsInNetwork;
            gridViewRow.Cells[UseColumn.Index].ReadOnly = AllowJoinLeaveNetwork(rowState);
            gridViewRow.Cells[DeviceColumn.Index].Value = $"{rowState.Status.DeviceType} ({rowState.Status.DeviceAddress.Value})";
            ((DataGridViewDisableButtonCell)gridViewRow.Cells[BlinkColumn.Index]).Enabled = AllowBlink(rowState);
            gridViewRow.Cells[SignalColumn.Index].Value = rowState.Status.SignalStrength;
            gridViewRow.Cells[BatteryColumn.Index].Value = rowState.Status.BatteryStatus;
            gridViewRow.Cells[AlignedColumn.Index].Value = rowState.Status.IsAligned;
            gridViewRow.Cells[SyncColumn.Index].Value = rowState.Status.ClockSynchronization;

            dataGridView.InvalidateRow(rowIndex);
            UpdateRowBackgroundColor(rowIndex);

            RefreshNetworkComposition();
        }

        private void UpdateRowBackgroundColor(int rowIndex)
        {
            if (rowIndex >= 0)
            {
                var backColor = Color.Empty;
                var rowState = (DeviceRowState)dataGridView.Rows[rowIndex].Tag!;

                if (rowState.IsDeleted)
                {
                    backColor = LightGray;
                }
                else if (!inStatusMode && rowState.Status.IsInNetwork)
                {
                    backColor = LightBlue;
                }

                dataGridView.Rows[rowIndex].DefaultCellStyle.BackColor = backColor;
            }
        }

        private void RefreshNetworkComposition()
        {
            NetworkComposition lastConfiguration = NetworkComposition;

            var newComposition = NetworkComposition.Empty;

            foreach (DataGridViewRow gridViewRow in dataGridView.Rows)
            {
                var rowState = (DeviceRowState)gridViewRow.Tag!;

                if (!rowState.IsDeleted && rowState.Status.IsInNetwork)
                {
                    newComposition = newComposition.ChangeRolesFor(rowState.Status.DeviceAddress, rowState.Status.Capabilities, rowState.Status.Roles);
                }
            }

            if (newComposition != lastConfiguration)
            {
                NetworkComposition = newComposition;
            }
        }

        private void DataGridView_SelectionChanged(object? sender, EventArgs e)
        {
            dataGridView.ClearSelection();
        }

        private void DataGridView_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
        {
            if (dataGridView.IsCurrentCellDirty)
            {
                dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DataGridView_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var rowState = (DeviceRowState)dataGridView.Rows[e.RowIndex].Tag!;
                var taskCancelTokenSource = new CancellationTokenSource();

                if (e.ColumnIndex == BlinkColumn.Index && AllowBlink(rowState))
                {
                    var cell = (DataGridViewDisableButtonCell)dataGridView.Rows[e.RowIndex].Cells[BlinkColumn.Index];
                    cell.Enabled = false;

                    var args = new AlertEventArgs(rowState.Status.DeviceAddress, taskCancelTokenSource.Token);
                    OnAlertRequested(args, rowState.Status.DeviceAddress, taskCancelTokenSource);
                }
                else if (e.ColumnIndex == UseColumn.Index && AllowJoinLeaveNetwork(rowState))
                {
                    rowState.Status = rowState.Status.ChangeIsInNetwork(!rowState.Status.IsInNetwork);
                    UpdateRowFromState(e.RowIndex);

                    var args = new NetworkSetupEventArgs(rowState.Status.DeviceAddress, rowState.Status.IsInNetwork, rowState.Status.Roles,
                        taskCancelTokenSource.Token);

                    OnNetworkSetupRequested(args, rowState.Status.DeviceAddress, taskCancelTokenSource);
                }
            }
        }

        private void OnAlertRequested(AlertEventArgs args, WirelessNetworkAddress deviceAddress, CancellationTokenSource taskCancelTokenSource)
        {
            AlertRequested?.Invoke(this, args);

            if (args.Task != null)
            {
                AutoCancelTaskAfterTimeout(args.Task, taskCancelTokenSource);

                DeviceRowState? rowState = GetRowStateForDeviceAddressOrNull(deviceAddress);

                if (rowState != null)
                {
                    rowState.AlertTask = args.Task;

                    rowState.AlertTask.ContinueWith(t =>
                    {
                        if (t.IsFaulted)
                        {
                            Log.Warn($"Failed sending Alert to device {rowState.Status.DeviceAddress}.", t.Exception);
                        }

                        if (t.IsCanceled)
                        {
                            Log.Warn($"Timeout on sending Alert to device {rowState.Status.DeviceAddress}.");
                        }

                        // Device may no longer exist or have changed after async operation has completed.
                        rowState = GetRowStateForDeviceAddressOrNull(deviceAddress);

                        if (rowState != null)
                        {
                            rowState.AlertTask = null;
                            TryInvalidateDeviceRow(deviceAddress);
                        }
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }

        private void OnNetworkSetupRequested(NetworkSetupEventArgs args, WirelessNetworkAddress deviceAddress, CancellationTokenSource taskCancelTokenSource)
        {
            NetworkSetupRequested?.Invoke(this, args);

            if (args.Task != null)
            {
                AutoCancelTaskAfterTimeout(args.Task, taskCancelTokenSource);

                DeviceRowState? rowState = GetRowStateForDeviceAddressOrNull(deviceAddress);

                if (rowState != null)
                {
                    rowState.SetupTask = args.Task;

                    rowState.SetupTask.ContinueWith(t =>
                    {
                        if (t.IsFaulted)
                        {
                            Log.Warn($"Failed sending Setup to device {rowState.Status.DeviceAddress}.", t.Exception);
                        }

                        if (t.IsCanceled)
                        {
                            Log.Warn($"Timeout on sending Setup to device {rowState.Status.DeviceAddress}.");
                        }

                        // Device may no longer exist or have changed after async operation has completed.
                        rowState = GetRowStateForDeviceAddressOrNull(deviceAddress);

                        if (rowState != null)
                        {
                            rowState.SetupTask = null;
                            TryInvalidateDeviceRow(deviceAddress);
                        }
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }

        private static void AutoCancelTaskAfterTimeout(Task taskToWatch, CancellationTokenSource taskCancelTokenSource)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    bool completed = taskToWatch.Wait(TaskTimeout);

                    if (!completed)
                    {
                        taskCancelTokenSource.Cancel();
                    }
                }
                catch (AggregateException)
                {
                    // Do not handle task errors here, caller should in its continuation.
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
        }

        private DeviceRowState? GetRowStateForDeviceAddressOrNull(WirelessNetworkAddress deviceAddress)
        {
            if (rowCache.Contains(deviceAddress))
            {
                int rowIndex = rowCache[deviceAddress];
                var rowState = (DeviceRowState)dataGridView.Rows[rowIndex].Tag!;
                return rowState;
            }

            return null;
        }

        private void TryInvalidateDeviceRow(WirelessNetworkAddress deviceAddress)
        {
            if (rowCache.Contains(deviceAddress))
            {
                int rowIndex = rowCache[deviceAddress];
                UpdateRowFromState(rowIndex);
            }
        }

        private void DataGridView_CellMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var rowState = (DeviceRowState)dataGridView.Rows[e.RowIndex].Tag!;

                if (e.ColumnIndex == RolesColumn.Index && e.Button == MouseButtons.Left && AllowChangeRoles(rowState))
                {
                    DeviceRoles role = GetRoleClicked(e.RowIndex, e.X, e.Y);

                    if (role != DeviceRoles.None)
                    {
                        bool wasChecked = rowState.RoleSetCached.IsInRole(role);

                        if (wasChecked)
                        {
                            rowState.Status = rowState.Status.ChangeRoles(rowState.Status.Roles & ~role);
                        }
                        else
                        {
                            DeviceRoles newRoles = rowState.Status.Roles;

                            var rolesToRemove = DeviceRoles.None;

                            if ((rowState.Status.Capabilities & DeviceCapabilities.TimeSensor) != 0)
                            {
                                // Do not allow multiple timer roles for a Gate (except Start + Finish).
                                if (role == DeviceRoles.StartTimer || role == DeviceRoles.FinishTimer)
                                {
                                    rolesToRemove = IntermediateRoles;
                                }
                                else
                                {
                                    rolesToRemove = DeviceRoles.StartTimer | IntermediateRoles | DeviceRoles.FinishTimer;
                                }
                            }
                            else
                            {
                                if ((role & IntermediateRoles) != 0)
                                {
                                    // Do not allow multiple Intermediate roles selected.
                                    rolesToRemove = IntermediateRoles;
                                }
                            }

                            newRoles &= ~rolesToRemove;
                            newRoles |= role;

                            rowState.Status = rowState.Status.ChangeRoles(newRoles);
                        }

                        rowState.ClearRoleSetCache();
                        UpdateRowFromState(e.RowIndex);

                        var taskCancelTokenSource = new CancellationTokenSource();

                        var args = new NetworkSetupEventArgs(rowState.Status.DeviceAddress, rowState.Status.IsInNetwork, rowState.Status.Roles,
                            taskCancelTokenSource.Token);

                        OnNetworkSetupRequested(args, rowState.Status.DeviceAddress, taskCancelTokenSource);
                    }
                }
            }
        }

        private DeviceRoles GetRoleClicked(int rowIndex, int x, int y)
        {
            if (rolesCellCheckboxOffsetMap.ContainsKey(rowIndex))
            {
                foreach (KeyValuePair<Rectangle, DeviceRoles> rectWithRole in rolesCellCheckboxOffsetMap[rowIndex])
                {
                    if (rectWithRole.Key.Contains(x, y))
                    {
                        return rectWithRole.Value;
                    }
                }
            }

            return DeviceRoles.None;
        }

        private void CleanupTimer_Tick(object? sender, EventArgs e)
        {
            var indexesOfRowsToRemove = new List<int>();

            foreach (DataGridViewRow gridViewRow in dataGridView.Rows)
            {
                var rowState = (DeviceRowState)gridViewRow.Tag!;

                if (rowState.HasExpired)
                {
                    int rowIndex = gridViewRow.Index;
                    indexesOfRowsToRemove.Add(rowIndex);
                }
            }

            rowCache.Clear();

            foreach (int rowIndex in Enumerable.Reverse(indexesOfRowsToRemove))
            {
                dataGridView.Rows.RemoveAt(rowIndex);
            }

            rowCache.Clear();
        }

        private void DataGridView_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == UseColumn.Index)
                {
                    PaintUseCell(e);
                }
                else if (e.ColumnIndex == RolesColumn.Index)
                {
                    PaintRolesCell(e);
                }
                else if (e.ColumnIndex == AlignedColumn.Index)
                {
                    PaintAlignedCell(e);
                }
                else if (e.ColumnIndex == SyncColumn.Index)
                {
                    PaintSyncCell(e);
                }
            }
        }

        private void PaintUseCell(DataGridViewCellPaintingEventArgs e)
        {
            var rowState = (DeviceRowState)dataGridView.Rows[e.RowIndex].Tag!;

            // @formatter:keep_existing_linebreaks true

            CheckBoxState checkState = AllowJoinLeaveNetwork(rowState)
                ? rowState.Status.IsInNetwork
                    ? CheckBoxState.CheckedNormal
                    : CheckBoxState.UncheckedNormal
                : rowState.Status.IsInNetwork
                    ? CheckBoxState.CheckedDisabled
                    : CheckBoxState.UncheckedDisabled;

            // @formatter:keep_existing_linebreaks restore

            PaintCheckBoxCell(e, checkState);
            e.Handled = true;
        }

        private void PaintRolesCell(DataGridViewCellPaintingEventArgs e)
        {
            Rectangle shiftingRect = e.CellBounds;
            rolesCellCheckboxOffsetMap[e.RowIndex] = new Dictionary<Rectangle, DeviceRoles>();

            var rowState = (DeviceRowState)dataGridView.Rows[e.RowIndex].Tag!;
            bool areCheckBoxesEnabled = AllowChangeRoles(rowState);

            e.PaintBackground(e.ClipBounds, true);

            foreach (DeviceRoleAssignment roleAssignment in rowState.RoleSetCached)
            {
                Size checkBoxSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, CheckBoxState.CheckedNormal);
                int checkBoxOffset = (shiftingRect.Height - checkBoxSize.Height) / 2;

                var checkBoxRectRelativeToCell = new Rectangle(shiftingRect.X - e.CellBounds.X + checkBoxOffset,
                    shiftingRect.Y - e.CellBounds.Y + checkBoxOffset, checkBoxSize.Width, checkBoxSize.Height);

                rolesCellCheckboxOffsetMap[e.RowIndex][checkBoxRectRelativeToCell] = roleAssignment.Role;

                // @formatter:keep_existing_linebreaks true

                CheckBoxState checkState = roleAssignment.IsAssigned
                    ? areCheckBoxesEnabled
                        ? CheckBoxState.CheckedNormal
                        : CheckBoxState.CheckedDisabled
                    : areCheckBoxesEnabled
                        ? CheckBoxState.UncheckedNormal
                        : CheckBoxState.UncheckedDisabled;

                // @formatter:keep_existing_linebreaks restore

                var drawPoint = new Point(shiftingRect.Location.X + checkBoxOffset, shiftingRect.Location.Y + checkBoxOffset);
                CheckBoxRenderer.DrawCheckBox(e.Graphics, drawPoint, checkState);

                int offsetX = checkBoxSize.Width + 2 * checkBoxOffset;
                shiftingRect = new Rectangle(shiftingRect.X + offsetX, shiftingRect.Y, shiftingRect.Width - offsetX, shiftingRect.Height);

                string text = RoleToDisplayNameLookup[roleAssignment.Role];

                Color textColor = e.State.HasFlag(DataGridViewElementStates.Selected) ? e.CellStyle.SelectionForeColor : e.CellStyle.ForeColor;
                Size textSize = TextRenderer.MeasureText(e.Graphics, text, e.CellStyle.Font, shiftingRect.Size, TextFlags);
                TextRenderer.DrawText(e.Graphics, text, e.CellStyle.Font, shiftingRect, textColor, TextFlags);

                shiftingRect = new Rectangle(shiftingRect.X + textSize.Width, shiftingRect.Y, shiftingRect.Width - textSize.Width, shiftingRect.Height);
            }

            e.Handled = true;
        }

        private bool AllowBlink(DeviceRowState rowState)
        {
            return !inStatusMode && !rowState.IsDeleted && rowState.AlertTask == null && IsConnected;
        }

        private bool AllowChangeRoles(DeviceRowState rowState)
        {
            return !inStatusMode && !rowState.IsDeleted && rowState.Status.IsInNetwork && rowState.SetupTask == null && IsConnected;
        }

        private bool AllowJoinLeaveNetwork(DeviceRowState rowState)
        {
            return !inStatusMode && !rowState.IsDeleted && rowState.SetupTask == null && IsConnected;
        }

        private void PaintAlignedCell(DataGridViewCellPaintingEventArgs e)
        {
            var rowState = (DeviceRowState)dataGridView.Rows[e.RowIndex].Tag!;

            // @formatter:keep_existing_linebreaks true

            CheckBoxState? checkState = rowState.Status.IsAligned == true
                ? CheckBoxState.CheckedDisabled
                : rowState.Status.IsAligned == false
                    ? CheckBoxState.UncheckedDisabled
                    : null;

            // @formatter:keep_existing_linebreaks restore

            PaintCheckBoxCell(e, checkState);
            e.Handled = true;
        }

        private void PaintSyncCell(DataGridViewCellPaintingEventArgs e)
        {
            var rowState = (DeviceRowState)dataGridView.Rows[e.RowIndex].Tag!;
            CheckBoxState? checkState = GetCheckStateFor(rowState.Status.ClockSynchronization);

            PaintCheckBoxCell(e, checkState);
            e.Handled = true;
        }

        private static CheckBoxState? GetCheckStateFor(ClockSynchronizationStatus? synchronizationStatus)
        {
            switch (synchronizationStatus)
            {
                case ClockSynchronizationStatus.SyncSucceeded:
                    return CheckBoxState.CheckedDisabled;
                case ClockSynchronizationStatus.RequiresSync:
                    return CheckBoxState.UncheckedDisabled;
                default:
                    return null;
            }
        }

        private static void PaintCheckBoxCell(DataGridViewCellPaintingEventArgs e, CheckBoxState? checkState)
        {
            e.PaintBackground(e.ClipBounds, true);

            if (checkState != null)
            {
                Size checkBoxSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, CheckBoxState.CheckedDisabled);

                int checkBoxOffsetX = e.CellBounds.Width / 2 - checkBoxSize.Width / 2;
                int checkBoxOffsetY = e.CellBounds.Height / 2 - checkBoxSize.Height / 2;

                var drawPoint = new Point(e.CellBounds.Location.X + checkBoxOffsetX, e.CellBounds.Location.Y + checkBoxOffsetY);

                CheckBoxRenderer.DrawCheckBox(e.Graphics, drawPoint, checkState.Value);
            }
        }

        private sealed class DeviceAddressToRowIndexCache
        {
            private readonly DataGridView source;

            private Dictionary<WirelessNetworkAddress, int>? deviceAddressToRowIndexTable;

            public int this[WirelessNetworkAddress deviceAddress]
            {
                get
                {
                    Guard.NotNull(deviceAddress, nameof(deviceAddress));

                    Dictionary<WirelessNetworkAddress, int> table = EnsureRowCache();

                    if (!table.ContainsKey(deviceAddress))
                    {
                        throw new InvalidOperationException($"No row found for device {deviceAddress}.");
                    }

                    return table[deviceAddress];
                }
                set
                {
                    Guard.NotNull(deviceAddress, nameof(deviceAddress));

                    Dictionary<WirelessNetworkAddress, int> table = EnsureRowCache();
                    table[deviceAddress] = value;
                }
            }

            public DeviceAddressToRowIndexCache(DataGridView source)
            {
                Guard.NotNull(source, nameof(source));

                this.source = source;
            }

            public bool Contains(WirelessNetworkAddress deviceAddress)
            {
                Guard.NotNull(deviceAddress, nameof(deviceAddress));

                Dictionary<WirelessNetworkAddress, int> table = EnsureRowCache();
                return table.ContainsKey(deviceAddress);
            }

            public void Clear()
            {
                deviceAddressToRowIndexTable = null;
            }

            private Dictionary<WirelessNetworkAddress, int> EnsureRowCache()
            {
                if (deviceAddressToRowIndexTable == null)
                {
                    deviceAddressToRowIndexTable = new Dictionary<WirelessNetworkAddress, int>();

                    foreach (DataGridViewRow gridViewRow in source.Rows)
                    {
                        var rowState = (DeviceRowState)gridViewRow.Tag!;
                        int rowIndex = gridViewRow.Index;

                        deviceAddressToRowIndexTable[rowState.Status.DeviceAddress] = rowIndex;
                    }
                }

                return deviceAddressToRowIndexTable;
            }
        }

        private sealed class DeviceRowState
        {
            private DateTime? removedAtUtc;
            private DeviceRoleSet? roleSetCached;
            private DeviceStatus status;

            /// <summary>
            /// Optional. The currently running Task for SetupNetwork operation.
            /// </summary>
            public Task? SetupTask { get; set; }

            /// <summary>
            /// Optional. The currently running Task for Alert operation.
            /// </summary>
            public Task? AlertTask { get; set; }

            public DeviceStatus Status
            {
                get => status;
                set
                {
                    Guard.NotNull(value, nameof(value));
                    status = value;
                }
            }

            public bool IsDeleted
            {
                get => removedAtUtc != null;
                set => removedAtUtc = value ? SystemContext.UtcNow() : null;
            }

            public bool HasExpired => removedAtUtc != null && removedAtUtc.Value.AddSeconds(15) < SystemContext.UtcNow();

            public DeviceRoleSet RoleSetCached
            {
                get
                {
                    return roleSetCached ??= new DeviceRoleSet(Status.Capabilities, Status.Roles);
                }
            }

            public DeviceRowState(DeviceStatus status)
            {
                Guard.NotNull(status, nameof(status));
                this.status = status;
            }

            public void ClearRoleSetCache()
            {
                roleSetCached = null;
            }
        }

        private sealed class DeviceRoleSet : IEnumerable<DeviceRoleAssignment>
        {
            private readonly List<DeviceRoleAssignment> items = new();

            public DeviceRoleSet(DeviceCapabilities capabilities, DeviceRoles rolesAssigned)
            {
                if (capabilities.HasFlag(DeviceCapabilities.ControlKeypad))
                {
                    items.Add(new DeviceRoleAssignment(DeviceRoles.Keypad, rolesAssigned.HasFlag(DeviceRoles.Keypad)));
                }
                else if (capabilities.HasFlag(DeviceCapabilities.Display))
                {
                    items.Add(new DeviceRoleAssignment(DeviceRoles.Display, rolesAssigned.HasFlag(DeviceRoles.Display)));
                }

                if (capabilities.HasFlag(DeviceCapabilities.TimeSensor))
                {
                    items.Add(new DeviceRoleAssignment(DeviceRoles.StartTimer, rolesAssigned.HasFlag(DeviceRoles.StartTimer)));
                    items.Add(new DeviceRoleAssignment(DeviceRoles.IntermediateTimer1, rolesAssigned.HasFlag(DeviceRoles.IntermediateTimer1)));
                    items.Add(new DeviceRoleAssignment(DeviceRoles.IntermediateTimer2, rolesAssigned.HasFlag(DeviceRoles.IntermediateTimer2)));
                    items.Add(new DeviceRoleAssignment(DeviceRoles.IntermediateTimer3, rolesAssigned.HasFlag(DeviceRoles.IntermediateTimer3)));
                    items.Add(new DeviceRoleAssignment(DeviceRoles.FinishTimer, rolesAssigned.HasFlag(DeviceRoles.FinishTimer)));
                }
                else
                {
                    if (capabilities.HasFlag(DeviceCapabilities.StartSensor))
                    {
                        items.Add(new DeviceRoleAssignment(DeviceRoles.StartTimer, rolesAssigned.HasFlag(DeviceRoles.StartTimer)));
                    }

                    if (capabilities.HasFlag(DeviceCapabilities.IntermediateSensor))
                    {
                        items.Add(new DeviceRoleAssignment(DeviceRoles.IntermediateTimer1, rolesAssigned.HasFlag(DeviceRoles.IntermediateTimer1)));
                        items.Add(new DeviceRoleAssignment(DeviceRoles.IntermediateTimer2, rolesAssigned.HasFlag(DeviceRoles.IntermediateTimer2)));
                        items.Add(new DeviceRoleAssignment(DeviceRoles.IntermediateTimer3, rolesAssigned.HasFlag(DeviceRoles.IntermediateTimer3)));
                    }

                    if (capabilities.HasFlag(DeviceCapabilities.FinishSensor))
                    {
                        items.Add(new DeviceRoleAssignment(DeviceRoles.FinishTimer, rolesAssigned.HasFlag(DeviceRoles.FinishTimer)));
                    }
                }
            }

            public bool IsInRole(DeviceRoles role)
            {
                IEnumerable<bool> assignments = from roleAssignment in items where roleAssignment.Role == role select roleAssignment.IsAssigned;
                return assignments.FirstOrDefault();
            }

            public IEnumerator<DeviceRoleAssignment> GetEnumerator()
            {
                return items.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private sealed class DeviceRoleAssignment
        {
            public DeviceRoles Role { get; }
            public bool IsAssigned { get; }

            public DeviceRoleAssignment(DeviceRoles role, bool isAssigned)
            {
                Role = role;
                IsAssigned = isAssigned;
            }
        }
    }
}
