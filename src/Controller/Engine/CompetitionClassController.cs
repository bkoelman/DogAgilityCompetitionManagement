using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Controller;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Controller.Engine.Storage;
using DogAgilityCompetition.Controller.Engine.Visualization;
using DogAgilityCompetition.Controller.Engine.Visualization.Changes;
using DogAgilityCompetition.Controller.Properties;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary>
    /// Controls the flow of activities and state during the competition runs in a class.
    /// </summary>
    public sealed class CompetitionClassController : IDisposable
    {
        // This class raises events inside an exclusive lock. Although doing so is generally not recommended (due to
        // potential deadlocks because event subscribers can do anything), in this application the event subscribers 
        // only asynchronously update UI-elements or enqueue outgoing packets, so it's not an actual problem here.

        [NotNull]
        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [NotNull]
        private static readonly ICollection<CompetitionClassState> AllStates = StatesInRange(CompetitionClassState.Offline, CompetitionClassState.RunCompleted);

        [NotNull]
        private readonly object stateLock = new();

        [NotNull]
        private readonly CompetitionRunData runData = new();

        [NotNull]
        private readonly Func<NetworkHealthReport> networkHealthNeededCallback;

        [NotNull]
        private readonly ClockSynchronizationMonitor synchronizationMonitor;

        [NotNull]
        private readonly ICompetitionRunVisualizer visualizer;

        private CompetitionClassState classState = CompetitionClassState.Offline;

        [CanBeNull]
        private CompetitionClassRequirements classRequirements;

        [NotNull]
        private CompetitionClassModel modelSnapshot;

        [CanBeNull]
        private int? currentCompetitorNumber;

        [CanBeNull]
        private int? nextCompetitorNumberTyped;

        [CanBeNull]
        private int? nextCompetitorNumberGenerated;

        [CanBeNull]
        private int? NextCompetitorNumber => nextCompetitorNumberTyped ?? nextCompetitorNumberGenerated;

        public bool CanStartClass
        {
            get
            {
                bool result;

                using var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod());

                lock (stateLock)
                {
                    lockTracker.Acquired();

                    NetworkHealthReport networkHealth = networkHealthNeededCallback();
                    bool inStateAfterSetupCompleted = classState != CompetitionClassState.Offline && classState != CompetitionClassState.SetupCompleted;
                    bool isRunInProgress = inStateAfterSetupCompleted || networkHealth.RunComposition != null;
                    bool isNetworkCompliant = !networkHealth.HasErrors;
                    bool hasCompetitors = CacheManager.DefaultInstance.ActiveModel.Results.Any();

                    result = !isRunInProgress && isNetworkCompliant && hasCompetitors;
                }

                return result;
            }
        }

        public event EventHandler Aborted;
        public event EventHandler<RankingChangeEventArgs> RankingChanged;
        public event EventHandler<EventArgs<Exception>> UnknownErrorDuringSave;
        public event EventHandler<EventArgs<CompetitionClassState>> StateTransitioned;

        public CompetitionClassController([NotNull] Func<NetworkHealthReport> healthNeededCallback,
            [NotNull] ClockSynchronizationMonitor clockSynchronizationMonitor, [NotNull] ICompetitionRunVisualizer runVisualizer)
        {
            Guard.NotNull(healthNeededCallback, nameof(healthNeededCallback));
            Guard.NotNull(clockSynchronizationMonitor, nameof(clockSynchronizationMonitor));
            Guard.NotNull(runVisualizer, nameof(runVisualizer));

            networkHealthNeededCallback = healthNeededCallback;
            synchronizationMonitor = clockSynchronizationMonitor;
            visualizer = runVisualizer;

            clockSynchronizationMonitor.SyncRecommended += ClockSynchronizationMonitorOnSyncRecommended;
            clockSynchronizationMonitor.SyncRequired += ClockSynchronizationMonitorOnSyncRequired;
            clockSynchronizationMonitor.SyncCompleted += ClockSynchronizationMonitorOnSyncCompleted;

            runData.EliminationTracker.EliminationChanged += EliminationTrackerOnEliminationChanged;
            runData.EliminationTracker.RefusalCountChanged += EliminationTrackerOnRefusalCountChanged;

            modelSnapshot = CacheManager.DefaultInstance.ActiveModel;
        }

        private void EliminationTrackerOnEliminationChanged([CanBeNull] object sender, [NotNull] EliminationEventArgs e)
        {
            ExecuteExclusiveIfStateIn(AllStates, () =>
            {
                using var collector = new VisualizationUpdateCollector(visualizer);

                runData.HideExistingRunResultIfAny(collector);

                collector.Include(new EliminationUpdate(e.IsEliminated));

                if (e.IsEliminated)
                {
                    if (modelSnapshot.Alerts.Eliminated.Picture.EffectiveItem != null)
                    {
                        collector.Include(new StartAnimation(modelSnapshot.Alerts.Eliminated.Picture.EffectiveItem));
                    }

                    if (modelSnapshot.Alerts.Eliminated.Sound.EffectivePath != null)
                    {
                        collector.Include(new PlaySound(modelSnapshot.Alerts.Eliminated.Sound.EffectivePath));
                    }
                }
            });
        }

        private void EliminationTrackerOnRefusalCountChanged([CanBeNull] object sender, [NotNull] EventArgs<int> e)
        {
            ExecuteExclusiveIfStateIn(AllStates, () =>
            {
                using var collector = new VisualizationUpdateCollector(visualizer);

                runData.HideExistingRunResultIfAny(collector);

                collector.Include(new RefusalCountUpdate(e.Argument));
            });
        }

        private void ClockSynchronizationMonitorOnSyncRecommended([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            ExecuteExclusiveIfStateIn(CompetitionClassState.ReadyToStart,
                () => VisualizationUpdateCollector.Single(visualizer, new ClockSynchronizationUpdate(ClockSynchronizationMode.RecommendSynchronization)));
        }

        private void ClockSynchronizationMonitorOnSyncRequired([CanBeNull] object sender, [NotNull] EventArgs e)
        {
            ExecuteExclusiveIfStateIn(CompetitionClassState.ReadyToStart, () =>
            {
                SetState(CompetitionClassState.SetupCompleted);

                VisualizationUpdateCollector.Single(visualizer, new ClockSynchronizationUpdate(ClockSynchronizationMode.RequireSynchronization));
            });
        }

        [CanBeNull]
        public string TryUpdateRunResult([NotNull] CompetitionRunResult originalRunVersion, [NotNull] CompetitionRunResult newRunVersion)
        {
            Guard.NotNull(originalRunVersion, nameof(originalRunVersion));
            Guard.NotNull(newRunVersion, nameof(newRunVersion));

            string result = null;

            ExecuteExclusiveIfStateIn(AllStates, () =>
            {
                // Assumptions:
                // 1] When class is started, underlying cache can never be changed from the outside.
                // 2] When no run active (state Offline), we are not in control of underlying cache 
                //    (so snapshot field should not be read because it is likely outdated).

                CompetitionClassModel activeModelVersion = classState == CompetitionClassState.Offline
                    ? CacheManager.DefaultInstance.ActiveModel
                    : modelSnapshot;

                CompetitionRunResult activeRunVersion = activeModelVersion.GetRunResultOrNull(originalRunVersion.Competitor.Number);

                if (activeRunVersion == null)
                {
                    result = "Competitor not found";
                    return;
                }

                if (!CompetitionRunResult.AreEquivalent(activeRunVersion, originalRunVersion))
                {
                    result = "Competitor changed in-between";
                    return;
                }

                if (classState != CompetitionClassState.Offline && currentCompetitorNumber == newRunVersion.Competitor.Number)
                {
                    result = "Competitor is running";
                    return;
                }

                CompetitionClassModel newSnapshot = activeModelVersion.ChangeRunResult(newRunVersion).RecalculatePlacements();

                modelSnapshot = CacheManager.DefaultInstance.ReplaceModel(newSnapshot, activeModelVersion);
                AutoExportRunResults();

                if (classState != CompetitionClassState.Offline)
                {
                    using var collector = new VisualizationUpdateCollector(visualizer);

                    IReadOnlyCollection<CompetitionRunResult> rankings = modelSnapshot.FilterCompletedAndSortedAscendingByPlacement().Results;
                    collector.Include(new RankingsUpdate(rankings));

                    if (newRunVersion.Competitor.Number == modelSnapshot.LastCompletedCompetitorNumber)
                    {
                        CompetitionRunResult previousCompetitorOrNull = modelSnapshot.GetLastCompletedOrNull();
                        collector.Include(new PreviousCompetitorRunUpdate(previousCompetitorOrNull));
                    }
                }
            });

            return result;
        }

        public void StartClass([NotNull] CompetitionClassRequirements requirements)
        {
            Guard.NotNull(requirements, nameof(requirements));

            Log.Debug("Entering StartClass.");

            ExecuteExclusiveIfStateIn(CompetitionClassState.Offline, () =>
            {
                modelSnapshot = CacheManager.DefaultInstance.ActiveModel;
                currentCompetitorNumber = modelSnapshot.GetBestStartingCompetitorNumber();

                nextCompetitorNumberTyped = null;
                nextCompetitorNumberGenerated = modelSnapshot.GetBestNextCompetitorNumberOrNull(currentCompetitorNumber);

                classRequirements = requirements;
                runData.Reset(false);

                SetState(CompetitionClassState.SetupCompleted);

                using var collector = new VisualizationUpdateCollector(visualizer);

                collector.Include(VisualizationChangeFactory.ClearAll());
                collector.Include(new ClassInfoUpdate(modelSnapshot.ClassInfo));

                UpdateCurrentCompetitorVisualization(collector);
                UpdateNextCompetitorVisualization(collector);

                CompetitionRunResult previousCompetitorOrNull = modelSnapshot.GetLastCompletedOrNull();
                collector.Include(new PreviousCompetitorRunUpdate(previousCompetitorOrNull));

                IReadOnlyCollection<CompetitionRunResult> rankings = modelSnapshot.FilterCompletedAndSortedAscendingByPlacement().Results;
                collector.Include(new RankingsUpdate(rankings));
            });
        }

        public void StartCompetitorSelection(bool isCurrentCompetitor)
        {
            Log.Debug($"Entering StartCompetitorSelection for {CurrentOrNext(isCurrentCompetitor)} competitor.");

            ICollection<CompetitionClassState> statesAllowed = isCurrentCompetitor
                ? StatesInRange(CompetitionClassState.SetupCompleted, CompetitionClassState.FinishPassed)
                : StatesInRange(CompetitionClassState.SetupCompleted, CompetitionClassState.RunCompleted);

            ExecuteExclusiveIfStateIn(statesAllowed,
                () => VisualizationUpdateCollector.Single(visualizer, VisualizationChangeFactory.CompetitorNumberBlinkOn(isCurrentCompetitor)));
        }

        [NotNull]
        private static string CurrentOrNext(bool isCurrentCompetitor)
        {
            return isCurrentCompetitor ? "current" : "next";
        }

        public void ReceiveDigit(bool isCurrentCompetitor, int competitorNumber)
        {
            Log.Debug($"Entering ReceiveDigit with number {competitorNumber} for {CurrentOrNext(isCurrentCompetitor)} competitor.");

            ICollection<CompetitionClassState> statesAllowed = isCurrentCompetitor
                ? StatesInRange(CompetitionClassState.SetupCompleted, CompetitionClassState.FinishPassed)
                : StatesInRange(CompetitionClassState.SetupCompleted, CompetitionClassState.RunCompleted);

            ExecuteExclusiveIfStateIn(statesAllowed,
                () => VisualizationUpdateCollector.Single(visualizer,
                    VisualizationChangeFactory.CompetitorNumberUpdate(isCurrentCompetitor, competitorNumber)));
        }

        public void CompleteCompetitorSelection(bool isCurrentCompetitor, [CanBeNull] int? competitorNumber)
        {
            Log.Debug($"Entering CompleteCompetitorSelection with number {competitorNumber} for {CurrentOrNext(isCurrentCompetitor)} competitor.");

            ICollection<CompetitionClassState> statesAllowed = isCurrentCompetitor
                ? StatesInRange(CompetitionClassState.SetupCompleted, CompetitionClassState.FinishPassed)
                : StatesInRange(CompetitionClassState.SetupCompleted, CompetitionClassState.RunCompleted);

            ExecuteExclusiveIfStateIn(statesAllowed, () =>
            {
                using var collector = new VisualizationUpdateCollector(visualizer);

                if (competitorNumber != null)
                {
                    if (isCurrentCompetitor)
                    {
                        if (modelSnapshot.Results.Any(r => r.Competitor.Number == competitorNumber.Value))
                        {
                            currentCompetitorNumber = competitorNumber.Value;

                            if (nextCompetitorNumberTyped == null || nextCompetitorNumberTyped.Value == currentCompetitorNumber.Value)
                            {
                                nextCompetitorNumberTyped = null;
                                nextCompetitorNumberGenerated = modelSnapshot.GetBestNextCompetitorNumberOrNull(currentCompetitorNumber);

                                UpdateNextCompetitorVisualization(collector);
                            }
                        }

                        UpdateCurrentCompetitorVisualization(collector);

                        if (classState == CompetitionClassState.ReadyToStart)
                        {
                            ClearCurrentRunOrShowExistingRun(collector);
                        }
                    }
                    else if (modelSnapshot.Results.Any(r =>
                        r.Competitor.Number == competitorNumber.Value &&
                        (currentCompetitorNumber == null || r.Competitor.Number != currentCompetitorNumber.Value)))
                    {
                        nextCompetitorNumberTyped = competitorNumber.Value;
                    }

                    UpdateNextCompetitorVisualization(collector);
                }

                collector.Include(VisualizationChangeFactory.CompetitorNumberBlinkOff(isCurrentCompetitor));
            });
        }

        private void UpdateCurrentCompetitorVisualization([NotNull] VisualizationUpdateCollector collector)
        {
            Competitor currentCompetitor = currentCompetitorNumber != null ? modelSnapshot.GetRunResultFor(currentCompetitorNumber.Value).Competitor : null;
            collector.Include(new CurrentCompetitorUpdate(currentCompetitor));
        }

        private void UpdateNextCompetitorVisualization([NotNull] VisualizationUpdateCollector collector)
        {
            Competitor nextCompetitorOrNull = NextCompetitorNumber != null ? modelSnapshot.GetRunResultFor(NextCompetitorNumber.Value).Competitor : null;

            collector.Include(new NextCompetitorUpdate(nextCompetitorOrNull));
        }

        public void HandleGatePassed([CanBeNull] TimeSpan? sensorTime, GatePassage passageGate, [NotNull] WirelessNetworkAddress source)
        {
            string timeText = sensorTime != null ? "with time " + sensorTime : "without time";
            Log.Debug($"Entering HandleGatePassed for {passageGate} {timeText}.");

            // Devices without capability TimeSensor are considered low-precision, but hardware time is used.
            bool useSensorTime = synchronizationMonitor.IsDeviceSynchronized(source);
            TimeAccuracy? accuracy = DeviceHasTimeSensorCapability(source) ? null : TimeAccuracy.LowPrecision;
            var passageTime = new RecordedTime(useSensorTime ? sensorTime : null, SystemContext.UtcNow(), accuracy);

            Log.Debug($"Evaluated sensor time: {passageTime}");

            switch (passageGate)
            {
                case GatePassage.PassStartFinish:
                    HandlePassStartOrFinishWhenSingleSensor(passageTime);
                    break;
                case GatePassage.PassStart:
                    HandlePassStart(passageTime);
                    break;
                case GatePassage.PassIntermediate1:
                    HandlePassIntermediate1(passageTime);
                    break;
                case GatePassage.PassIntermediate2:
                    HandlePassIntermediate2(passageTime);
                    break;
                case GatePassage.PassIntermediate3:
                    HandlePassIntermediate3(passageTime);
                    break;
                case GatePassage.PassFinish:
                    HandlePassFinish(passageTime);
                    break;
                default:
                    throw ExceptionFactory.CreateNotSupportedExceptionFor(passageGate);
            }
        }

        private bool DeviceHasTimeSensorCapability([NotNull] WirelessNetworkAddress deviceAddress)
        {
            NetworkHealthReport health = networkHealthNeededCallback();
            return health.RunComposition != null && health.RunComposition.HasCapability(deviceAddress, DeviceCapabilities.TimeSensor);
        }

        private void HandlePassStartOrFinishWhenSingleSensor([NotNull] RecordedTime passageTime)
        {
            ExecuteExclusiveIfStateIn(StatesInRange(CompetitionClassState.ReadyToStart, CompetitionClassState.Intermediate3Passed), () =>
            {
                if (classState == CompetitionClassState.ReadyToStart)
                {
                    HandlePassStart(passageTime);
                }
                else
                {
                    CompetitionClassRequirements requirementsNotNull = AssertClassRequirementsNotNull();
                    CompetitionRunTimings timingsNotNull = AssertRunDataTimingsNotNull();

                    bool allowFinish = true;

                    if (classState == CompetitionClassState.StartPassed)
                    {
                        TimeSpan elapsed = passageTime.ElapsedSince(timingsNotNull.StartTime).TimeValue;
                        allowFinish = elapsed >= requirementsNotNull.StartFinishMinDelayForSingleSensor;
                    }

                    if (allowFinish)
                    {
                        HandlePassFinish(passageTime);
                    }
                    else
                    {
                        Log.Debug("Ignoring passage of start/finish sensor, because insufficient time has passed since start.");
                    }
                }
            });
        }

        [AssertionMethod]
        [NotNull]
        private CompetitionClassRequirements AssertClassRequirementsNotNull()
        {
            return Assertions.InternalValueIsNotNull(() => classRequirements, () => classRequirements);
        }

        [AssertionMethod]
        [NotNull]
        private CompetitionRunTimings AssertRunDataTimingsNotNull()
        {
            return Assertions.InternalValueIsNotNull(() => runData.Timings, () => runData.Timings);
        }

        private void HandlePassStart([NotNull] RecordedTime passageTime)
        {
            ExecuteExclusiveIfStateIn(CompetitionClassState.ReadyToStart, () =>
            {
                runData.Timings = new CompetitionRunTimings(passageTime);
                SetState(CompetitionClassState.StartPassed);

                runData.EliminationTracker.StartMonitorCourseTime(modelSnapshot.ClassInfo.MaximumCourseTime);

                using var collector = new VisualizationUpdateCollector(visualizer);

                runData.HideExistingRunResultIfAny(collector);

                collector.Include(new StartPrimaryTimer());
            });
        }

        private void HandlePassIntermediate1([NotNull] RecordedTime passageTime)
        {
            ExecuteExclusiveIfStateIn(CompetitionClassState.StartPassed, () =>
            {
                CompetitionRunTimings timingsNotNull = AssertRunDataTimingsNotNull();

                if (modelSnapshot.IntermediateTimerCount >= 1)
                {
                    runData.Timings = timingsNotNull.ChangeIntermediateTime1(passageTime);
                    SetState(CompetitionClassState.Intermediate1Passed);

                    TimeSpanWithAccuracy elapsed = passageTime.ElapsedSince(runData.Timings.StartTime);
                    Log.Info($"Passed Intermediate1 at {elapsed}.");
                    VisualizationUpdateCollector.Single(visualizer, SecondaryTimeUpdate.FromTimeSpanWithAccuracy(elapsed, false));
                }
            });
        }

        private void HandlePassIntermediate2([NotNull] RecordedTime passageTime)
        {
            ExecuteExclusiveIfStateIn(CompetitionClassState.Intermediate1Passed, () =>
            {
                CompetitionRunTimings timingsNotNull = AssertRunDataTimingsNotNull();

                if (modelSnapshot.IntermediateTimerCount >= 2)
                {
                    runData.Timings = timingsNotNull.ChangeIntermediateTime2(passageTime);
                    SetState(CompetitionClassState.Intermediate2Passed);

                    TimeSpanWithAccuracy elapsed = passageTime.ElapsedSince(runData.Timings.StartTime);
                    Log.Info($"Passed Intermediate2 at {elapsed}.");
                    VisualizationUpdateCollector.Single(visualizer, SecondaryTimeUpdate.FromTimeSpanWithAccuracy(elapsed, true));
                }
            });
        }

        private void HandlePassIntermediate3([NotNull] RecordedTime passageTime)
        {
            ExecuteExclusiveIfStateIn(CompetitionClassState.Intermediate2Passed, () =>
            {
                CompetitionRunTimings timingsNotNull = AssertRunDataTimingsNotNull();

                if (modelSnapshot.IntermediateTimerCount >= 3)
                {
                    runData.Timings = timingsNotNull.ChangeIntermediateTime3(passageTime);
                    SetState(CompetitionClassState.Intermediate3Passed);

                    TimeSpanWithAccuracy elapsed = passageTime.ElapsedSince(runData.Timings.StartTime);
                    Log.Info($"Passed Intermediate3 at {elapsed}.");
                    VisualizationUpdateCollector.Single(visualizer, SecondaryTimeUpdate.FromTimeSpanWithAccuracy(elapsed, true));
                }
            });
        }

        private void HandlePassFinish([NotNull] RecordedTime passageTime)
        {
            ExecuteExclusiveIfStateIn(StatesInRange(CompetitionClassState.StartPassed, CompetitionClassState.Intermediate3Passed), () =>
            {
                using var collector = new VisualizationUpdateCollector(visualizer);

                CompetitionRunTimings timingsNotNull = AssertRunDataTimingsNotNull();

                runData.Timings = timingsNotNull.ChangeFinishTime(passageTime);
                SetState(CompetitionClassState.FinishPassed);

                TimeSpanWithAccuracy elapsed = passageTime.ElapsedSince(runData.Timings.StartTime);
                Log.Info($"Passed Finish at {elapsed}.");

                runData.EliminationTracker.StopMonitorCourseTime();
                collector.Include(PrimaryTimeStopAndSet.FromTimeSpanWithAccuracy(elapsed));
            });
        }

        public void HandleCommand(DeviceCommand command)
        {
            Log.Debug($"Entering HandleCommand for {command}.");

            switch (command)
            {
                case DeviceCommand.Ready:
                    HandleReady();
                    break;
                case DeviceCommand.DecreaseRefusals:
                    HandleChangeRefusals(false);
                    break;
                case DeviceCommand.IncreaseRefusals:
                    HandleChangeRefusals(true);
                    break;
                case DeviceCommand.DecreaseFaults:
                    HandleChangeFaults(false);
                    break;
                case DeviceCommand.IncreaseFaults:
                    HandleChangeFaults(true);
                    break;
                case DeviceCommand.ToggleElimination:
                    HandleToggleElimination();
                    break;
                case DeviceCommand.ResetRun:
                    HandleResetRun();
                    break;
                case DeviceCommand.PlaySoundA:
                    HandlePlaySoundA();
                    break;
                case DeviceCommand.MuteSound:
                    HandleMuteSound();
                    break;
                default:
                    throw ExceptionFactory.CreateNotSupportedExceptionFor(command);
            }
        }

        private void HandleReady()
        {
            ExecuteExclusiveIfStateIn(StatesInRange(CompetitionClassState.SetupCompleted, CompetitionClassState.RunCompleted), () =>
            {
                using var collector = new VisualizationUpdateCollector(visualizer);

                switch (classState)
                {
                    case CompetitionClassState.SetupCompleted:
                        PrepareForRun(collector, true);
                        break;
                    case CompetitionClassState.ReadyToStart:
                        StartClockSynchronization(collector);
                        break;
                    case CompetitionClassState.RunCompleted:
                        PrepareForRun(collector, false);
                        break;
                    default:
                        if (runData.EliminationTracker.IsEliminated || classState == CompetitionClassState.FinishPassed)
                        {
                            CompleteActiveRun(collector);
                        }

                        break;
                }
            });
        }

        private void PrepareForRun([NotNull] VisualizationUpdateCollector collector, bool isFirstRun)
        {
            if (!isFirstRun)
            {
                currentCompetitorNumber = NextCompetitorNumber;
                nextCompetitorNumberTyped = null;
                nextCompetitorNumberGenerated = modelSnapshot.GetBestNextCompetitorNumberOrNull(currentCompetitorNumber);
            }

            bool isClassCompleted = currentCompetitorNumber == null;

            if (isClassCompleted)
            {
                GoOffline(collector);
            }
            else
            {
                runData.Reset(false);

                if (!isFirstRun)
                {
                    UpdateCurrentCompetitorVisualization(collector);
                    UpdateNextCompetitorVisualization(collector);
                }

                NetworkHealthReport networkHealth = networkHealthNeededCallback();

                if (!networkHealth.HasErrors)
                {
                    StartClockSynchronization(collector);
                }
                else
                {
                    Log.Info($"Denying start of run because network has errors - {networkHealth}");

                    SetState(CompetitionClassState.SetupCompleted);
                    collector.Include(VisualizationChangeFactory.ClearCurrentRun());
                }
            }
        }

        private void CompleteActiveRun([NotNull] VisualizationUpdateCollector collector)
        {
            runData.EliminationTracker.StopMonitorCourseTime();

            if (!runData.HasFinished)
            {
                collector.Include(PrimaryTimeStopAndSet.Hidden);
            }

            PersistRunResultToCache();

            int competitorNumber = AssertCurrentCompetitorNumberNotNull();
            CompetitionRunResult previousRunResultOrNull = modelSnapshot.GetRunResultFor(competitorNumber);
            collector.Include(new PreviousCompetitorRunUpdate(previousRunResultOrNull));

            IReadOnlyCollection<CompetitionRunResult> rankings = modelSnapshot.FilterCompletedAndSortedAscendingByPlacement().Results;
            collector.Include(new RankingsUpdate(rankings));

            RankingChanged?.Invoke(this, new RankingChangeEventArgs(rankings, previousRunResultOrNull));

            SetState(CompetitionClassState.RunCompleted);

            AnimateForRunCompleted(collector);
        }

        private void AnimateForRunCompleted([NotNull] VisualizationUpdateCollector collector)
        {
            if (runData.HasFaultsOrRefusalsOrIsEliminated)
            {
                return;
            }

            CompetitionRunTimings timings = AssertRunDataTimingsNotNull();

            if (modelSnapshot.ClassInfo.StandardCourseTime != null && timings.FinishTime != null)
            {
                TimeSpanWithAccuracy elapsed = timings.FinishTime.ElapsedSince(timings.StartTime);

                if (elapsed.TimeValue > modelSnapshot.ClassInfo.StandardCourseTime.Value)
                {
                    return;
                }
            }

            // Make sure we do not show picture for FirstPlace while at the same time
            // play the sound for CleanRunInStandardCourseTime.
            bool firstPlaceAnimated = false;

            if (MatchesConditionsForFirstPlaceAlert())
            {
                if (modelSnapshot.Alerts.FirstPlace.Picture.EffectiveItem != null)
                {
                    collector.Include(new StartAnimation(modelSnapshot.Alerts.FirstPlace.Picture.EffectiveItem));
                    firstPlaceAnimated = true;
                }

                if (modelSnapshot.Alerts.FirstPlace.Sound.EffectivePath != null)
                {
                    collector.Include(new PlaySound(modelSnapshot.Alerts.FirstPlace.Sound.EffectivePath));
                    firstPlaceAnimated = true;
                }
            }

            if (!firstPlaceAnimated)
            {
                if (modelSnapshot.Alerts.CleanRunInStandardCourseTime.Picture.EffectiveItem != null)
                {
                    collector.Include(new StartAnimation(modelSnapshot.Alerts.CleanRunInStandardCourseTime.Picture.EffectiveItem));
                }

                if (modelSnapshot.Alerts.CleanRunInStandardCourseTime.Sound.EffectivePath != null)
                {
                    collector.Include(new PlaySound(modelSnapshot.Alerts.CleanRunInStandardCourseTime.Sound.EffectivePath));
                }
            }
        }

        private bool MatchesConditionsForFirstPlaceAlert()
        {
            if (!modelSnapshot.Results.Any(r => r.Placement > 0 && r.Competitor.Number != currentCompetitorNumber))
            {
                return false;
            }

            IReadOnlyCollection<CompetitionRunResult> rankings = modelSnapshot.FilterCompletedAndSortedAscendingByPlacement().Results;

            IEnumerable<CompetitionRunResult> firstPlaces = rankings.TakeWhile(r => r.Placement == 1);
            return firstPlaces.Any(r => r.Competitor.Number == currentCompetitorNumber);
        }

        private int AssertCurrentCompetitorNumberNotNull()
        {
            return Assertions.InternalValueIsNotNull(() => currentCompetitorNumber, () => currentCompetitorNumber);
        }

        private void PersistRunResultToCache()
        {
            try
            {
                if (currentCompetitorNumber != null)
                {
                    Competitor existingCompetitor = modelSnapshot.GetRunResultFor(currentCompetitorNumber.Value).Competitor;
                    CompetitionRunResult newRunResult = runData.ToRunResultFor(existingCompetitor);

                    CompetitionClassModel newSnapshot = modelSnapshot.ChangeRunResult(newRunResult)
                        .SafeChangeLastCompletedCompetitorNumber(existingCompetitor.Number).RecalculatePlacements();

                    modelSnapshot = CacheManager.DefaultInstance.ReplaceModel(newSnapshot, modelSnapshot);
                    AutoExportRunResults();
                }
            }
            catch (Exception ex)
            {
                int competitorNumber = AssertCurrentCompetitorNumberNotNull();

                string message = "Failed to save run results. Please write these down manually, " +
                    $"or press Ctrl+C to copy to clipboard.\n\n{runData.GetMessageFor(competitorNumber)}\n";

                Log.Error(message, ex);
                var exception = new Exception(message, ex);
                UnknownErrorDuringSave?.Invoke(this, new EventArgs<Exception>(exception));
            }
        }

        private void AutoExportRunResults()
        {
            if (Settings.Default.AutoExportRunResults)
            {
                string commonAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                string exportFolder = Path.Combine(commonAppDataFolder, "DogAgilityCompetitionController", "AutoExports");
                Directory.CreateDirectory(exportFolder);

                string exportPath = Path.Combine(exportFolder, $"AgilityRun_{SystemContext.UtcNow():yyyyMMdd_HHmmss}.csv");
                RunResultsExporter.ExportTo(exportPath, modelSnapshot.Results);
            }
        }

        private void StartClockSynchronization([NotNull] VisualizationUpdateCollector collector)
        {
            collector.Include(VisualizationChangeFactory.ClearCurrentRun());

            SetState(CompetitionClassState.WaitingForSync);

            NetworkHealthReport networkHealth = networkHealthNeededCallback();
            networkHealth = AssertNotNull(networkHealth);
            NetworkComposition networkComposition = AssertCompositionIsNotNull(networkHealth);

            IEnumerable<WirelessNetworkAddress> devicesToSynchronize =
                networkComposition.GetDeviceAddresses().Where(networkComposition.RequiresClockSynchronization);

            synchronizationMonitor.StartNetworkSynchronization(devicesToSynchronize.ToList());
        }

        [NotNull]
        private static NetworkHealthReport AssertNotNull([CanBeNull] NetworkHealthReport networkHealth)
        {
            if (networkHealth == null)
            {
                throw new InvalidOperationException("Network health is not available at this time.");
            }

            return networkHealth;
        }

        [NotNull]
        private static NetworkComposition AssertCompositionIsNotNull([NotNull] NetworkHealthReport networkHealth)
        {
            if (networkHealth.RunComposition == null)
            {
                throw new InvalidOperationException("Network health reports no active run.");
            }

            return networkHealth.RunComposition;
        }

        private void ClockSynchronizationMonitorOnSyncCompleted([CanBeNull] object sender, [NotNull] ClockSynchronizationCompletedEventArgs e)
        {
            ExecuteExclusiveIfStateIn(CompetitionClassState.WaitingForSync, () =>
            {
                using var collector = new VisualizationUpdateCollector(visualizer);

                if (e.Result == ClockSynchronizationResult.Succeeded)
                {
                    collector.Include(new ClockSynchronizationUpdate(ClockSynchronizationMode.Normal));

                    if (modelSnapshot.Alerts.ReadyToStart.Sound.EffectivePath != null)
                    {
                        collector.Include(new PlaySound(modelSnapshot.Alerts.ReadyToStart.Sound.EffectivePath));
                    }

                    ClearCurrentRunOrShowExistingRun(collector);
                }
                else
                {
                    GoOffline(collector);
                }
            });
        }

        private void HandleChangeRefusals(bool isIncrement)
        {
            ExecuteExclusiveIfStateIn(StatesInRange(CompetitionClassState.ReadyToStart, CompetitionClassState.FinishPassed), () =>
            {
                if (isIncrement)
                {
                    runData.EliminationTracker.IncreaseRefusals();
                }
                else
                {
                    runData.EliminationTracker.DecreaseRefusals();
                }
            });
        }

        private void HandleChangeFaults(bool isIncrement)
        {
            ExecuteExclusiveIfStateIn(StatesInRange(CompetitionClassState.ReadyToStart, CompetitionClassState.FinishPassed), () =>
            {
                if ((isIncrement && runData.FaultCount <= CompetitionRunResult.MaxFaultValue - CompetitionRunResult.FaultStepSize) ||
                    (!isIncrement && runData.FaultCount > 0))
                {
                    int stepSize = isIncrement ? CompetitionRunResult.FaultStepSize : -CompetitionRunResult.FaultStepSize;
                    int faultCount = runData.FaultCount + stepSize;
                    runData.FaultCount = faultCount;
                }

                using var collector = new VisualizationUpdateCollector(visualizer);

                runData.HideExistingRunResultIfAny(collector);

                collector.Include(new FaultCountUpdate(runData.FaultCount));
            });
        }

        private void HandleToggleElimination()
        {
            ExecuteExclusiveIfStateIn(StatesInRange(CompetitionClassState.ReadyToStart, CompetitionClassState.FinishPassed), () =>
            {
                runData.EliminationTracker.IsManuallyEliminated = !runData.EliminationTracker.IsManuallyEliminated;
            });
        }

        private void HandleResetRun()
        {
            ExecuteExclusiveIfStateIn(StatesInRange(CompetitionClassState.ReadyToStart, CompetitionClassState.FinishPassed), () =>
            {
                using var collector = new VisualizationUpdateCollector(visualizer);
                ClearCurrentRunOrShowExistingRun(collector);
            });
        }

        private void HandlePlaySoundA()
        {
            ExecuteExclusiveIfStateIn(StatesInRange(CompetitionClassState.SetupCompleted, CompetitionClassState.RunCompleted), () =>
            {
                if (modelSnapshot.Alerts.CustomItemA.Sound.EffectivePath != null)
                {
                    using var collector = new VisualizationUpdateCollector(visualizer);
                    collector.Include(new PlaySound(modelSnapshot.Alerts.CustomItemA.Sound.EffectivePath));
                }
            });
        }

        private void HandleMuteSound()
        {
            ExecuteExclusiveIfStateIn(StatesInRange(CompetitionClassState.SetupCompleted, CompetitionClassState.RunCompleted), () =>
            {
                using var collector = new VisualizationUpdateCollector(visualizer);
                collector.Include(PlaySound.Mute);
            });
        }

        public void RequestAbort()
        {
            Log.Debug("Entering RequestAbort.");

            ExecuteExclusiveIfStateIn(AllStates, () =>
            {
                using var collector = new VisualizationUpdateCollector(visualizer);
                GoOffline(collector);
            });
        }

        private void GoOffline([NotNull] VisualizationUpdateCollector collector)
        {
            synchronizationMonitor.Suspend();
            collector.Include(new ClockSynchronizationUpdate(ClockSynchronizationMode.Normal));

            collector.Include(VisualizationChangeFactory.ClearAll());
            collector.Include(PlaySound.Mute);

            SetState(CompetitionClassState.Offline);

            Aborted?.Invoke(this, EventArgs.Empty);
        }

        private void ClearCurrentRunOrShowExistingRun([NotNull] VisualizationUpdateCollector collector)
        {
            int currentCompetitorNumberNotNull = AssertCurrentCompetitorNumberNotNull();
            CompetitionRunResult existingRunResult = modelSnapshot.GetRunResultFor(currentCompetitorNumberNotNull);

            if (existingRunResult.HasCompleted)
            {
                RecordedTime latestIntermediateTimeOrNull = modelSnapshot.GetLatestIntermediateTimeOrNull(currentCompetitorNumberNotNull);
                collector.Include(VisualizationChangeFactory.ForExistingRun(existingRunResult, latestIntermediateTimeOrNull));

                runData.Reset(true);
            }
            else
            {
                collector.Include(VisualizationChangeFactory.ResetCurrentRun());

                runData.Reset(false);
            }

            SetState(CompetitionClassState.ReadyToStart);
        }

        private void ExecuteExclusiveIfStateIn(CompetitionClassState stateAllowed, [NotNull] Action action)
        {
            ExecuteExclusiveIfStateIn(new[]
            {
                stateAllowed
            }, action);
        }

        private void ExecuteExclusiveIfStateIn([NotNull] ICollection<CompetitionClassState> statesAllowed, [NotNull] Action action)
        {
            using var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod());

            lock (stateLock)
            {
                lockTracker.Acquired();

                if (statesAllowed.Contains(classState))
                {
                    action();
                }
                else
                {
                    string statesList = string.Join(", ", statesAllowed.Select(s => s.ToString()));
                    Log.Debug($"Discarding operation, because current state {classState} is not in allowed set ({statesList}).");
                }
            }
        }

        private void SetState(CompetitionClassState newState)
        {
            if (classState != newState)
            {
                Log.Info($"Entering state: {newState}.");
                classState = newState;

                StateTransitioned?.Invoke(this, new EventArgs<CompetitionClassState>(classState));
            }
        }

        [NotNull]
        private static ICollection<CompetitionClassState> StatesInRange(CompetitionClassState first, CompetitionClassState lastInclusive)
        {
            // @formatter:keep_existing_linebreaks true

            IEnumerable<CompetitionClassState> enumValues = Enum
                .GetValues(typeof(CompetitionClassState))
                .Cast<CompetitionClassState>()
                .SkipWhile(s => s != first)
                .TakeWhile(s => s != lastInclusive);

            // @formatter:keep_existing_linebreaks restore

            var states = new HashSet<CompetitionClassState>(enumValues)
            {
                lastInclusive
            };

            return states;
        }

        public void Dispose()
        {
            runData.Dispose();
        }

        private sealed class CompetitionRunData : IDisposable
        {
            private bool isShowingExistingRunResult;

            [CanBeNull]
            public CompetitionRunTimings Timings { get; set; }

            [NotNull]
            public EliminationTracker EliminationTracker { get; } = new(CompetitionRunResult.RefusalStepSize, CompetitionRunResult.EliminationThreshold);

            public int FaultCount { get; set; }

            public bool HasFaultsOrRefusalsOrIsEliminated => FaultCount != 0 || EliminationTracker.RefusalCount != 0 || EliminationTracker.IsEliminated;

            public bool HasFinished => Timings?.FinishTime != null;

            [NotNull]
            public CompetitionRunResult ToRunResultFor([NotNull] Competitor competitor)
            {
                // @formatter:keep_existing_linebreaks true

                return new CompetitionRunResult(competitor)
                    .ChangeTimings(Timings)
                    .ChangeFaultCount(FaultCount)
                    .ChangeRefusalCount(EliminationTracker.RefusalCount)
                    .ChangeIsEliminated(EliminationTracker.IsEliminated);

                // @formatter:keep_existing_linebreaks restore
            }

            [NotNull]
            public string GetMessageFor(int competitorNumber)
            {
                var textBuilder = new StringBuilder();
                textBuilder.AppendLine($"Competitor number: {competitorNumber}");

                if (Timings != null)
                {
                    textBuilder.AppendLine($"Start time: {Timings.StartTime}");

                    if (Timings.IntermediateTime1 != null)
                    {
                        textBuilder.AppendLine($"Intermediate1 time: {Timings.IntermediateTime1}");
                    }

                    if (Timings.IntermediateTime2 != null)
                    {
                        textBuilder.AppendLine($"Intermediate2 time: {Timings.IntermediateTime2}");
                    }

                    if (Timings.IntermediateTime3 != null)
                    {
                        textBuilder.AppendLine($"Intermediate3 time: {Timings.IntermediateTime3}");
                    }

                    if (Timings.FinishTime != null)
                    {
                        textBuilder.AppendLine($"Finish time: {Timings.FinishTime}");
                    }
                }

                textBuilder.AppendLine($"Faults: {FaultCount}");
                textBuilder.AppendLine($"Refusals: {EliminationTracker.RefusalCount}");
                textBuilder.AppendLine($"Eliminated: {EliminationTracker.IsEliminated}");
                return textBuilder.ToString();
            }

            [Pure]
            public override string ToString()
            {
                var textBuilder = new StringBuilder();
                textBuilder.Append("Competitor");

                if (EliminationTracker.IsEliminated)
                {
                    textBuilder.Append(" has been eliminated");
                }
                else if (Timings?.FinishTime != null)
                {
                    textBuilder.Append(" finished at ");
                    TimeSpanWithAccuracy finishTime = Timings.FinishTime.ElapsedSince(Timings.StartTime);
                    textBuilder.Append(finishTime);

                    if (FaultCount > 0)
                    {
                        textBuilder.Append(" with ");
                        textBuilder.Append(FaultCount);
                        textBuilder.Append(" faults");
                    }

                    if (EliminationTracker.RefusalCount > 0)
                    {
                        textBuilder.Append(FaultCount > 0 ? " and " : " with ");
                        textBuilder.Append(EliminationTracker.RefusalCount);
                        textBuilder.Append(" refusals");
                    }
                }

                return textBuilder.ToString();
            }

            public void Dispose()
            {
                EliminationTracker.Dispose();
            }

            public void HideExistingRunResultIfAny([NotNull] VisualizationUpdateCollector collector)
            {
                Guard.NotNull(collector, nameof(collector));

                if (isShowingExistingRunResult)
                {
                    collector.Include(VisualizationChangeFactory.ResetCurrentRun());
                    isShowingExistingRunResult = false;
                }
            }

            public void Reset(bool hasExistingRunResult)
            {
                Timings = null;
                FaultCount = 0;

                EliminationTracker.Reset();

                isShowingExistingRunResult = hasExistingRunResult;
            }
        }
    }
}
