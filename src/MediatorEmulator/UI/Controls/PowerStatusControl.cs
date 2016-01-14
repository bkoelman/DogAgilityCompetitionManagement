using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DogAgilityCompetition.Circe.Session;
using DogAgilityCompetition.WinForms;
using JetBrains.Annotations;

namespace DogAgilityCompetition.MediatorEmulator.UI.Controls
{
    /// <summary>
    /// Enables configuration of power status for emulated wireless devices, such as on/off and blinking.
    /// </summary>
    public sealed partial class PowerStatusControl : UserControl
    {
        public bool SupportsBlink
        {
            get
            {
                return statusLed.Visible;
            }
            set
            {
                statusLed.Visible = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPoweredOn
        {
            get
            {
                return !onButton.Enabled;
            }
            set
            {
                if (value != IsPoweredOn)
                {
                    onButton.Enabled = !value;
                    offButton.Enabled = value;
                    threadSafeIsPoweredOn.Value = value;

                    StatusChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        [NotNull]
        private readonly FreshBoolean threadSafeIsPoweredOn = new FreshBoolean(false);

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ThreadSafeIsPoweredOn => threadSafeIsPoweredOn.Value;

        public event EventHandler StatusChanged;

        public PowerStatusControl()
        {
            InitializeComponent();
        }

        [NotNull]
        public Task BlinkAsync()
        {
            if (SupportsBlink)
            {
                return Task.Factory.StartNew(Blink, CancellationToken.None, TaskCreationOptions.None,
                    TaskScheduler.Default);
            }
            return Task.FromResult((object) null);
        }

        private void Blink()
        {
            this.EnsureOnMainThread(SetBlinkOn);
            Thread.Sleep(1000);
            this.EnsureOnMainThread(SetBlinkOff);
        }

        private void SetBlinkOn()
        {
            statusLed.Blink(100);
        }

        private void SetBlinkOff()
        {
            statusLed.Blink(0);
        }

        private void PowerButton_Click([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            Toggle();
        }

        private void Toggle()
        {
            IsPoweredOn = !IsPoweredOn;
        }
    }
}