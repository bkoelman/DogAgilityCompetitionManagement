using System;
using System.Reflection;
using System.Threading;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine
{
    public sealed class EliminationTracker : IDisposable
    {
        [NotNull]
        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly TimeSpan InfiniteTime = TimeSpan.FromMilliseconds(-1);

        private readonly int refusalStepSize;
        private readonly int eliminationThreshold;

        [NotNull]
        private readonly Timer maximumCourseTimeTimer;

        [NotNull]
        private readonly object stateLock = new();

        private bool maximumCourseTimeElapsed; // Protected by stateLock

        private bool isManuallyEliminated; // Protected by stateLock

        private int refusalCount; // Protected by stateLock

        private int MaxRefusalsValue => refusalStepSize * eliminationThreshold;

        private bool UnsafeIsEliminated => isManuallyEliminated || refusalCount >= MaxRefusalsValue || maximumCourseTimeElapsed;

        public bool IsEliminated
        {
            get
            {
                using (var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod()))
                {
                    lock (stateLock)
                    {
                        lockTracker.Acquired();

                        return UnsafeIsEliminated;
                    }
                }
            }
        }

        public bool IsManuallyEliminated
        {
            get
            {
                using (var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod()))
                {
                    lock (stateLock)
                    {
                        lockTracker.Acquired();

                        return isManuallyEliminated;
                    }
                }
            }
            set
            {
                RaiseEventsOnChangeWithLock(() =>
                {
                    isManuallyEliminated = value;
                });
            }
        }

        public int RefusalCount
        {
            get
            {
                using (var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod()))
                {
                    lock (stateLock)
                    {
                        lockTracker.Acquired();

                        return refusalCount;
                    }
                }
            }
        }

        public event EventHandler<EliminationEventArgs> EliminationChanged;
        public event EventHandler<EventArgs<int>> RefusalCountChanged;

        public EliminationTracker(int refusalStepSize, int eliminationThreshold)
        {
            this.refusalStepSize = refusalStepSize;
            this.eliminationThreshold = eliminationThreshold;

            maximumCourseTimeTimer = new Timer(_ => MaximumCourseTimeTimerTick());
        }

        private void MaximumCourseTimeTimerTick()
        {
            RaiseEventsOnChangeWithLock(() =>
            {
                maximumCourseTimeElapsed = true;
            });
        }

        public void IncreaseRefusals()
        {
            RaiseEventsOnChangeWithLock(() =>
            {
                if (refusalCount < MaxRefusalsValue)
                {
                    refusalCount += refusalStepSize;
                }
            });
        }

        public void DecreaseRefusals()
        {
            RaiseEventsOnChangeWithLock(() =>
            {
                if (refusalCount > 0)
                {
                    refusalCount -= refusalStepSize;
                }
            });
        }

        public void StartMonitorCourseTime([CanBeNull] TimeSpan? maximumCourseTime)
        {
            if (maximumCourseTime != null)
            {
                maximumCourseTimeTimer.Change(maximumCourseTime.Value, InfiniteTime);
            }
        }

        public void StopMonitorCourseTime()
        {
            maximumCourseTimeTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public void Reset()
        {
            // Note: Intentionally not raising any events here, because caller wants to 
            // combine multiple changes in single network packet (performance optimization).

            StopMonitorCourseTime();

            using (var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod()))
            {
                lock (stateLock)
                {
                    lockTracker.Acquired();

                    maximumCourseTimeElapsed = false;
                    isManuallyEliminated = false;
                    refusalCount = 0;
                }
            }
        }

        public void Dispose()
        {
            maximumCourseTimeTimer.Dispose();
        }

        private void RaiseEventsOnChangeWithLock([NotNull] Action action)
        {
            EliminationEventArgs argsForEliminationChanged;
            EventArgs<int> argsForRefusalCountChanged;

            using (var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod()))
            {
                lock (stateLock)
                {
                    lockTracker.Acquired();

                    bool beforeIsEliminated = UnsafeIsEliminated;
                    int beforeRefusalCount = refusalCount;

                    action();

                    argsForEliminationChanged = UnsafeIsEliminated != beforeIsEliminated ? new EliminationEventArgs(UnsafeIsEliminated) : null;
                    argsForRefusalCountChanged = refusalCount != beforeRefusalCount ? new EventArgs<int>(refusalCount) : null;
                }
            }

            if (argsForEliminationChanged != null)
            {
                EliminationChanged?.Invoke(this, argsForEliminationChanged);
            }

            if (argsForRefusalCountChanged != null)
            {
                RefusalCountChanged?.Invoke(this, argsForRefusalCountChanged);
            }
        }
    }
}
