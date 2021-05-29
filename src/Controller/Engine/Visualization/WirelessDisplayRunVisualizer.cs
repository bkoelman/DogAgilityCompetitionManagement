using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Controller;
using DogAgilityCompetition.Circe.Protocol;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Visualization
{
    /// <summary>
    /// Collects all visualization changes into a single Circe operation and transmits it to the displays in the logical network.
    /// </summary>
    public sealed class WirelessDisplayRunVisualizer : ICompetitionRunVisualizer
    {
        [NotNull]
        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [CanBeNull]
        private CirceControllerSessionManager circeSessionManager;

        [NotNull]
        [ItemNotNull]
        private IReadOnlyList<WirelessNetworkAddress> displaysInRunComposition = new List<WirelessNetworkAddress>();

        public void InitializeRun([NotNull] CirceControllerSessionManager sessionManager, [NotNull] NetworkComposition runComposition)
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

        private void ApplyFieldSet([NotNull] CirceControllerSessionManager sessionManager, VisualizeFieldSet fieldSet)
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
                });
            }
        }

        private void AssertPreconditions([NotNull] Action<CirceControllerSessionManager> action)
        {
            CirceControllerSessionManager sessionManager = AssertSessionNotNull();

            if (displaysInRunComposition.Any())
            {
                action(sessionManager);
            }
        }

        [AssertionMethod]
        [NotNull]
        private CirceControllerSessionManager AssertSessionNotNull()
        {
            return Assertions.InternalValueIsNotNull(() => circeSessionManager, () => circeSessionManager);
        }
    }
}
