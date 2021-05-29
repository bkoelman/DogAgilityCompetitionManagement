using System.Collections.Generic;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Visualization
{
    /// <summary>
    /// Defines the contract to visualize a set of changes on a user interface.
    /// </summary>
    public interface ICompetitionRunVisualizer
    {
        void Apply([NotNull] [ItemNotNull] IReadOnlyCollection<VisualizationChange> changes);
    }
}
