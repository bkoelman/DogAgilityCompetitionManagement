using System;
using System.Reflection;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine
{
    public sealed class EliminationTracker
    {
        [NotNull]
        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly int refusalStepSize;
        private readonly int eliminationThreshold;

        private bool hasMaximumCourseTimeElapsed; // Protected by stateLock
        private bool isManuallyEliminated; // Protected by stateLock
        private int refusalCount; // Protected by stateLock

        [NotNull]
        private readonly object stateLock = new object();

        private int MaxRefusalsValue => refusalStepSize * eliminationThreshold;

        public event EventHandler<EliminationEventArgs> EliminationChanged;
        public event EventHandler<EventArgs<int>> RefusalCountChanged;

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

        private bool UnsafeIsEliminated
            => isManuallyEliminated || refusalCount >= MaxRefusalsValue || hasMaximumCourseTimeElapsed;

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
                RaiseEventsOnChangeWithLock(() => { isManuallyEliminated = value; });
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

        public EliminationTracker(int refusalStepSize, int eliminationThreshold,
            [NotNull] CourseTimeTracker courseTimeTracker)
        {
            Guard.NotNull(courseTimeTracker, nameof(courseTimeTracker));

            this.refusalStepSize = refusalStepSize;
            this.eliminationThreshold = eliminationThreshold;

            courseTimeTracker.MaximumCourseTimeElapsed += (sender, args) => HandleMaximumCourseTimeElapsed();
        }

        private void HandleMaximumCourseTimeElapsed()
        {
            RaiseEventsOnChangeWithLock(() => { hasMaximumCourseTimeElapsed = true; });
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

        public void Reset()
        {
            // Note: Intentionally not raising any events here, because caller wants to 
            // combine multiple changes in single network packet (performance optimization).

            using (var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod()))
            {
                lock (stateLock)
                {
                    lockTracker.Acquired();

                    hasMaximumCourseTimeElapsed = false;
                    isManuallyEliminated = false;
                    refusalCount = 0;
                }
            }
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

                    argsForEliminationChanged = UnsafeIsEliminated != beforeIsEliminated
                        ? new EliminationEventArgs(UnsafeIsEliminated)
                        : null;
                    argsForRefusalCountChanged = refusalCount != beforeRefusalCount
                        ? new EventArgs<int>(refusalCount)
                        : null;
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