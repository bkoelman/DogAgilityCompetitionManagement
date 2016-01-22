using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Visualization.Changes
{
    /// <summary>
    /// A visualization change that updates/clears the number of faults.
    /// </summary>
    public sealed class FaultCountUpdate : NullableVisualizationChange<int?>
    {
        [NotNull]
        public static FaultCountUpdate Hidden => new FaultCountUpdate(null);

        [NotNull]
        public static FaultCountUpdate Zero => new FaultCountUpdate(0);

        public FaultCountUpdate([CanBeNull] int? value)
            : base(value)
        {
        }

        public override void ApplyTo(IVisualizationActor actor)
        {
            Guard.NotNull(actor, nameof(actor));
            actor.SetOrClearFaultCount(Value);
        }
    }
}