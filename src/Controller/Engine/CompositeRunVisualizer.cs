using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Visualization;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary>
    /// Delegates visualization actions to all of its children and logs them.
    /// </summary>
    public sealed class CompositeRunVisualizer : ICompetitionRunVisualizer
    {
        [NotNull]
        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [NotNull]
        [ItemNotNull]
        private readonly IEnumerable<ICompetitionRunVisualizer> children;

        public CompositeRunVisualizer([NotNull] [ItemNotNull] IEnumerable<ICompetitionRunVisualizer> children)
        {
            Guard.NotNull(children, nameof(children));
            this.children = children;
        }

        public void Apply(IReadOnlyCollection<VisualizationChange> changes)
        {
            LogChanges(changes);

            foreach (ICompetitionRunVisualizer child in children)
            {
                child.Apply(changes);
            }
        }

        private static void LogChanges([NotNull] [ItemNotNull] IEnumerable<VisualizationChange> changes)
        {
            var writer = new StringWriter();
            foreach (VisualizationChange change in changes)
            {
                writer.WriteLine();
                writer.Write("\t");
                change.WriteTo(writer);
            }
            Log.Debug("Applying set of visualization changes: " + writer);
        }
    }
}