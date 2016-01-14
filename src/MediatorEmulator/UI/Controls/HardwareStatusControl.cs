using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.MediatorEmulator.Properties;
using JetBrains.Annotations;

namespace DogAgilityCompetition.MediatorEmulator.UI.Controls
{
    /// <summary>
    /// Enables configuration of status for emulated wireless devices, such as signal strength and battery status.
    /// </summary>
    public sealed partial class HardwareStatusControl : UserControl
    {
        [NotNull]
        private readonly Random randomizer = new Random();

        private DateTime lastVariationUpdate;
        private DateTime clockStartedAt;

        [CanBeNull]
        private DateTime? clockPausedAt;

        [CanBeNull]
        private DateTime? syncRequestReceivedAt;

        public bool SupportsAlignment
        {
            get
            {
                return alignedCheckBox.Visible;
            }
            set
            {
                alignedCheckBox.Visible = value;
            }
        }

        public bool SupportsClock
        {
            get
            {
                return clockLabel.Visible;
            }
            set
            {
                pausePlayClockButton.Visible = value;
                clockLabel.Visible = value;
                syncStateLabel.Visible = value;
            }
        }

        public bool SupportsBatteryStatus
        {
            get
            {
                return batteryStatusTrackBar.Visible;
            }
            set
            {
                if (value != SupportsBatteryStatus)
                {
                    int offsetY = (value ? 1 : -1) * 64;

                    clockLabel.Location = new Point(clockLabel.Location.X, clockLabel.Location.Y + offsetY);
                    syncStateLabel.Location = new Point(syncStateLabel.Location.X, syncStateLabel.Location.Y + offsetY);

                    batteryStatusLabel.Visible = value;
                    batteryStatusTrackBar.Visible = value;
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [CanBeNull]
        public bool? IsAligned
        {
            get
            {
                return SupportsAlignment ? alignedCheckBox.Checked : (bool?) null;
            }
            set
            {
                if (value != IsAligned)
                {
                    alignedCheckBox.Checked = value != null && value.Value;

                    StatusChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool HasVersionMismatch
        {
            get
            {
                return hasVersionMismatchCheckBox.Checked;
            }
            set
            {
                if (value != HasVersionMismatch)
                {
                    hasVersionMismatchCheckBox.Checked = value;

                    StatusChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SignalStrength
        {
            get
            {
                return signalStrengthTrackBar.Value;
            }
            set
            {
                if (value != SignalStrength)
                {
                    signalStrengthTrackBar.Value = value;

                    StatusChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [CanBeNull]
        public int? BatteryStatus
        {
            get
            {
                return !SupportsBatteryStatus ? (int?) null : batteryStatusTrackBar.Value;
            }
            set
            {
                if (value != BatteryStatus)
                {
                    if (value != null)
                    {
                        batteryStatusTrackBar.Value = value.Value;
                    }

                    StatusChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimeSpan ClockValue
        {
            get
            {
                DateTime clockEnd = clockPausedAt ?? SystemContext.UtcNow();
                return SupportsClock ? clockEnd - clockStartedAt : TimeSpan.Zero;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [CanBeNull]
        public ClockSynchronizationStatus? SynchronizationStatus
        {
            get
            {
                if (SupportsClock)
                {
                    if (syncRequestReceivedAt == null)
                    {
                        return ClockSynchronizationStatus.RequiresSync;
                    }
                    if (syncRequestReceivedAt.Value.AddSeconds(4) > SystemContext.UtcNow())
                    {
                        return ClockSynchronizationStatus.SyncSucceeded;
                    }
                }
                return null;
            }
        }

        public event EventHandler StatusChanged;

        public HardwareStatusControl()
        {
            InitializeComponent();
            syncStateLabel.Text = string.Empty;
        }

        public void StartClockSynchronization()
        {
            RaiseChangeOnValueChange(() => SynchronizationStatus, () =>
            {
                DateTime now = SystemContext.UtcNow();
                clockStartedAt = now;
                syncRequestReceivedAt = now;
            });
        }

        private void RaiseChangeOnValueChange<T>([NotNull] Func<T> getValue, [NotNull] Action action)
        {
            T previousValue = getValue();
            action();
            T currentValue = getValue();

            if (!EqualityComparer<T>.Default.Equals(currentValue, previousValue))
            {
                StatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void AutomaticCheckBox_CheckedChanged([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            batteryStatusTrackBar.Enabled = !automaticCheckBox.Checked;
            signalStrengthTrackBar.Enabled = !automaticCheckBox.Checked;
            UpdateTimerEnabled();
        }

        private void HasVersionMismatchCheckBox_CheckedChanged([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            StatusChanged?.Invoke(this, EventArgs.Empty);
        }

        private void AlignedCheckBox_CheckedChanged([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            StatusChanged?.Invoke(this, EventArgs.Empty);
        }

        private void TrackBar_Scroll([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            StatusChanged?.Invoke(this, EventArgs.Empty);
        }

        private void PausePlayClockButton_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            if (clockPausedAt == null)
            {
                clockPausedAt = SystemContext.UtcNow();
                pausePlayClockButton.Image = Resources.PlayButton;
            }
            else
            {
                TimeSpan pauseTime = SystemContext.UtcNow() - clockPausedAt.Value;
                clockStartedAt += pauseTime;
                clockPausedAt = null;
                pausePlayClockButton.Image = Resources.PauseButton;
            }

            UpdateTimerEnabled();
        }

        private void UpdateTimer_Tick([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            if (automaticCheckBox.Checked && lastVariationUpdate.AddMilliseconds(750) < SystemContext.UtcNow())
            {
                if (SupportsBatteryStatus)
                {
                    BatteryStatus = GetNextRandomValue(batteryStatusTrackBar.Value);
                }
                SignalStrength = GetNextRandomValue(signalStrengthTrackBar.Value);
                lastVariationUpdate = SystemContext.UtcNow();
            }

            if (SupportsClock)
            {
                SetClockTo(ClockValue);

                RaiseChangeOnValueChange(() => syncStateLabel.Text,
                    () => { syncStateLabel.Text = SynchronizationStatus.ToString(); });
            }
        }

        private void SetClockTo(TimeSpan elapsed)
        {
            double seconds = Math.Truncate(elapsed.TotalSeconds);
            clockLabel.Text = $"Clock (sec): {seconds:000}.{elapsed:ffff}";
        }

        private int GetNextRandomValue(int current)
        {
            int min = current - 10;
            int max = current + 10;

            int value = randomizer.Next(min, max);
            return Math.Max(0, Math.Min(255, value));
        }

        protected override void OnEnabledChanged([CanBeNull] EventArgs e)
        {
            if (SupportsClock && Enabled)
            {
                clockStartedAt = SystemContext.UtcNow();
                clockPausedAt = clockPausedAt != null ? clockStartedAt : clockPausedAt;
                syncRequestReceivedAt = null;

                SetClockTo(ClockValue);
            }

            UpdateTimerEnabled();

            base.OnEnabledChanged(e);
        }

        private void UpdateTimerEnabled()
        {
            updateTimer.Enabled = Enabled && !DesignMode &&
                (automaticCheckBox.Checked || (SupportsClock && clockPausedAt == null));
        }
    }
}