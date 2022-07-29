using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Controller.Engine.Visualization;

/// <summary>
/// Provides a temporary context in which visualization changes are collected, which are then sent out as a set, in order to minimize the number of
/// outgoing network packets.
/// </summary>
public sealed class VisualizationUpdateCollector : IDisposable
{
    private readonly ICompetitionRunVisualizer source;

    private List<VisualizationChange> changeSet = new();

    public VisualizationUpdateCollector(ICompetitionRunVisualizer source)
    {
        Guard.NotNull(source, nameof(source));
        this.source = source;
    }

    public void Include(IEnumerable<VisualizationChange> changes)
    {
        Guard.NotNull(changes, nameof(changes));

        foreach (VisualizationChange change in changes)
        {
            Include(change);
        }
    }

    public void Include(VisualizationChange change)
    {
        Guard.NotNull(change, nameof(change));

        VisualizationChange? existing = changeSet.FirstOrDefault(c => c.GetType() == change.GetType());

        if (existing != null)
        {
            changeSet.Remove(existing);
        }

        changeSet.Add(change);
    }

    public void Dispose()
    {
        VisualizationConflictResolver.InspectChangeSet(changeSet);

        if (changeSet.Count > 0)
        {
            source.Apply(changeSet);
            changeSet = new List<VisualizationChange>();
        }
    }

    public static void Single(ICompetitionRunVisualizer visualizer, VisualizationChange change)
    {
        Guard.NotNull(visualizer, nameof(visualizer));
        Guard.NotNull(change, nameof(change));

        using var collector = new VisualizationUpdateCollector(visualizer);
        collector.Include(change);
    }
}
