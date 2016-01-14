using System;
using JetBrains.Annotations;

namespace DogAgilityCompetition.WinForms
{
    /// <summary>
    /// Turns an <see cref="Action" /> into an <see cref="IDisposable" /> that executes the action at the time the wrapper
    /// object gets disposed.
    /// </summary>
    public sealed class DisposableHolder : IDisposable
    {
        [NotNull]
        private readonly Action disposeAction;

        public DisposableHolder([NotNull] Action disposeAction)
        {
            this.disposeAction = disposeAction;
        }

        public void Dispose()
        {
            disposeAction();
        }
    }
}