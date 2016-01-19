using System;
using System.ComponentModel;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace DogAgilityCompetition.WinForms
{
    /// <summary>
    /// A <see cref="Form" /> that notifies about changes in window state (minimize, maximize and restore).
    /// </summary>
    public class FormWithWindowStateChangeEvent : FormWithHandleManagement
    {
        [Category("Property Changed")]
        [Description("Event raised before the value of WindowState property is changed on Form.")]
        public event EventHandler<WindowStateChangingEventArgs> WindowStateChanging;

        [Category("Property Changed")]
        [Description("Event raised after the value of WindowState property is changed on Form.")]
        public event EventHandler WindowStateChanged;

        protected override void WndProc(ref Message m)
        {
            FormWindowState? newWindowState = null;

            if (m.Msg == WinUserConstants.WmSysCommand)
            {
                int wParam = m.WParam.ToInt32() & 0xFFF0;
                if (wParam == WinUserConstants.ScMinimize || wParam == WinUserConstants.ScMaximize ||
                    wParam == WinUserConstants.ScRestore)
                {
                    newWindowState = TranslateWindowState(wParam);
                }
            }

            if (newWindowState != null)
            {
                EventHandler<WindowStateChangingEventArgs> eventHandler = WindowStateChanging;
                if (eventHandler != null)
                {
                    var args = new WindowStateChangingEventArgs(newWindowState.Value);
                    eventHandler(this, args);

                    if (args.Cancel)
                    {
                        return;
                    }
                }
            }

            base.WndProc(ref m);

            if (newWindowState != null)
            {
                WindowStateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [CanBeNull]
        private static FormWindowState? TranslateWindowState(int wParam)
        {
            switch (wParam)
            {
                case WinUserConstants.ScMinimize:
                    return FormWindowState.Minimized;
                case WinUserConstants.ScMaximize:
                    return FormWindowState.Maximized;
                case WinUserConstants.ScRestore:
                    return FormWindowState.Normal;
                default:
                    return null;
            }
        }

        private static class WinUserConstants
        {
            public const int WmSysCommand = 0x0112;

            public const int ScMinimize = 0xF020;
            public const int ScMaximize = 0xF030;
            public const int ScRestore = 0xF120;
        }
    }
}