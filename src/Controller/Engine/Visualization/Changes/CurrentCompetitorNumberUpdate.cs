using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Controller.Engine.Visualization.Changes;

/// <summary>
/// A visualization change that changes displayed current competitor number.
/// </summary>
public sealed class CurrentCompetitorNumberUpdate : NotNullableVisualizationChange<int>
{
    public CurrentCompetitorNumberUpdate(int value)
        : base(value)
    {
    }

    public override void ApplyTo(IVisualizationActor actor)
    {
        Guard.NotNull(actor, nameof(actor));
        actor.SetCurrentCompetitorNumber(Value);
    }
}
