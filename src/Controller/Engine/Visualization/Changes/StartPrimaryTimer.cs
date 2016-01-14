using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Controller.Engine.Visualization.Changes
{
    /// <summary>
    /// A visualization change that resets the primary timer to zero and starts it.
    /// </summary>
    public sealed class StartPrimaryTimer : VisualizationChange
    {
        public override void ApplyTo(IVisualizationActor actor)
        {
            Guard.NotNull(actor, nameof(actor));
            actor.StartPrimaryTimer();
        }
    }
}