using JetBrains.Annotations;

namespace DogAgilityCompetition.Specs.Facilities
{
    /// <summary>
    /// Supports writing unit tests that utilize the USB loop-back cable.
    /// </summary>
    public sealed class CirceUsbLoopbackTestRunner<TResult> : CirceUsbLoopbackTestRunner
    {
        [CanBeNull]
        public TResult Result { get; private set; }

        public void SignalSucceeded([NotNull] TResult result)
        {
            Result = result;
            SignalSucceeded();
        }
    }
}
