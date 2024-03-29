﻿using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol.Operations;
using DogAgilityCompetition.WinForms;

namespace DogAgilityCompetition.MediatorEmulator.Engine;

/// <summary>
/// Applies visualization changes on a WinForms form/control on the UI thread.
/// </summary>
public sealed class VisualizeOperationDispatcher
{
    private const int CirceHiddenCompetitorNumber = 0;
    private const int CirceHiddenPlacement = 0;
    private const int CirceHiddenFaultsRefusals = 99;
    private static readonly TimeSpan CirceHiddenTime = TimeSpan.FromMilliseconds(999999);

    private readonly ISimpleVisualizationActor actor;
    private readonly Control invokeContext;

    private VisualizeOperationDispatcher(ISimpleVisualizationActor actor, Control invokeContext)
    {
        Guard.NotNull(actor, nameof(actor));
        Guard.NotNull(invokeContext, nameof(invokeContext));

        this.actor = actor;
        this.invokeContext = invokeContext;
    }

    public static VisualizeOperationDispatcher CreateFor<T>(T source)
        where T : Control, ISimpleVisualizationActor
    {
        return new VisualizeOperationDispatcher(source, source);
    }

    public void ClearAll()
    {
        actor.StopAndSetOrClearPrimaryTime(null);
        actor.SetOrClearSecondaryTime(null);
        actor.SetOrClearFaultCount(null);
        actor.SetOrClearRefusalCount(null);
        actor.SetElimination(false);
        actor.SetOrClearCurrentCompetitorNumber(null);
        actor.SetOrClearNextCompetitorNumber(null);
        actor.SetOrClearPreviousCompetitorPlacement(null);
    }

    public void Dispatch(VisualizeOperation operation)
    {
        Guard.NotNull(operation, nameof(operation));

        invokeContext.EnsureOnMainThread(() =>
        {
            // Important: Dispatch Elimination Change before handling time changes.
            DispatchEliminated(operation);
            DispatchCurrentCompetitorNumber(operation);
            DispatchNextCompetitorNumber(operation);

            // Important: Dispatch Secondary Timer Value before Primary Timer Value (can occur when competitor has previous results before start of run)
            DispatchSecondaryTimerValue(operation);

            // Important: Dispatch Primary Timer Value before Start Timer (they should never occur both, but just in case).
            DispatchPrimaryTimerValue(operation);
            DispatchStartTimer(operation);

            DispatchFaults(operation);
            DispatchRefusals(operation);
            DispatchPlacement(operation);
        });
    }

    private void DispatchEliminated(VisualizeOperation operation)
    {
        if (operation.Eliminated != null)
        {
            actor.SetElimination(operation.Eliminated.Value);
        }
    }

    private void DispatchCurrentCompetitorNumber(VisualizeOperation operation)
    {
        if (operation.CurrentCompetitorNumber != null)
        {
            int? number = operation.CurrentCompetitorNumber == CirceHiddenCompetitorNumber ? null : operation.CurrentCompetitorNumber;
            actor.SetOrClearCurrentCompetitorNumber(number);
        }
    }

    private void DispatchNextCompetitorNumber(VisualizeOperation operation)
    {
        if (operation.NextCompetitorNumber != null)
        {
            int? number = operation.NextCompetitorNumber == CirceHiddenCompetitorNumber ? null : operation.NextCompetitorNumber;
            actor.SetOrClearNextCompetitorNumber(number);
        }
    }

    private void DispatchStartTimer(VisualizeOperation operation)
    {
        if (operation.StartTimer)
        {
            actor.StartPrimaryTimer();
        }
    }

    private void DispatchPrimaryTimerValue(VisualizeOperation operation)
    {
        if (operation.PrimaryTimerValue != null)
        {
            TimeSpan? time = operation.PrimaryTimerValue == CirceHiddenTime ? null : operation.PrimaryTimerValue;
            actor.StopAndSetOrClearPrimaryTime(time);
        }
    }

    private void DispatchSecondaryTimerValue(VisualizeOperation operation)
    {
        if (operation.SecondaryTimerValue != null)
        {
            TimeSpan? time = operation.SecondaryTimerValue == CirceHiddenTime ? null : operation.SecondaryTimerValue;
            actor.SetOrClearSecondaryTime(time);
        }
    }

    private void DispatchFaults(VisualizeOperation operation)
    {
        if (operation.FaultCount != null)
        {
            int? count = operation.FaultCount == CirceHiddenFaultsRefusals ? null : operation.FaultCount;
            actor.SetOrClearFaultCount(count);
        }
    }

    private void DispatchRefusals(VisualizeOperation operation)
    {
        if (operation.RefusalCount != null)
        {
            int? count = operation.RefusalCount == CirceHiddenFaultsRefusals ? null : operation.RefusalCount;
            actor.SetOrClearRefusalCount(count);
        }
    }

    private void DispatchPlacement(VisualizeOperation operation)
    {
        if (operation.PreviousPlacement != null)
        {
            int? placement = operation.PreviousPlacement == CirceHiddenPlacement ? null : operation.PreviousPlacement;
            actor.SetOrClearPreviousCompetitorPlacement(placement);
        }
    }
}
