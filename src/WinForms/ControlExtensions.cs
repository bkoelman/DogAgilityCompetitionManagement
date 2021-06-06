using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.WinForms
{
    /// <summary />
    public static class ControlExtensions
    {
        public static void EnsureOnMainThread(this Control control, Action action)
        {
            Guard.NotNull(control, nameof(control));
            Guard.NotNull(action, nameof(action));

            // Critical: do not check whether Invoke is required, because it can change the order of execution.

            if (!control.IsDisposed && control.IsHandleCreated)
            {
                control.BeginInvoke(new MethodInvoker(action));
            }
        }

        public static IEnumerable<Control> GetAllChildControlsRecursive(this Control control)
        {
            Guard.NotNull(control, nameof(control));

            foreach (Control child in control.Controls)
            {
                yield return child;

                foreach (Control next in GetAllChildControlsRecursive(child))
                {
                    yield return next;
                }
            }
        }
    }
}
