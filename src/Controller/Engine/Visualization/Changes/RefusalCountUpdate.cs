using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Visualization.Changes
{
    /// <summary>
    /// A visualization change that updates/clears the number of refusals.
    /// </summary>
    public sealed class RefusalCountUpdate : NullableVisualizationChange<int?>
    {
        [NotNull]
        public static RefusalCountUpdate Hidden => new RefusalCountUpdate(null);

        [NotNull]
        public static RefusalCountUpdate Zero => new RefusalCountUpdate(0);

        public RefusalCountUpdate([CanBeNull] int? value)
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