using System.Collections.Generic;
using System.Linq;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Visualization.Changes;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Visualization
{
    /// <summary>
    /// Resolves conflicts in an outgoing set of visualization changes, according to CIRCE rules.
    /// </summary>
    public static class VisualizationConflictResolver
    {
        public static void InspectChangeSet([NotNull] [ItemNotNull] IList<VisualizationChange> changeSet)
        {
            Guard.NotNull(changeSet, nameof(changeSet));

            ResolveTimePrecedence(changeSet);
            ResolveEliminationPrecedence(changeSet);
        }

        private static void ResolveTimePrecedence([NotNull] [ItemNotNull] ICollection<VisualizationChange> changeSet)
        {
            StartPrimaryTimer startPrimaryTimer = changeSet.OfType<StartPrimaryTimer>().FirstOrDefault();
            PrimaryTimeStopAndSet setPrimaryValue = changeSet.OfType<PrimaryTimeStopAndSet>().FirstOrDefault();

            if (startPrimaryTimer != null && setPrimaryValue != null)
            {
                changeSet.Remove(setPrimaryValue);
            }
        }

        private static void ResolveEliminationPrecedence([NotNull] [ItemNotNull] IList<VisualizationChange> changeSet)
        {
            EliminationUpdate eliminationUpdate = changeSet.OfType<EliminationUpdate>().FirstOrDefault();
            if (eliminationUpdate != null)
            {
                changeSet.Remove(eliminationUpdate);
                changeSet.Insert(0, eliminationUpdate);
            }
        }
    }
}