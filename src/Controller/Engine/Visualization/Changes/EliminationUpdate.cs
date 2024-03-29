using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Controller.Engine.Visualization.Changes;

/// <summary>
/// A visualization change that turns elimination on/off.
/// </summary>
public sealed class EliminationUpdate : NotNullableVisualizationChange<bool>
{
    public static EliminationUpdate Off => new(false);

    public EliminationUpdate(bool value)
        : base(value)
    {
    }

    public override void ApplyTo(IVisualizationActor actor)
    {
        Guard.NotNull(actor, nameof(actor));
        actor.SetElimination(Value);
    }
}
