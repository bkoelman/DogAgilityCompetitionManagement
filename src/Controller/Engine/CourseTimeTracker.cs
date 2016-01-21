using System;
using System.Threading;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine
{
    public sealed class CourseTimeTracker : IDisposable
    {
        private static readonly TimeSpan InfiniteTime = TimeSpan.FromMilliseconds(-1);

        [CanBeNull]
        private Timer standardCourseTimeTimer; // Protected by stateLock

        [CanBeNull]
        private Timer maximumCourseTimeTimer; // Protected by stateLock

        [NotNull]
        private readonly object stateLock = new object();

        public event EventHandler StandardCourseTimeElapsed = delegate { };
        public event EventHandler MaximumCourseTimeElapsed = delegate { };

        public void StartMonitorCourseTime([CanBeNull] TimeSpan? standardCourseTime,
            [CanBeNull] TimeSpan? maximumCourseTime)
        {
            lock (stateLock)
            {
                if (standardCourseTimeTimer != null || maximumCourseTimeTimer != null)
                {
                    throw new InvalidOperationException("Already started.");
                }

                if (standardCourseTime != null)
                {
                    standardCourseTimeTimer = new Timer(state => CourseTimeTimerTick(true), null,
                        standardCourseTime.Value, InfiniteTime);
                }

                if (maximumCourseTime != null)
                {
                    maximumCourseTimeTimer = new Timer(state => CourseTimeTimerTick(false), null,
                        maximumCourseTime.Value, InfiniteTime);
                }
            }
        }

        private void CourseTimeTimerTick(bool isStandardCourseTime)
        {
            if (isStandardCourseTime)
            {
                StandardCourseTimeElapsed(this, EventArgs.Empty);
            }
            else
            {
                MaximumCourseTimeElapsed(this, EventArgs.Empty);
            }
        }

        public void StopMonitorCourseTime()
        {
            lock (stateLock)
            {
                if (standardCourseTimeTimer != null)
                {
                    standardCourseTimeTimer.Dispose();
                    standardCourseTimeTimer = null;
                }

                if (maximumCourseTimeTimer != null)
                {
                    maximumCourseTimeTimer.Dispose();
                    maximumCourseTimeTimer = null;
                }
            }
        }

        public void Dispose()
        {
            StopMonitorCourseTime();
        }
    }
}