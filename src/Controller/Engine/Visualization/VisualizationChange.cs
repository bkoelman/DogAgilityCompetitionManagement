using System.IO;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Visualization
{
    /// <summary>
    /// Represents the base class for all visualization changes.
    /// </summary>
    public abstract class VisualizationChange
    {
        public abstract void ApplyTo([NotNull] IVisualizationActor actor);

        public void WriteTo([NotNull] TextWriter writer)
        {
            writer.Write(ToString());
        }

        [Pure]
        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
