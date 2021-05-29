using System;
using System.Drawing;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Forms
{
    /// <summary>
    /// Transparent full-screen overlay to display an animation on a large screen.
    /// </summary>
    public sealed partial class NotifyPictureForm : Form
    {
        private const double MinOpacity = 0.0;
        private const double MaxOpacity = 0.9;
        private const double OpacityStepSize = 0.05;
        private static readonly TimeSpan HoldAtMaxOpacityDuration = TimeSpan.FromSeconds(2);

        private bool directionIsUp;
        private bool isCompleted;

        [CanBeNull]
        private DateTime? holdStartedAt;

        public event EventHandler AnimationCompleted;

        public NotifyPictureForm()
        {
            InitializeComponent();
        }

        private void NotifyPictureForm_Load([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            Opacity = MinOpacity;
            directionIsUp = true;
            holdStartedAt = null;
            isCompleted = false;
            fadeTimer.Enabled = true;
        }

        public void ShowAnimated([NotNull] Control parentForm, [NotNull] Bitmap picture)
        {
            Guard.NotNull(parentForm, nameof(parentForm));
            Guard.NotNull(picture, nameof(picture));

            pictureBox.Image = picture;

            DisplayOverParent(parentForm);
            Show(parentForm);
        }

        public void Cancel()
        {
            if (!isCompleted)
            {
                // Immediately fade out now, unless already fading out.
                if (directionIsUp)
                {
                    holdStartedAt = SystemContext.UtcNow().AddHours(-1);
                }
            }
        }

        private void DisplayOverParent([NotNull] Control parentForm)
        {
            Screen currentScreen = Screen.FromControl(parentForm);
            StartPosition = FormStartPosition.Manual;
            Location = currentScreen.Bounds.Location;

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
        }

        private void NotifyPictureForm_FormClosing([CanBeNull] object sender, [NotNull] FormClosingEventArgs e)
        {
            if (!isCompleted && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
            }
            else
            {
                AnimationCompleted?.Invoke(this, EventArgs.Empty);
            }
        }

        private void FadeTimer_Tick([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            if (holdStartedAt != null)
            {
                TimeSpan elapsed = SystemContext.UtcNow() - holdStartedAt.Value;

                if (elapsed >= HoldAtMaxOpacityDuration)
                {
                    holdStartedAt = null;
                    directionIsUp = false;
                }

                return;
            }

            if (directionIsUp)
            {
                Opacity += OpacityStepSize;

                if (Opacity >= MaxOpacity)
                {
                    holdStartedAt = SystemContext.UtcNow();
                }
            }
            else
            {
                Opacity -= OpacityStepSize;

                if (Opacity <= MinOpacity)
                {
                    fadeTimer.Enabled = false;
                    isCompleted = true;
                    Close();
                }
            }
        }
    }
}
