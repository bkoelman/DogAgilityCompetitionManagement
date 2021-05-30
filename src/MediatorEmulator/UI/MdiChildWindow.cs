using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.MediatorEmulator.Engine.Storage;
using DogAgilityCompetition.WinForms;

namespace DogAgilityCompetition.MediatorEmulator.UI
{
    /// <summary>
    /// Represents the base class for MDI child forms.
    /// </summary>
    public static class MdiChildWindow
    {
        private const int MenuStripHeight = 24;
        private const int ExtraSpace = 5;

        public static void Register(FormWithWindowStateChangeEvent form, IWindowSettings settings, bool initiallyMaximized, ref IContainer? container)
        {
            Guard.NotNull(form, nameof(form));
            Guard.NotNull(settings, nameof(settings));

            if (!form.IsMdiChild)
            {
                throw new InvalidOperationException("Form must be an MDI child.");
            }

            // Justification for discard: Object registers itself to get disposed along with specified container on Form.
            _ = new DisposableComponent<TargetWindow>(new TargetWindow(form, settings, initiallyMaximized), ref container);
        }

        private sealed class TargetWindow : IDisposable
        {
            private readonly FormWithWindowStateChangeEvent form;
            private readonly IWindowSettings settings;

            public TargetWindow(FormWithWindowStateChangeEvent form, IWindowSettings settings, bool initiallyMaximized)
            {
                Guard.NotNull(form, nameof(form));
                Guard.NotNull(settings, nameof(settings));

                this.form = form;
                this.settings = settings;

                form.LocationChanged += FormOnLocationChanged;
                form.SizeChanged += FormOnSizeChanged;
                form.WindowStateChanged += FormOnWindowStateChanged;

                UpdateLayoutFromSettings(initiallyMaximized);
            }

            private void FormOnLocationChanged(object? sender, EventArgs eventArgs)
            {
                settings.WindowLocationX = form.WindowState == FormWindowState.Normal ? form.Location.X : form.RestoreBounds.Location.X;
                settings.WindowLocationY = form.WindowState == FormWindowState.Normal ? form.Location.Y : form.RestoreBounds.Location.Y;
            }

            private void FormOnSizeChanged(object? sender, EventArgs eventArgs)
            {
                settings.WindowHeight = form.WindowState == FormWindowState.Normal ? form.Size.Height : form.RestoreBounds.Height;
                settings.WindowWidth = form.WindowState == FormWindowState.Normal ? form.Size.Width : form.RestoreBounds.Width;
            }

            private void FormOnWindowStateChanged(object? sender, EventArgs eventArgs)
            {
                settings.IsMinimized = form.WindowState == FormWindowState.Minimized;
            }

            private void UpdateLayoutFromSettings(bool isMaximized)
            {
                if (settings.WindowWidth > 0 && settings.WindowHeight > 0)
                {
                    form.Size = new Size(settings.WindowWidth, settings.WindowHeight);
                }

                if (settings.WindowLocationX != null && settings.WindowLocationY != null)
                {
                    form.Location = new Point(settings.WindowLocationX.Value, settings.WindowLocationY.Value);
                }

                FitInMdiParent();

                if (settings.IsMinimized)
                {
                    form.WindowState = FormWindowState.Minimized;
                }
                else if (isMaximized)
                {
                    form.WindowState = FormWindowState.Maximized;
                }
            }

            private void FitInMdiParent()
            {
                if (form.WindowState == FormWindowState.Normal && form.MdiParent != null)
                {
                    if (form.Width > form.MdiParent.ClientSize.Width)
                    {
                        form.Width = form.MdiParent.ClientSize.Width - ExtraSpace;
                    }

                    if (form.Height > form.MdiParent.ClientSize.Height)
                    {
                        form.Height = form.MdiParent.ClientSize.Height - ExtraSpace - MenuStripHeight;
                    }

                    int? newX = null;
                    int? newY = null;

                    if (form.Location.X < 0 || form.Location.X + form.Size.Width > form.MdiParent.ClientSize.Width)
                    {
                        newX = Math.Max(0, form.MdiParent.ClientSize.Width - form.Size.Width - ExtraSpace);
                    }

                    if (form.Location.Y < 0 || form.Location.Y + form.Size.Height > form.MdiParent.ClientSize.Height)
                    {
                        newY = Math.Max(0, form.MdiParent.ClientSize.Height - form.Size.Height - ExtraSpace - MenuStripHeight);
                    }

                    if (newX != null || newY != null)
                    {
                        form.Location = new Point(newX ?? form.Location.X, newY ?? form.Location.Y);
                    }
                }
            }

            public void Dispose()
            {
                form.LocationChanged -= FormOnLocationChanged;
                form.SizeChanged -= FormOnSizeChanged;
                form.WindowStateChanged -= FormOnWindowStateChanged;
            }
        }
    }
}
