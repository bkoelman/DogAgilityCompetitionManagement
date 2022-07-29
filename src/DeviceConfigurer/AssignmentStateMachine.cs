using System.Reflection;
using System.Threading;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.DeviceConfigurer.Phases;

namespace DogAgilityCompetition.DeviceConfigurer;

/// <summary>
/// Enables conditional execution of a phase and waiting for a specific phase in the wireless network address assignment process.
/// </summary>
public sealed class AssignmentStateMachine
{
    private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod()!.DeclaringType!);

    private readonly object stateLock = new();

    private AssignmentPhase currentPhase;

    public AssignmentStateMachine(AssignmentPhase startPhase)
    {
        Guard.NotNull(startPhase, nameof(startPhase));

        currentPhase = startPhase;
    }

    public bool ExecuteIfInPhase<TPhase>(Func<TPhase, AssignmentPhase?> callback)
        where TPhase : AssignmentPhase
    {
        Guard.NotNull(callback, nameof(callback));

        using var lockTracker = new LockTracker(Log, GetDisplayNameFor<TPhase>(MethodBase.GetCurrentMethod()!));

        lock (stateLock)
        {
            lockTracker.Acquired();

            if (typeof(TPhase) == currentPhase.GetType())
            {
                AssignmentPhase? newPhase = callback((TPhase)currentPhase);

                if (newPhase != null)
                {
                    currentPhase = newPhase;
                    return true;
                }
            }
        }

        return false;
    }

    private static string GetDisplayNameFor<TPhase>(MethodBase source)
        where TPhase : AssignmentPhase
    {
        return $"{source.Name}<{typeof(TPhase).Name}>";
    }

    public TPhase WaitForPhase<TPhase>()
        where TPhase : AssignmentPhase
    {
        while (true)
        {
            using var lockTracker = new LockTracker(Log, GetDisplayNameFor<TPhase>(MethodBase.GetCurrentMethod()!));

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
