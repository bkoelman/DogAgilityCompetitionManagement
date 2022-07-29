using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Storage;

namespace DogAgilityCompetition.Controller.Engine.Visualization.Changes;

/// <summary>
/// A visualization change that updates/clears class information.
/// </summary>
public sealed class ClassInfoUpdate : NullableVisualizationChange<CompetitionClassInfo>
{
    public static ClassInfoUpdate Hidden => new(null);

    public ClassInfoUpdate(CompetitionClassInfo? value)
        : base(value)
    {
    }

    public override void ApplyTo(IVisualizationActor actor)
    {
        Guard.NotNull(actor, nameof(actor));
        actor.SetClass(Value);
    }
}
