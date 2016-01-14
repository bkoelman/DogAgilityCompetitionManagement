using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Visualization.Changes
{
    /// <summary>
    /// A visualization change that updates/clears the number of faults.
    /// </summary>
    public sealed class FaultCountUpdate : NullableVisualizationChange<int?>
    {
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