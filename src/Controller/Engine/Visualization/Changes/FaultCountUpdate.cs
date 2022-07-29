using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Controller.Engine.Visualization.Changes;

/// <summary>
/// A visualization change that updates/clears the number of faults.
/// </summary>
public sealed class FaultCountUpdate : NullableVisualizationChange<int?>
{
    public static FaultCountUpdate Hidden => new(null);
    public static FaultCountUpdate Zero => new(0);

    public FaultCountUpdate(int? value)
        : base(value)
    {
    }

    public override void ApplyTo(IVisualizationActor actor)
    {
        Guard.NotNull(actor, nameof(actor));
        actor.SetOrClearFaultCount(Value);
    }
}
