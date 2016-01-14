using System;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol.Operations;
using DogAgilityCompetition.WinForms;
using JetBrains.Annotations;

namespace DogAgilityCompetition.MediatorEmulator.Engine
{
    /// <summary>
    /// Applies visualization changes on a WinForms form/control on the UI thread.
    /// </summary>
    public sealed class VisualizeOperationDispatcher
    {
        private static readonly TimeSpan CirceHiddenTime = TimeSpan.FromMilliseconds(999999);
        private const int CirceHiddenCompetitorNumber = 0;
        private const int CirceHiddenPlacement = 0;
        private const int CirceHiddenFaultsRefusals = 99;

        [NotNull]
        private readonly ISimpleVisualizationActor actor;

        [NotNull]
        private readonly Control invokeContext;

        [NotNull]
        public static VisualizeOperationDispatcher CreateFor<T>([NotNull] T source)
            where T : Control, ISimpleVisualizationActor
        {
            return new VisualizeOperationDispatcher(source, source);
        }

        private VisualizeOperationDispatcher([NotNull] ISimpleVisualizationActor actor, [NotNull] Control invokeContext)
        {
            Guard.NotNull(actor, nameof(actor));
            Guard.NotNull(invokeContext, nameof(invokeContext));

            this.actor = actor;
            this.invokeContext = invokeContext;
        }

        public void ClearAll()
        {
            actor.StopAndSetOrClearPrimaryTime(null);
            actor.SetOrClearFaultCount(null);
            actor.SetOrClearRefusalCount(null);
            actor.SetElimination(false);
            actor.SetOrClearCurrentCompetitorNumber(null);
            actor.SetOrClearNextCompetitorNumber(null);
            actor.SetOrClearPreviousCompetitorPlacement(null);
        }

        public void Dispatch([NotNull] VisualizeOperation operation)
        {
            Guard.NotNull(operation, nameof(operation));

            invokeContext.EnsureOnMainThread(() =>
            {
                // Important: Dispatch elimination change before handling time changes.
                DispatchEliminated(operation);
                DispatchCurrentCompetitorNumber(operation);
                DispatchNextCompetitorNumber(operation);

                // Important: Dispatch Timer Value before Start Timer (they should never occur both, but just in case).
                DispatchTimerValue(operation);
                DispatchStartTimer(operation);

                DispatchFaults(operation);
                DispatchRefusals(operation);
                DispatchPlacement(operation);
            });
        }

        private void DispatchEliminated([NotNull] VisualizeOperation operation)
        {
            if (operation.Eliminated != null)
            {
                actor.SetElimination(operation.Eliminated.Value);
            }
        }

        private void DispatchCurrentCompetitorNumber([NotNull] VisualizeOperation operation)
        {
            if (operation.CurrentCompetitorNumber != null)
            {
                int? number = operation.CurrentCompetitorNumber == CirceHiddenCompetitorNumber
                    ? null
                    : operation.CurrentCompetitorNumber;
                actor.SetOrClearCurrentCompetitorNumber(number);
            }
        }

        private void DispatchNextCompetitorNumber([NotNull] VisualizeOperation operation)
        {
            if (operation.NextCompetitorNumber != null)
            {
                int? number = operation.NextCompetitorNumber == CirceHiddenCompetitorNumber
                    ? null
                    : operation.NextCompetitorNumber;
                actor.SetOrClearNextCompetitorNumber(number);
            }
        }

        private void DispatchStartTimer([NotNull] VisualizeOperation operation)
        {
            if (operation.StartTimer)
            {
                actor.StartPrimaryTimer();
            }
        }

        private void DispatchTimerValue([NotNull] VisualizeOperation operation)
        {
            if (operation.TimerValue != null)
            {
                TimeSpan? time = operation.TimerValue == CirceHiddenTime ? null : operation.TimerValue;
                actor.StopAndSetOrClearPrimaryTime(time);
            }
        }

        private void DispatchFaults([NotNull] VisualizeOperation operation)
        {
            if (operation.FaultCount != null)
            {
                int? count = operation.FaultCount == CirceHiddenFaultsRefusals ? null : operation.FaultCount;
                actor.SetOrClearFaultCount(count);
            }
        }

        private void DispatchRefusals([NotNull] VisualizeOperation operation)
        {
            if (operation.RefusalCount != null)
            {
                int? count = operation.RefusalCount == CirceHiddenFaultsRefusals ? null : operation.RefusalCount;
                actor.SetOrClearRefusalCount(count);
            }
        }

        private void DispatchPlacement([NotNull] VisualizeOperation operation)
        {
            if (operation.PreviousPlacement != null)
            {
                int? placement = operation.PreviousPlacement == CirceHiddenPlacement
                    ? null
                    : operation.PreviousPlacement;
                actor.SetOrClearPreviousCompetitorPlacement(placement);
            }
        }
    }
}