using System;
using System.Reflection;
using System.Threading;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.DeviceConfigurer.Phases;
using JetBrains.Annotations;

namespace DogAgilityCompetition.DeviceConfigurer
{
    /// <summary>
    /// Enables conditional execution of a phase and waiting for a specific phase in the wireless network address assignment process.
    /// </summary>
    public sealed class AssignmentStateMachine
    {
        [NotNull]
        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [NotNull]
        private readonly object stateLock = new();

        [NotNull]
        private AssignmentPhase currentPhase;

        public AssignmentStateMachine([NotNull] AssignmentPhase startPhase)
        {
            Guard.NotNull(startPhase, nameof(startPhase));

            currentPhase = startPhase;
        }

        public bool ExecuteIfInPhase<TPhase>([NotNull] Func<TPhase, AssignmentPhase> callback)
            where TPhase : AssignmentPhase
        {
            Guard.NotNull(callback, nameof(callback));

            using (var lockTracker = new LockTracker(Log, GetDisplayNameFor<TPhase>(MethodBase.GetCurrentMethod())))
            {
                lock (stateLock)
                {
                    lockTracker.Acquired();

                    if (typeof(TPhase) == currentPhase.GetType())
                    {
                        AssignmentPhase newPhase = callback((TPhase)currentPhase);

                        if (newPhase != null)
                        {
                            currentPhase = newPhase;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        [NotNull]
        private static string GetDisplayNameFor<TPhase>([NotNull] MethodBase source)
            where TPhase : AssignmentPhase
        {
            return source.Name + "<" + typeof(TPhase).Name + ">";
        }

        [NotNull]
        public TPhase WaitForPhase<TPhase>()
            where TPhase : AssignmentPhase
        {
            while (true)
            {
                using (var lockTracker = new LockTracker(Log, GetDisplayNameFor<TPhase>(MethodBase.GetCurrentMethod())))
                {
                    lock (stateLock)
                    {
                        lockTracker.Acquired();

                        if (typeof(TPhase) == currentPhase.GetType())
                        {
                            return (TPhase)currentPhase;
                        }

                        Thread.Sleep(250);
                    }
                }
            }
        }
    }
}
