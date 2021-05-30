using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DogAgilityCompetition.Circe.Session;
using DogAgilityCompetition.WinForms;

namespace DogAgilityCompetition.MediatorEmulator.UI.Controls
{
    /// <summary>
    /// Enables configuration of power status for emulated wireless devices, such as on/off and blinking.
    /// </summary>
    public sealed partial class PowerStatusControl : UserControl
    {
        private readonly FreshBoolean threadSafeIsPoweredOn = new(false);

        public bool SupportsBlink
        {
            get => statusLed.Visible;
            set => statusLed.Visible = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPoweredOn
        {
            get => !onButton.Enabled;
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

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ThreadSafeIsPoweredOn => threadSafeIsPoweredOn.Value;

        public event EventHandler? StatusChanged;

        public PowerStatusControl()
        {
            InitializeComponent();
        }

        public Task BlinkAsync()
        {
            if (SupportsBlink)
            {
                return Task.Factory.StartNew(Blink, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
            }

            return Task.CompletedTask;
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

        private void PowerButton_Click(object? sender, EventArgs e)
        {
            Toggle();
        }

        private void Toggle()
        {
            IsPoweredOn = !IsPoweredOn;
        }
    }
}
