using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.MediatorEmulator.Engine;

namespace DogAgilityCompetition.MediatorEmulator.UI.Controls
{
    /// <summary>
    /// The pressable keys of a wireless remote control.
    /// </summary>
    public sealed partial class KeypadControl : UserControl
    {
        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        private static readonly IReadOnlyCollection<RawDeviceKeys> ModifierRawKeys = new ReadOnlyCollection<RawDeviceKeys>(new List<RawDeviceKeys>
        {
            RawDeviceKeys.EnterCurrentCompetitor,
            RawDeviceKeys.EnterNextCompetitor
        });

        private readonly Lazy<Dictionary<ButtonBase, RawDeviceKeys>> buttonToKeyLookupLazy;

        private RawDeviceKeys keysDown = RawDeviceKeys.None;

        private bool InDigitEntryMode => enterCurrentCompetitorCheckBox.Checked || enterNextCompetitorCheckBox.Checked;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RemoteEmulatorFeatures Features
        {
            get
            {
                var features = RemoteEmulatorFeatures.None;
                features |= coreCheckBox.Checked ? RemoteEmulatorFeatures.CoreKeys : 0;
                features |= numericCheckBox.Checked ? RemoteEmulatorFeatures.NumericKeys : 0;
                features |= timeCheckBox.Checked ? RemoteEmulatorFeatures.TimerKeys : 0;
                return features;
            }
            set
            {
                if (value != Features)
                {
                    coreCheckBox.Checked = (value & RemoteEmulatorFeatures.CoreKeys) != 0;
                    numericCheckBox.Checked = (value & RemoteEmulatorFeatures.NumericKeys) != 0;
                    timeCheckBox.Checked = (value & RemoteEmulatorFeatures.TimerKeys) != 0;

                    StatusChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler? StatusChanged;
        public event EventHandler<EventArgs<RawDeviceKeys>>? KeysDownChanged;

        public KeypadControl()
        {
            InitializeComponent();

            buttonToKeyLookupLazy = new Lazy<Dictionary<ButtonBase, RawDeviceKeys>>(CreateButtonToKeyLookupTable);

            UpdateButtons();
        }

        private Dictionary<ButtonBase, RawDeviceKeys> CreateButtonToKeyLookupTable()
        {
            return new()
            {
                { playSoundAButton, RawDeviceKeys.Key1OrPlaySoundA },
                { passIntermediateButton, RawDeviceKeys.Key2OrPassIntermediate },
                { toggleEliminationButton, RawDeviceKeys.Key3OrToggleElimination },
                { fourButton, RawDeviceKeys.Key4 },
                { decreaseRefusalsButton, RawDeviceKeys.Key5OrDecreaseRefusals },
                { increaseRefusalsButton, RawDeviceKeys.Key6OrIncreaseRefusals },
                { sevenButton, RawDeviceKeys.Key7 },
                { decreaseFaultsButton, RawDeviceKeys.Key8OrDecreaseFaults },
                { increaseFaultsButton, RawDeviceKeys.Key9OrIncreaseFaults },
                { enterCurrentCompetitorCheckBox, RawDeviceKeys.EnterCurrentCompetitor },
                { enterNextCompetitorCheckBox, RawDeviceKeys.EnterNextCompetitor },
                { muteSoundButton, RawDeviceKeys.Key0OrMuteSound },
                { passFinishButton, RawDeviceKeys.PassFinish },
                { passStartButton, RawDeviceKeys.PassStart },
                { resetRunButton, RawDeviceKeys.ResetRun },
                { readyButton, RawDeviceKeys.Ready }
            };
        }

        private void UpdateButtons()
        {
            SetButtonEnabledStates();
            SetButtonCaptions();
        }

        private void SetButtonEnabledStates()
        {
            bool numericOnly = (Features & RemoteEmulatorFeatures.NumericKeys) != 0;
            bool timerOnly = (Features & RemoteEmulatorFeatures.TimerKeys) != 0;
            bool coreOnly = (Features & RemoteEmulatorFeatures.CoreKeys) != 0;
            bool numericOrCore = (Features & (RemoteEmulatorFeatures.NumericKeys | RemoteEmulatorFeatures.CoreKeys)) != 0;
            bool numericOrTimer = (Features & (RemoteEmulatorFeatures.NumericKeys | RemoteEmulatorFeatures.TimerKeys)) != 0;

            playSoundAButton.Enabled = numericOrCore;
            passIntermediateButton.Enabled = numericOrTimer;
            toggleEliminationButton.Enabled = numericOrCore;
            fourButton.Enabled = numericOnly;
            decreaseRefusalsButton.Enabled = numericOrCore;
            increaseRefusalsButton.Enabled = numericOrCore;
            sevenButton.Enabled = numericOnly;
            decreaseFaultsButton.Enabled = numericOrCore;
            increaseFaultsButton.Enabled = numericOrCore;
            enterCurrentCompetitorCheckBox.Enabled = numericOnly;
            enterNextCompetitorCheckBox.Enabled = numericOnly;
            muteSoundButton.Enabled = numericOrCore;
            passFinishButton.Enabled = timerOnly;
            passStartButton.Enabled = timerOnly;
            resetRunButton.Enabled = coreOnly;
            readyButton.Enabled = coreOnly;
        }

        private void SetButtonCaptions()
        {
            var buttons = new ButtonBase[]
            {
                playSoundAButton,
                passIntermediateButton,
                toggleEliminationButton,
                fourButton,
                decreaseRefusalsButton,
                increaseRefusalsButton,
                sevenButton,
                decreaseFaultsButton,
                increaseFaultsButton,
                enterCurrentCompetitorCheckBox,
                enterNextCompetitorCheckBox,
                muteSoundButton,
                passFinishButton,
                passStartButton,
                resetRunButton,
                readyButton
            };

            foreach (ButtonBase button in buttons)
            {
                RawDeviceKeys key = GetKeyForButton(button);
                button.Text = GetButtonText(key);
            }
        }

        private RawDeviceKeys GetKeyForButton(ButtonBase button)
        {
            if (!buttonToKeyLookupLazy.Value.ContainsKey(button))
            {
                throw new InvalidOperationException($"Cannot get key for button {button.Name}.");
            }

            return buttonToKeyLookupLazy.Value[button];
        }

        private string GetButtonText(RawDeviceKeys key)
        {
            bool hasCore = (Features & RemoteEmulatorFeatures.CoreKeys) != 0;
            bool hasNumeric = (Features & RemoteEmulatorFeatures.NumericKeys) != 0;
            bool hasTimer = (Features & RemoteEmulatorFeatures.TimerKeys) != 0;
            bool inEntry = InDigitEntryMode;

            switch (key)
            {
                case RawDeviceKeys.Key1OrPlaySoundA:
                    return hasNumeric && (!hasCore || inEntry) ? "1" : hasCore ? "Sound A" : string.Empty;
                case RawDeviceKeys.Key2OrPassIntermediate:
                    return hasNumeric && (!hasTimer || inEntry) ? "2" : hasTimer ? "Intermediate" : string.Empty;
                case RawDeviceKeys.Key3OrToggleElimination:
                    return hasNumeric && (!hasCore || inEntry) ? "3" : hasCore ? "E" : string.Empty;
                case RawDeviceKeys.Key4:
                    return hasNumeric ? "4" : string.Empty;
                case RawDeviceKeys.Key5OrDecreaseRefusals:
                    return hasNumeric && (!hasCore || inEntry) ? "5" : hasCore ? "R-" : string.Empty;
                case RawDeviceKeys.Key6OrIncreaseRefusals:
                    return hasNumeric && (!hasCore || inEntry) ? "6" : hasCore ? "R+" : string.Empty;
                case RawDeviceKeys.Key7:
                    return hasNumeric ? "7" : string.Empty;
                case RawDeviceKeys.Key8OrDecreaseFaults:
                    return hasNumeric && (!hasCore || inEntry) ? "8" : hasCore ? "F-" : string.Empty;
                case RawDeviceKeys.Key9OrIncreaseFaults:
                    return hasNumeric && (!hasCore || inEntry) ? "9" : hasCore ? "F+" : string.Empty;
                case RawDeviceKeys.EnterCurrentCompetitor:
                    return hasNumeric ? "Current" : string.Empty;
                case RawDeviceKeys.EnterNextCompetitor:
                    return hasNumeric ? "Next" : string.Empty;
                case RawDeviceKeys.Key0OrMuteSound:
                    return hasNumeric && (!hasCore || inEntry) ? "0" : hasCore ? "Mute" : string.Empty;
                case RawDeviceKeys.PassFinish:
                    return hasTimer ? "Finish" : string.Empty;
                case RawDeviceKeys.PassStart:
                    return hasTimer ? "Start" : string.Empty;
                case RawDeviceKeys.ResetRun:
                    return hasCore ? "Reset" : string.Empty;
                case RawDeviceKeys.Ready:
                    return hasCore ? "Ready" : string.Empty;
                default:
                    throw new InvalidOperationException($"No text available for key {key}.");
            }
        }

        private void FeatureCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            UpdateButtons();

            StatusChanged?.Invoke(this, EventArgs.Empty);
        }

        private void CompetitorCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            UpdateButtons();

            RawDeviceKeys keyChanged = sender == enterCurrentCompetitorCheckBox ? RawDeviceKeys.EnterCurrentCompetitor : RawDeviceKeys.EnterNextCompetitor;

            bool isKeyDownEvent = (sender == enterCurrentCompetitorCheckBox && enterCurrentCompetitorCheckBox.Checked) ||
                (sender == enterNextCompetitorCheckBox && enterNextCompetitorCheckBox.Checked);

            HandleKeyChange(keyChanged, isKeyDownEvent);
        }

        private void KeyButton_MouseDown(object sender, MouseEventArgs e)
        {
            // When user presses mouse button, leaves with Alt+Tab and releases the mouse button, then we do not 
            // receive a matching MouseUp event. Even when multiple mice are connected, only one WinForms button 
            // can be down at a time. To work around the missing MouseUp, we detect if any other buttons are down 
            // in addition to the current one. And if that's the case, we catch up by handling the MouseUp events 
            // we missed earlier before handling this MouseDown event.
            HandleMouseUpEventsPreviouslyMissed();

            var button = (ButtonBase)sender;
            Log.Debug($"MouseKeyDown on button {button.Text}");

            RawDeviceKeys key = GetKeyForButton(button);
            HandleKeyChange(key, true);
        }

        private void HandleMouseUpEventsPreviouslyMissed()
        {
            // @formatter:keep_existing_linebreaks true

            IEnumerable<RawDeviceKeys> rawKeys = Enum
                .GetValues(typeof(RawDeviceKeys))
                .Cast<RawDeviceKeys>()
                .Except(ModifierRawKeys)
                .Where(key => (key & keysDown) != RawDeviceKeys.None);

            // @formatter:keep_existing_linebreaks restore

            foreach (RawDeviceKeys rawKey in rawKeys)
            {
                HandleKeyChange(rawKey, false);
            }
        }

        private void KeyButton_MouseUp(object sender, MouseEventArgs e)
        {
            var button = (ButtonBase)sender;
            Log.Debug($"MouseKeyUp on button {button.Text}");

            RawDeviceKeys key = GetKeyForButton(button);
            HandleKeyChange(key, false);
        }

        private void HandleKeyChange(RawDeviceKeys keyChanged, bool isKeyDownEvent)
        {
            if (isKeyDownEvent)
            {
                keysDown |= keyChanged;
            }
            else
            {
                keysDown &= ~keyChanged;
            }

            KeysDownChanged?.Invoke(this, new EventArgs<RawDeviceKeys>(keysDown));
        }
    }
}
