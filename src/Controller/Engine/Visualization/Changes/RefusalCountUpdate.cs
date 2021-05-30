using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Controller.Engine.Visualization.Changes
{
    /// <summary>
    /// A visualization change that updates/clears the number of refusals.
    /// </summary>
    public sealed class RefusalCountUpdate : NullableVisualizationChange<int?>
    {
        public static RefusalCountUpdate Hidden => new(null);
        public static RefusalCountUpdate Zero => new(0);

        public RefusalCountUpdate(int? value)
            : base(value)
        {
        }

        public override void ApplyTo(IVisualizationActor actor)
        {
            Guard.NotNull(actor, nameof(actor));
            actor.SetOrClearRefusalCount(Value);
        }
    }
}
