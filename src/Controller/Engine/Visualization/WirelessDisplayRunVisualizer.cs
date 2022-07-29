using System.Reflection;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Controller;
using DogAgilityCompetition.Circe.Protocol;

namespace DogAgilityCompetition.Controller.Engine.Visualization;

/// <summary>
/// Collects all visualization changes into a single Circe operation and transmits it to the displays in the logical network.
/// </summary>
public sealed class WirelessDisplayRunVisualizer : ICompetitionRunVisualizer
{
    private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod()!.DeclaringType!);

    private CirceControllerSessionManager? circeSessionManager;
    private IReadOnlyList<WirelessNetworkAddress> displaysInRunComposition = new List<WirelessNetworkAddress>();

    public void InitializeRun(CirceControllerSessionManager sessionManager, NetworkComposition runComposition)
    {
        Guard.NotNull(sessionManager, nameof(sessionManager));
        Guard.NotNull(runComposition, nameof(runComposition));

        circeSessionManager = sessionManager;
        displaysInRunComposition = runComposition.GetDevicesInAnyRole(DeviceRoles.Display).ToList();
    }

    void ICompetitionRunVisualizer.Apply(IReadOnlyCollection<VisualizationChange> changes)
    {
        AssertPreconditions(sessionManager =>
        {
            var updateCollector = new WirelessDisplayUpdateCollector();

            foreach (VisualizationChange change in changes)
            {
                change.ApplyTo(updateCollector);
            }

            VisualizeFieldSet fieldSet = updateCollector.GetResult();

            if (!fieldSet.IsEmpty)
            {
                ApplyFieldSet(sessionManager, fieldSet);
            }
        });
    }

    private void ApplyFieldSet(CirceControllerSessionManager sessionManager, VisualizeFieldSet fieldSet)
    {
        // Due to limited size of hardware buffers, we currently send the operation for each device individually.
        foreach (WirelessNetworkAddress display in displaysInRunComposition)
        {
            Task task = sessionManager.VisualizeAsync(new[]
            {
                display
            }, fieldSet);

            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Log.Warn($"Failed to send display update operation from controller: {fieldSet}.", t.Exception);
                }
            }, TaskScheduler.Default);
        }
    }

    private void AssertPreconditions(Action<CirceControllerSessionManager> action)
    {
        Assertions.IsNotNull(circeSessionManager, nameof(circeSessionManager));

        if (displaysInRunComposition.Any())
        {
            action(circeSessionManager);
        }
    }
}
