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
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary>
    /// A grid of wireless network devices.
    /// </summary>
    public sealed partial class NetworkGrid : UserControl
    {
        private const TextFormatFlags TextFlags =
            TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix |
                TextFormatFlags.PreserveGraphicsClipping;

        private const DeviceRoles IntermediateRoles =
            DeviceRoles.IntermediateTimer1 | DeviceRoles.IntermediateTimer2 | DeviceRoles.IntermediateTimer3;

        private static readonly TimeSpan TaskTimeout = TimeSpan.FromSeconds(3);

        [NotNull]
        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Color LightBlue = Color.FromArgb(255, 96, 192, 192);
        private static readonly Color LightGray = Color.FromArgb(255, 192, 192, 192);

        [NotNull]
        private readonly Dictionary<int, Dictionary<Rectangle, DeviceRoles>> rolesCellCheckboxOffsetMap =
            new Dictionary<int, Dictionary<Rectangle, DeviceRoles>>();

        [NotNull]
        private readonly DeviceAddressToRowIndexCache rowCache;

        private bool inStatusMode;
        private bool isConnected;

        [NotNull]
        private static readonly Dictionary<DeviceRoles, string> RoleToDisplayNameLookup =
            new Dictionary<DeviceRoles, string>
            {
                { DeviceRoles.StartTimer, "Start" },
                { DeviceRoles.IntermediateTimer1, "Int1" },
                { DeviceRoles.IntermediateTimer2, "Int2" },
                { DeviceRoles.IntermediateTimer3, "Int3" },
                { DeviceRoles.FinishTimer, "Finish" },
                { DeviceRoles.Keypad, "Keypad" },
                { DeviceRoles.Display, "Display" }
            };

        public bool IsConnected
        {
            get
            {
                return isConnected;
            }
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

        [NotNull]
        public NetworkComposition NetworkComposition { get; private set; }

        public event EventHandler<NetworkSetupEventArgs> NetworkSetupRequested;
        public event EventHandler<AlertEventArgs> AlertRequested;

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

            // ReSharper disable once LoopCanBeConvertedToQuery
            // Reason: Procedural algorithm is more readable and easier to understand here.
            foreach (DataGridViewRow gridViewRow in dataGridView.Rows)
            {
                var rowState = (DeviceRowState) gridViewRow.Tag;

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

        public void AddOrUpdate([NotNull] DeviceStatus status)
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
                Task existingSetupTask = null;
                Task existingAlertTask = null;

                int rowIndex;
                if (!rowCache.Contains(status.DeviceAddress))
                {
                    rowIndex = dataGridView.Rows.Add();
                    rowCache[status.DeviceAddress] = rowIndex;
                }
                else
                {
                    rowIndex = rowCache[status.DeviceAddress];

                    var existingRowState = (DeviceRowState) dataGridView.Rows[rowIndex].Tag;
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

        public void Remove([NotNull] WirelessNetworkAddress deviceAddress)
        {
            Guard.NotNull(deviceAddress, nameof(deviceAddress));

            if (rowCache.Contains(deviceAddress))
            {
                int rowIndex = rowCache[deviceAddress];

                var rowState = (DeviceRowState) dataGridView.Rows[rowIndex].Tag;
                rowState.IsDeleted = true;

                UpdateRowFromState(rowIndex);
            }
        }

        private void UpdateRowFromState(int rowIndex)
        {
            var rowState = (DeviceRowState) dataGridView.Rows[rowIndex].Tag;
            DataGridViewRow gridViewRow = dataGridView.Rows[rowIndex];

            gridViewRow.Cells[UseColumn.Index].Value = rowState.Status.IsInNetwork;
            gridViewRow.Cells[UseColumn.Index].ReadOnly = AllowJoinLeaveNetwork(rowState);
            gridViewRow.Cells[DeviceColumn.Index].Value =
                $"{rowState.Status.DeviceType} ({rowState.Status.DeviceAddress.Value})";
            ((DataGridViewDisableButtonCell) gridViewRow.Cells[BlinkColumn.Index]).Enabled = AllowBlink(rowState);
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
                Color backColor = Color.Empty;

                var rowState = (DeviceRowState) dataGridView.Rows[rowIndex].Tag;
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

            NetworkComposition newComposition = NetworkComposition.Empty;

            // ReSharper disable once LoopCanBeConvertedToQuery
            // Reason: Procedural algorithm is more readable and easier to understand here.
            foreach (DataGridViewRow gridViewRow in dataGridView.Rows)
            {
                var rowState = (DeviceRowState) gridViewRow.Tag;
                if (!rowState.IsDeleted && rowState.Status.IsInNetwork)
                {
                    newComposition = newComposition.ChangeRolesFor(rowState.Status.DeviceAddress,
                        rowState.Status.Capabilities, rowState.Status.Roles);
                }
            }

            if (newComposition != lastConfiguration)
            {
                NetworkComposition = newComposition;
            }
        }

        private void DataGridView_SelectionChanged([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            dataGridView.ClearSelection();
        }

        private void DataGridView_CurrentCellDirtyStateChanged([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            if (dataGridView.IsCurrentCellDirty)
            {
                dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DataGridView_CellContentClick([CanBeNull] object sender, [NotNull] DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var rowState = (DeviceRowState) dataGridView.Rows[e.RowIndex].Tag;
                var taskCancelTokenSource = new CancellationTokenSource();

                if (e.ColumnIndex == BlinkColumn.Index && AllowBlink(rowState))
                {
                    var cell = ((DataGridViewDisableButtonCell) dataGridView.Rows[e.RowIndex].Cells[BlinkColumn.Index]);
                    cell.Enabled = false;

                    var args = new AlertEventArgs(rowState.Status.DeviceAddress, taskCancelTokenSource.Token);
                    OnAlertRequested(args, rowState.Status.DeviceAddress, taskCancelTokenSource);
                }
                else if (e.ColumnIndex == UseColumn.Index && AllowJoinLeaveNetwork(rowState))
                {
                    rowState.Status = rowState.Status.ChangeIsInNetwork(!rowState.Status.IsInNetwork);
                    UpdateRowFromState(e.RowIndex);

                    var args = new NetworkSetupEventArgs(rowState.Status.DeviceAddress, rowState.Status.IsInNetwork,
                        rowState.Status.Roles, taskCancelTokenSource.Token);
                    OnNetworkSetupRequested(args, rowState.Status.DeviceAddress, taskCancelTokenSource);
                }
            }
        }

        private void OnAlertRequested([NotNull] AlertEventArgs args, [NotNull] WirelessNetworkAddress deviceAddress,
            [NotNull] CancellationTokenSource taskCancelTokenSource)
        {
            AlertRequested?.Invoke(this, args);

            if (args.Task != null)
            {
                AutoCancelTaskAfterTimeout(args.Task, taskCancelTokenSource);

                DeviceRowState rowState = GetRowStateForDeviceAddressOrNull(deviceAddress);
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

        private void OnNetworkSetupRequested([NotNull] NetworkSetupEventArgs args,
            [NotNull] WirelessNetworkAddress deviceAddress, [NotNull] CancellationTokenSource taskCancelTokenSource)
        {
            NetworkSetupRequested?.Invoke(this, args);

            if (args.Task != null)
            {
                AutoCancelTaskAfterTimeout(args.Task, taskCancelTokenSource);

                DeviceRowState rowState = GetRowStateForDeviceAddressOrNull(deviceAddress);
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

        private static void AutoCancelTaskAfterTimeout([NotNull] Task taskToWatch,
            [NotNull] CancellationTokenSource taskCancelTokenSource)
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

        [CanBeNull]
        private DeviceRowState GetRowStateForDeviceAddressOrNull([NotNull] WirelessNetworkAddress deviceAddress)
        {
            if (rowCache.Contains(deviceAddress))
            {
                int rowIndex = rowCache[deviceAddress];
                var rowState = (DeviceRowState) dataGridView.Rows[rowIndex].Tag;
                return rowState;
            }
            return null;
        }

        private void TryInvalidateDeviceRow([NotNull] WirelessNetworkAddress deviceAddress)
        {
            if (rowCache.Contains(deviceAddress))
            {
                int rowIndex = rowCache[deviceAddress];
                UpdateRowFromState(rowIndex);
            }
        }

        private void DataGridView_CellMouseClick([CanBeNull] object sender, [NotNull] DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var rowState = (DeviceRowState) dataGridView.Rows[e.RowIndex].Tag;
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
                        var args = new NetworkSetupEventArgs(rowState.Status.DeviceAddress, rowState.Status.IsInNetwork,
                            rowState.Status.Roles, taskCancelTokenSource.Token);
                        OnNetworkSetupRequested(args, rowState.Status.DeviceAddress, taskCancelTokenSource);
                    }
                }
            }
        }

        private DeviceRoles GetRoleClicked(int rowIndex, int x, int y)
        {
            if (rolesCellCheckboxOffsetMap.ContainsKey(rowIndex))
            {
                // ReSharper disable once LoopCanBeConvertedToQuery
                // Reason: Procedural algorithm is more readable and easier to understand here.
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

        private void CleanupTimer_Tick([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            var indexesOfRowsToRemove = new List<int>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            // Reason: Procedural algorithm is more readable and easier to understand here.
            foreach (DataGridViewRow gridViewRow in dataGridView.Rows)
            {
                var rowState = (DeviceRowState) gridViewRow.Tag;

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

        private void DataGridView_CellPainting([CanBeNull] object sender, [NotNull] DataGridViewCellPaintingEventArgs e)
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

        private void PaintUseCell([NotNull] DataGridViewCellPaintingEventArgs e)
        {
            var rowState = (DeviceRowState) dataGridView.Rows[e.RowIndex].Tag;

            CheckBoxState checkState = AllowJoinLeaveNetwork(rowState)
                ? (rowState.Status.IsInNetwork ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal)
                : (rowState.Status.IsInNetwork ? CheckBoxState.CheckedDisabled : CheckBoxState.UncheckedDisabled);

            PaintCheckBoxCell(e, checkState);
            e.Handled = true;
        }

        private void PaintRolesCell([NotNull] DataGridViewCellPaintingEventArgs e)
        {
            Rectangle shiftingRect = e.CellBounds;
            rolesCellCheckboxOffsetMap[e.RowIndex] = new Dictionary<Rectangle, DeviceRoles>();

            var rowState = (DeviceRowState) dataGridView.Rows[e.RowIndex].Tag;
            bool areCheckBoxesEnabled = AllowChangeRoles(rowState);

            e.PaintBackground(e.ClipBounds, true);

            foreach (DeviceRoleAssignment roleAssignment in rowState.RoleSetCached)
            {
                Size checkBoxSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, CheckBoxState.CheckedNormal);
                int checkBoxOffset = (shiftingRect.Height - checkBoxSize.Height) / 2;

                var checkBoxRectRelativeToCell = new Rectangle(shiftingRect.X - e.CellBounds.X + checkBoxOffset,
                    shiftingRect.Y - e.CellBounds.Y + checkBoxOffset, checkBoxSize.Width, checkBoxSize.Height);
                rolesCellCheckboxOffsetMap[e.RowIndex][checkBoxRectRelativeToCell] = roleAssignment.Role;

                CheckBoxState checkState = roleAssignment.IsAssigned
                    ? (areCheckBoxesEnabled ? CheckBoxState.CheckedNormal : CheckBoxState.CheckedDisabled)
                    : (areCheckBoxesEnabled ? CheckBoxState.UncheckedNormal : CheckBoxState.UncheckedDisabled);

                var drawPoint = new Point(shiftingRect.Location.X + checkBoxOffset,
                    shiftingRect.Location.Y + checkBoxOffset);
                CheckBoxRenderer.DrawCheckBox(e.Graphics, drawPoint, checkState);

                int offsetX = checkBoxSize.Width + 2 * checkBoxOffset;
                shiftingRect = new Rectangle(shiftingRect.X + offsetX, shiftingRect.Y, shiftingRect.Width - offsetX,
                    shiftingRect.Height);

                string text = RoleToDisplayNameLookup[roleAssignment.Role];

                Color textColor = e.State.HasFlag(DataGridViewElementStates.Selected)
                    ? e.CellStyle.SelectionForeColor
                    : e.CellStyle.ForeColor;
                Size textSize = TextRenderer.MeasureText(e.Graphics, text, e.CellStyle.Font, shiftingRect.Size,
                    TextFlags);
                TextRenderer.DrawText(e.Graphics, text, e.CellStyle.Font, shiftingRect, textColor, TextFlags);

                shiftingRect = new Rectangle(shiftingRect.X + textSize.Width, shiftingRect.Y,
                    shiftingRect.Width - textSize.Width, shiftingRect.Height);
            }

            e.Handled = true;
        }

        private bool AllowBlink([NotNull] DeviceRowState rowState)
        {
            return !inStatusMode && !rowState.IsDeleted && rowState.AlertTask == null && IsConnected;
        }

        private bool AllowChangeRoles([NotNull] DeviceRowState rowState)
        {
            return !inStatusMode && !rowState.IsDeleted && rowState.Status.IsInNetwork && rowState.SetupTask == null &&
                IsConnected;
        }

        private bool AllowJoinLeaveNetwork([NotNull] DeviceRowState rowState)
        {
            return !inStatusMode && !rowState.IsDeleted && rowState.SetupTask == null && IsConnected;
        }

        private void PaintAlignedCell([NotNull] DataGridViewCellPaintingEventArgs e)
        {
            var rowState = (DeviceRowState) dataGridView.Rows[e.RowIndex].Tag;

            CheckBoxState? checkState = rowState.Status.IsAligned == true
                ? CheckBoxState.CheckedDisabled
                : rowState.Status.IsAligned == false ? CheckBoxState.UncheckedDisabled : (CheckBoxState?) null;

            PaintCheckBoxCell(e, checkState);
            e.Handled = true;
        }

        private void PaintSyncCell([NotNull] DataGridViewCellPaintingEventArgs e)
        {
            var rowState = (DeviceRowState) dataGridView.Rows[e.RowIndex].Tag;
            CheckBoxState? checkState = GetCheckStateFor(rowState.Status.ClockSynchronization);

            PaintCheckBoxCell(e, checkState);
            e.Handled = true;
        }

        [CanBeNull]
        private static CheckBoxState? GetCheckStateFor([CanBeNull] ClockSynchronizationStatus? synchronizationStatus)
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

        private static void PaintCheckBoxCell([NotNull] DataGridViewCellPaintingEventArgs e,
            [CanBeNull] CheckBoxState? checkState)
        {
            e.PaintBackground(e.ClipBounds, true);

            if (checkState != null)
            {
                Size checkBoxSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, CheckBoxState.CheckedDisabled);

                int checkBoxOffsetX = e.CellBounds.Width / 2 - checkBoxSize.Width / 2;
                int checkBoxOffsetY = e.CellBounds.Height / 2 - checkBoxSize.Height / 2;

                var drawPoint = new Point(e.CellBounds.Location.X + checkBoxOffsetX,
                    e.CellBounds.Location.Y + checkBoxOffsetY);

                CheckBoxRenderer.DrawCheckBox(e.Graphics, drawPoint, checkState.Value);
            }
        }

        private sealed class DeviceAddressToRowIndexCache
        {
            [NotNull]
            private readonly DataGridView source;

            [CanBeNull]
            private Dictionary<WirelessNetworkAddress, int> deviceAddressToRowIndexTable;

            public DeviceAddressToRowIndexCache([NotNull] DataGridView source)
            {
                Guard.NotNull(source, nameof(source));

                this.source = source;
            }

            public int this[[NotNull] WirelessNetworkAddress deviceAddress]
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

            public bool Contains([NotNull] WirelessNetworkAddress deviceAddress)
            {
                Guard.NotNull(deviceAddress, nameof(deviceAddress));

                Dictionary<WirelessNetworkAddress, int> table = EnsureRowCache();
                return table.ContainsKey(deviceAddress);
            }

            public void Clear()
            {
                deviceAddressToRowIndexTable = null;
            }

            [NotNull]
            private Dictionary<WirelessNetworkAddress, int> EnsureRowCache()
            {
                if (deviceAddressToRowIndexTable == null)
                {
                    deviceAddressToRowIndexTable = new Dictionary<WirelessNetworkAddress, int>();

                    foreach (DataGridViewRow gridViewRow in source.Rows)
                    {
                        var rowState = (DeviceRowState) gridViewRow.Tag;
                        int rowIndex = gridViewRow.Index;

                        deviceAddressToRowIndexTable[rowState.Status.DeviceAddress] = rowIndex;
                    }
                }
                return deviceAddressToRowIndexTable;
            }
        }

        private sealed class DeviceRowState
        {
            [CanBeNull]
            private DateTime? removedAtUtc;

            [CanBeNull]
            [ItemNotNull]
            private DeviceRoleSet roleSetCached;

            [NotNull]
            private DeviceStatus status;

            /// <summary>
            /// Optional. The currently running Task for SetupNetwork operation.
            /// </summary>
            [CanBeNull]
            public Task SetupTask { get; set; }

            /// <summary>
            /// Optional. The currently running Task for Alert operation.
            /// </summary>
            [CanBeNull]
            public Task AlertTask { get; set; }

            [NotNull]
            public DeviceStatus Status
            {
                get
                {
                    return status;
                }
                set
                {
                    Guard.NotNull(value, nameof(value));
                    status = value;
                }
            }

            public bool IsDeleted
            {
                get
                {
                    return removedAtUtc != null;
                }
                set
                {
                    removedAtUtc = value ? SystemContext.UtcNow() : (DateTime?) null;
                }
            }

            public bool HasExpired => removedAtUtc != null && removedAtUtc.Value.AddSeconds(15) < SystemContext.UtcNow()
                ;

            [NotNull]
            [ItemNotNull]
            public DeviceRoleSet RoleSetCached
            {
                get
                {
                    // ReSharper disable once ConvertIfStatementToNullCoalescingExpression
                    // Reason: Assignment inside expression (side effect) decreases code readability.
                    if (roleSetCached == null)
                    {
                        roleSetCached = new DeviceRoleSet(Status.Capabilities, Status.Roles);
                    }
                    return roleSetCached;
                }
            }

            public DeviceRowState([NotNull] DeviceStatus status)
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
            [NotNull]
            [ItemNotNull]
            private readonly List<DeviceRoleAssignment> items = new List<DeviceRoleAssignment>();

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
                    items.Add(new DeviceRoleAssignment(DeviceRoles.StartTimer,
                        rolesAssigned.HasFlag(DeviceRoles.StartTimer)));
                    items.Add(new DeviceRoleAssignment(DeviceRoles.IntermediateTimer1,
                        rolesAssigned.HasFlag(DeviceRoles.IntermediateTimer1)));
                    items.Add(new DeviceRoleAssignment(DeviceRoles.IntermediateTimer2,
                        rolesAssigned.HasFlag(DeviceRoles.IntermediateTimer2)));
                    items.Add(new DeviceRoleAssignment(DeviceRoles.IntermediateTimer3,
                        rolesAssigned.HasFlag(DeviceRoles.IntermediateTimer3)));
                    items.Add(new DeviceRoleAssignment(DeviceRoles.FinishTimer,
                        rolesAssigned.HasFlag(DeviceRoles.FinishTimer)));
                }
                else
                {
                    if (capabilities.HasFlag(DeviceCapabilities.StartSensor))
                    {
                        items.Add(new DeviceRoleAssignment(DeviceRoles.StartTimer,
                            rolesAssigned.HasFlag(DeviceRoles.StartTimer)));
                    }
                    if (capabilities.HasFlag(DeviceCapabilities.IntermediateSensor))
                    {
                        items.Add(new DeviceRoleAssignment(DeviceRoles.IntermediateTimer1,
                            rolesAssigned.HasFlag(DeviceRoles.IntermediateTimer1)));
                        items.Add(new DeviceRoleAssignment(DeviceRoles.IntermediateTimer2,
                            rolesAssigned.HasFlag(DeviceRoles.IntermediateTimer2)));
                        items.Add(new DeviceRoleAssignment(DeviceRoles.IntermediateTimer3,
                            rolesAssigned.HasFlag(DeviceRoles.IntermediateTimer3)));
                    }
                    if (capabilities.HasFlag(DeviceCapabilities.FinishSensor))
                    {
                        items.Add(new DeviceRoleAssignment(DeviceRoles.FinishTimer,
                            rolesAssigned.HasFlag(DeviceRoles.FinishTimer)));
                    }
                }
            }

            public bool IsInRole(DeviceRoles role)
            {
                IEnumerable<bool> assignments = from roleAssignment in items
                    where roleAssignment.Role == role
                    select roleAssignment.IsAssigned;
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