using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Controller.Engine.Visualization.Changes;

/// <summary>
/// A visualization change that updates clock synchronization status.
/// </summary>
public sealed class ClockSynchronizationUpdate : NotNullableVisualizationChange<ClockSynchronizationMode>
{
    public ClockSynchronizationUpdate(ClockSynchronizationMode value)
        : base(value)
    {
    }

    public override void ApplyTo(IVisualizationActor actor)
    {
        Guard.NotNull(actor, nameof(actor));
        actor.SetClockSynchronizationMode(Value);
    }
}
