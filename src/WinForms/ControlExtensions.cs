using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.WinForms
{
    /// <summary />
    public static class ControlExtensions
    {
        public static void EnsureOnMainThread([NotNull] this Control control, [NotNull] Action action)
        {
            Guard.NotNull(control, nameof(control));
            Guard.NotNull(action, nameof(action));

            // Critical: do not check whether Invoke is required, because it can change the order of execution.

            if (!control.IsDisposed)
            {
                control.BeginInvoke(new MethodInvoker(action));
            }
        }

        [NotNull]
        [ItemNotNull]
        public static IEnumerable<Control> GetAllChildControlsRecursive([NotNull] this Control control)
        {
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
