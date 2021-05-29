using System.Drawing;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Visualization.Changes
{
    /// <summary>
    /// A visualization change that fades in, displays, then fades out a picture.
    /// </summary>
    public sealed class StartAnimation : NotNullableVisualizationChange<Bitmap>
    {
        public StartAnimation([NotNull] Bitmap value)
            : base(value)
        {
        }

        public override void ApplyTo(IVisualizationActor actor)
        {
            Guard.NotNull(actor, nameof(actor));
            actor.StartAnimation(Value);
        }
    }
}
