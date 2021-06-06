using System.IO;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Visualization
{
    /// <summary>
    /// Represents the base class for all visualization changes.
    /// </summary>
    public abstract class VisualizationChange
    {
        public abstract void ApplyTo(IVisualizationActor actor);

        public void WriteTo(TextWriter writer)
        {
            Guard.NotNull(writer, nameof(writer));

            writer.Write(ToString());
        }

        [Pure]
        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
