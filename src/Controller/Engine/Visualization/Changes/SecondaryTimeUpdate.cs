using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Visualization.Changes;

/// <summary>
/// A visualization change that sets/clears the secondary (intermediate) time value.
/// </summary>
public sealed class SecondaryTimeUpdate : NullableVisualizationChange<TimeSpan?>
{
    public static SecondaryTimeUpdate Hidden => new(null, false);

    public bool DoBlink { get; }

    public SecondaryTimeUpdate(TimeSpan? value, bool doBlink)
        : base(value)
    {
        DoBlink = doBlink;
    }

    public override void ApplyTo(IVisualizationActor actor)
    {
        Guard.NotNull(actor, nameof(actor));
        actor.SetOrClearSecondaryTime(Value, DoBlink);
    }

    [Pure]
    public override string ToString()
    {
        return $"{GetType().Name} Value: {Value?.ToString() ?? "(null)"}, DoBlink: {DoBlink}";
    }

    public static SecondaryTimeUpdate FromTimeSpanWithAccuracy(TimeSpanWithAccuracy? value, bool doBlink)
    {
        return new SecondaryTimeUpdate(value?.TimeValue, doBlink);
    }
}
