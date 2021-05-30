using System;

namespace DogAgilityCompetition.WinForms
{
    /// <summary>
    /// Turns an <see cref="Action" /> into an <see cref="IDisposable" /> that executes the action at the time the wrapper object gets disposed.
    /// </summary>
    public sealed class DisposableHolder : IDisposable
    {
        private readonly Action disposeAction;

        public DisposableHolder(Action disposeAction)
        {
            this.disposeAction = disposeAction;
        }

        public void Dispose()
        {
            disposeAction();
        }
    }
}
