using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Visualization.Changes;

namespace DogAgilityCompetition.Controller.Engine.Visualization;

/// <summary>
/// Resolves conflicts in an outgoing set of visualization changes, according to CIRCE rules.
/// </summary>
public static class VisualizationConflictResolver
{
    public static void InspectChangeSet(IList<VisualizationChange> changeSet)
    {
        Guard.NotNull(changeSet, nameof(changeSet));

        ResolvePrimaryTimePrecedence(changeSet);
        ResolveEliminationPrecedence(changeSet);
    }

    private static void ResolvePrimaryTimePrecedence(ICollection<VisualizationChange> changeSet)
    {
        StartPrimaryTimer? startPrimaryTimer = changeSet.OfType<StartPrimaryTimer>().FirstOrDefault();
        PrimaryTimeStopAndSet? setPrimaryValue = changeSet.OfType<PrimaryTimeStopAndSet>().FirstOrDefault();

        if (startPrimaryTimer != null && setPrimaryValue != null)
        {
            changeSet.Remove(setPrimaryValue);
        }
    }

    private static void ResolveEliminationPrecedence(IList<VisualizationChange> changeSet)
    {
        EliminationUpdate? eliminationUpdate = changeSet.OfType<EliminationUpdate>().FirstOrDefault();

        if (eliminationUpdate != null)
        {
            changeSet.Remove(eliminationUpdate);
            changeSet.Insert(0, eliminationUpdate);
        }
    }
}
