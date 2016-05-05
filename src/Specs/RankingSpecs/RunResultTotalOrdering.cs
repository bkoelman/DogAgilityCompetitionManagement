using System;
using System.Collections.Generic;
using System.Linq;
using DogAgilityCompetition.Controller.Engine;
using DogAgilityCompetition.Controller.Engine.Storage;
using DogAgilityCompetition.Specs.Builders;
using DogAgilityCompetition.Specs.Facilities;
using FluentAssertions;
using JetBrains.Annotations;
using NUnit.Framework;

namespace DogAgilityCompetition.Specs.RankingSpecs
{
    /// <summary>
    /// Tests for total/complete ordering of competitor run results.
    /// </summary>
    [TestFixture]
    public sealed class RunResultTotalOrdering
    {
        private static readonly TimeSpan LowOrSameExtraFinishTime = TimeSpan.FromSeconds(5);
        private static readonly TimeSpan HighExtraFinishTime = TimeSpan.FromSeconds(10);

        private static readonly TimeSpan LowOrSameOverrunTime = TimeSpan.FromSeconds(20);
        private static readonly TimeSpan HighOverrunTime = TimeSpan.FromSeconds(40);

        private static readonly TimeSpan LowOrSamePenaltyTime = TimeSpan.FromSeconds(80);
        private static readonly TimeSpan HighPenaltyTime = TimeSpan.FromSeconds(160);

        private const int LowOrSameCompetitorNumber = 3;
        private const int HighCompetitorNumber = 7;

        private static readonly TimeSpan StandardCourseTime = TimeSpan.FromSeconds(80);

        [Test]
        public void RunExploded()
        {
            // Arrange
            var runCompletionScenarios = new List<OrderingScenario>
            {
                // Bits: X HasFinished, X IsEliminated, Y HasFinished, Y IsEliminated
                new OrderingScenario(4, OrderingScenario.FromBits(0, 0, 0, 0), OrderExpect.IsEven),
                new OrderingScenario(4, OrderingScenario.FromBits(0, 0, 0, 1), OrderExpect.WinnerIsY),
                new OrderingScenario(4, OrderingScenario.FromBits(0, 0, 1, 0), OrderExpect.WinnerIsY),
                new OrderingScenario(4, OrderingScenario.FromBits(0, 0, 1, 1), OrderExpect.WinnerIsY),
                new OrderingScenario(4, OrderingScenario.FromBits(0, 1, 0, 0), OrderExpect.WinnerIsX),
                new OrderingScenario(4, OrderingScenario.FromBits(0, 1, 0, 1), OrderExpect.IsEven),
                new OrderingScenario(4, OrderingScenario.FromBits(0, 1, 1, 0), OrderExpect.WinnerIsY),
                new OrderingScenario(4, OrderingScenario.FromBits(0, 1, 1, 1), OrderExpect.WinnerIsY),
                new OrderingScenario(4, OrderingScenario.FromBits(1, 0, 0, 0), OrderExpect.WinnerIsX),
                new OrderingScenario(4, OrderingScenario.FromBits(1, 0, 0, 1), OrderExpect.WinnerIsX),
                new OrderingScenario(4, OrderingScenario.FromBits(1, 0, 1, 0), OrderExpect.IsEven),
                new OrderingScenario(4, OrderingScenario.FromBits(1, 0, 1, 1), OrderExpect.WinnerIsX),
                new OrderingScenario(4, OrderingScenario.FromBits(1, 1, 0, 0), OrderExpect.WinnerIsX),
                new OrderingScenario(4, OrderingScenario.FromBits(1, 1, 0, 1), OrderExpect.WinnerIsX),
                new OrderingScenario(4, OrderingScenario.FromBits(1, 1, 1, 0), OrderExpect.WinnerIsY),
                new OrderingScenario(4, OrderingScenario.FromBits(1, 1, 1, 1), OrderExpect.IsEven)
            };

            var penaltyOverrunScenarios = new List<OrderingScenario>
            {
                // Bits: PenaltyTime X > Y, OverrunTime X > Y, PenaltyTime Y > X, OverrunTime Y > X
                new OrderingScenario(4, OrderingScenario.FromBits(0, 0, 0, 0), OrderExpect.IsEven),
                new OrderingScenario(4, OrderingScenario.FromBits(0, 0, 0, 1), OrderExpect.WinnerIsY),
                new OrderingScenario(4, OrderingScenario.FromBits(0, 0, 1, 0), OrderExpect.WinnerIsX),
                new OrderingScenario(4, OrderingScenario.FromBits(0, 0, 1, 1), OrderExpect.WinnerIsX),
                new OrderingScenario(4, OrderingScenario.FromBits(0, 1, 0, 0), OrderExpect.WinnerIsX),
                new OrderingScenario(4, OrderingScenario.FromBits(0, 1, 0, 1), OrderExpect.DoNotCare),
                new OrderingScenario(4, OrderingScenario.FromBits(0, 1, 1, 0), OrderExpect.WinnerIsX),
                new OrderingScenario(4, OrderingScenario.FromBits(0, 1, 1, 1), OrderExpect.DoNotCare),
                new OrderingScenario(4, OrderingScenario.FromBits(1, 0, 0, 0), OrderExpect.WinnerIsY),
                new OrderingScenario(4, OrderingScenario.FromBits(1, 0, 0, 1), OrderExpect.WinnerIsY),
                new OrderingScenario(4, OrderingScenario.FromBits(1, 0, 1, 0), OrderExpect.DoNotCare),
                new OrderingScenario(4, OrderingScenario.FromBits(1, 0, 1, 1), OrderExpect.DoNotCare),
                new OrderingScenario(4, OrderingScenario.FromBits(1, 1, 0, 0), OrderExpect.WinnerIsY),
                new OrderingScenario(4, OrderingScenario.FromBits(1, 1, 0, 1), OrderExpect.DoNotCare),
                new OrderingScenario(4, OrderingScenario.FromBits(1, 1, 1, 0), OrderExpect.DoNotCare),
                new OrderingScenario(4, OrderingScenario.FromBits(1, 1, 1, 1), OrderExpect.DoNotCare)
            };

            var finishNumberScenarios = new List<OrderingScenario>
            {
                // Bits: FinishTime X > Y, CompetitorNumber X > Y, FinishTime Y > X, CompetitorNumber Y > X
                new OrderingScenario(4, OrderingScenario.FromBits(0, 0, 0, 0), OrderExpect.DoNotCare),
                new OrderingScenario(4, OrderingScenario.FromBits(0, 0, 0, 1), OrderExpect.WinnerIsX),
                new OrderingScenario(4, OrderingScenario.FromBits(0, 0, 1, 0), OrderExpect.DoNotCare),
                new OrderingScenario(4, OrderingScenario.FromBits(0, 0, 1, 1), OrderExpect.WinnerIsX),
                new OrderingScenario(4, OrderingScenario.FromBits(0, 1, 0, 0), OrderExpect.WinnerIsY),
                new OrderingScenario(4, OrderingScenario.FromBits(0, 1, 0, 1), OrderExpect.DoNotCare),
                new OrderingScenario(4, OrderingScenario.FromBits(0, 1, 1, 0), OrderExpect.WinnerIsX),
                new OrderingScenario(4, OrderingScenario.FromBits(0, 1, 1, 1), OrderExpect.DoNotCare),
                new OrderingScenario(4, OrderingScenario.FromBits(1, 0, 0, 0), OrderExpect.DoNotCare),
                new OrderingScenario(4, OrderingScenario.FromBits(1, 0, 0, 1), OrderExpect.WinnerIsY),
                new OrderingScenario(4, OrderingScenario.FromBits(1, 0, 1, 0), OrderExpect.DoNotCare),
                new OrderingScenario(4, OrderingScenario.FromBits(1, 0, 1, 1), OrderExpect.DoNotCare),
                new OrderingScenario(4, OrderingScenario.FromBits(1, 1, 0, 0), OrderExpect.WinnerIsY),
                new OrderingScenario(4, OrderingScenario.FromBits(1, 1, 0, 1), OrderExpect.DoNotCare),
                new OrderingScenario(4, OrderingScenario.FromBits(1, 1, 1, 0), OrderExpect.DoNotCare),
                new OrderingScenario(4, OrderingScenario.FromBits(1, 1, 1, 1), OrderExpect.DoNotCare)
            };

            IEnumerable<OrderingScenario> exploded = ExplodeCombinations(runCompletionScenarios, penaltyOverrunScenarios,
                finishNumberScenarios);

            CompetitionClassModel model = new CompetitionClassModel()
                .ChangeClassInfo(new CompetitionClassInfo()
                    .ChangeStandardCourseTime(StandardCourseTime));
            var comparer = new CompetitionRunResultRankingComparer(model, RankingComparisonMode.Regular);

            foreach (OrderingScenario scenario in exploded)
            {
                bool xHasFinished = scenario[0];
                bool xIsEliminated = scenario[1];
                bool yHasFinished = scenario[2];
                bool yIsEliminated = scenario[3];
                bool xPenaltyTimeIsGreater = scenario[4];
                bool xOverrunTimeIsGreater = scenario[5];
                bool yPenaltyTimeIsGreater = scenario[6];
                bool yOverrunTimeIsGreater = scenario[7];
                bool xFinishTimeIsGreater = scenario[8];
                bool xCompetitorNumberIsGreater = scenario[9];
                bool yFinishTimeIsGreater = scenario[10];
                bool yCompetitorNumberIsGreater = scenario[11];

                CompetitionRunResult xCompetitor = CreateCompetitorFor("X", xHasFinished, xIsEliminated, yHasFinished,
                    xPenaltyTimeIsGreater, xOverrunTimeIsGreater, yPenaltyTimeIsGreater, yOverrunTimeIsGreater,
                    xFinishTimeIsGreater, xCompetitorNumberIsGreater, yFinishTimeIsGreater, yCompetitorNumberIsGreater);
                CompetitionRunResult yCompetitor = CreateCompetitorFor("Y", yHasFinished, yIsEliminated, xHasFinished,
                    yPenaltyTimeIsGreater, yOverrunTimeIsGreater, xPenaltyTimeIsGreater, xOverrunTimeIsGreater,
                    yFinishTimeIsGreater, yCompetitorNumberIsGreater, xFinishTimeIsGreater, xCompetitorNumberIsGreater);

                AssertCompetitorsAreCompatibleWithProposedScenario(xCompetitor, yCompetitor, scenario, model);

                // Act
                int result = comparer.Compare(xCompetitor, yCompetitor);

                // Assert
                OrderExpect actual = TranslateComparerResult(result);
                actual.Should().Be(scenario.Result, scenario.ToString());
            }
        }

        [NotNull]
        [ItemNotNull]
        private static IEnumerable<OrderingScenario> ExplodeCombinations(
            [NotNull] [ItemNotNull] IEnumerable<OrderingScenario> primaryScenarios,
            [NotNull] [ItemNotNull] List<OrderingScenario> secondaryScenarios,
            [NotNull] [ItemNotNull] List<OrderingScenario> tertiaryScenarios)
        {
            return
                from primaryScenario in primaryScenarios
                from secondaryScenario in secondaryScenarios
                from tertiaryScenario in tertiaryScenarios
                select CreateCombinedScenario(primaryScenario, secondaryScenario, tertiaryScenario)
                into combined
                where combined != null
                select combined;
        }

        [CanBeNull]
        private static OrderingScenario CreateCombinedScenario([NotNull] OrderingScenario primaryScenario,
            [NotNull] OrderingScenario secondaryScenario, [NotNull] OrderingScenario tertiaryScenario)
        {
            OrderExpect result = CombineResult(primaryScenario, secondaryScenario, tertiaryScenario);
            if (result != OrderExpect.DoNotCare)
            {
                ulong sequence = CreateBitSequenceFor(primaryScenario, secondaryScenario, tertiaryScenario);
                var combined = new OrderingScenario(12, sequence, result);

                if (!IsImpossibleCombination(combined))
                {
                    return combined;
                }
            }
            return null;
        }

        private static OrderExpect CombineResult([NotNull] OrderingScenario primaryScenario,
            [NotNull] OrderingScenario secondaryScenario, [NotNull] OrderingScenario tertiaryScenario)
        {
            if (primaryScenario.Result == OrderExpect.DoNotCare ||
                secondaryScenario.Result == OrderExpect.DoNotCare ||
                tertiaryScenario.Result == OrderExpect.DoNotCare)
            {
                return OrderExpect.DoNotCare;
            }

            return primaryScenario.Result != OrderExpect.IsEven
                ? primaryScenario.Result
                : secondaryScenario.Result != OrderExpect.IsEven
                    ? secondaryScenario.Result
                    : tertiaryScenario.Result;
        }

        private static ulong CreateBitSequenceFor([NotNull] OrderingScenario primaryScenario,
            [NotNull] OrderingScenario secondaryScenario, [NotNull] OrderingScenario tertiaryScenario)
        {
            ulong leftNibble = primaryScenario.Value << (tertiaryScenario.BitCount + secondaryScenario.BitCount);
            ulong middleNibble = secondaryScenario.Value << tertiaryScenario.BitCount;
            ulong rightNibble = tertiaryScenario.Value;

            return leftNibble | middleNibble | rightNibble;
        }

        private static bool IsImpossibleCombination([NotNull] OrderingScenario scenario)
        {
            bool xHasFinished = scenario[0];
            bool yHasFinished = scenario[2];
            bool xPenaltyTimeIsGreater = scenario[4];
            bool xOverrunTimeIsGreater = scenario[5];
            bool yPenaltyTimeIsGreater = scenario[6];
            bool yOverrunTimeIsGreater = scenario[7];
            bool xFinishTimeIsGreater = scenario[8];
            bool yFinishTimeIsGreater = scenario[10];

            // Two competitors can never both have the same input greater than the other.
            if ((xPenaltyTimeIsGreater && yPenaltyTimeIsGreater) ||
                (xOverrunTimeIsGreater && yOverrunTimeIsGreater) ||
                (xFinishTimeIsGreater && yFinishTimeIsGreater))
            {
                return true;
            }

            // An unfinished competitor has finish time 0, so it cannot have a higher finish time.
            if ((!yHasFinished && yFinishTimeIsGreater) ||
                (!xHasFinished && xFinishTimeIsGreater))
            {
                return true;
            }

            // An unfinished competitor has overrun time 0, so it cannot have a higher overrun time.
            if ((!yHasFinished && yOverrunTimeIsGreater) ||
                (!xHasFinished && xOverrunTimeIsGreater))
            {
                return true;
            }

            // An unfinished competitor has penalty time 0, so it cannot have a higher penalty time.
            if ((!yHasFinished && yPenaltyTimeIsGreater) ||
                (!xHasFinished && xPenaltyTimeIsGreater))
            {
                return true;
            }

            // Formula: OverrunTime = ( FinishTime - StandardCourseTime ) ranged [0, ->>

            // Because StandardCourseTime is a constant, same finish time implies same overrun time.
            bool hasSameFinishTime = !xFinishTimeIsGreater && !yFinishTimeIsGreater;
            if (hasSameFinishTime && (yOverrunTimeIsGreater || xOverrunTimeIsGreater))
            {
                return true;
            }

            if ((xFinishTimeIsGreater && yOverrunTimeIsGreater) ||
                (yFinishTimeIsGreater && xOverrunTimeIsGreater))
            {
                // Because StandardCourseTime is a constant, greater finish time implies 
                // greater overrun time (or same when zero; lower is not possible).
                return true;
            }

            return false;
        }

        [NotNull]
        private static CompetitionRunResult CreateCompetitorFor([NotNull] string name, bool hasFinished,
            bool isEliminated, bool otherHasFinished, bool penaltyTimeIsGreater, bool overrunTimeIsGreater,
            bool otherPenaltyTimeIsGreater, bool otherOverrunTimeIsGreater, bool finishTimeIsGreater,
            bool competitorNumberIsGreater, bool otherFinishTimeIsGreater, bool otherCompetitorNumberIsGreater)
        {
            int competitorNumber = !competitorNumberIsGreater && !otherCompetitorNumberIsGreater
                ? LowOrSameCompetitorNumber
                : (competitorNumberIsGreater ? HighCompetitorNumber : LowOrSameCompetitorNumber);

            var result = new CompetitionRunResult(new Competitor(competitorNumber, name, name));

            if (isEliminated)
            {
                result = result.ChangeIsEliminated(true);
            }

            if (hasFinished)
            {
                bool requireSameOverrunTime = !overrunTimeIsGreater && !otherOverrunTimeIsGreater;
                TimeSpan overrunTime = requireSameOverrunTime
                    ? (!otherHasFinished ? TimeSpan.Zero : LowOrSameOverrunTime)
                    : overrunTimeIsGreater ? HighOverrunTime : LowOrSameOverrunTime;

                bool requireSamePenaltyTime = !penaltyTimeIsGreater && !otherPenaltyTimeIsGreater;
                TimeSpan penaltyTime = requireSamePenaltyTime
                    ? (!otherHasFinished ? TimeSpan.Zero : LowOrSamePenaltyTime)
                    : penaltyTimeIsGreater ? HighPenaltyTime : LowOrSamePenaltyTime;

                bool requireSameFinishTime = !finishTimeIsGreater && !otherFinishTimeIsGreater;
                TimeSpan extraFinishTime = requireSameFinishTime
                    ? (!otherHasFinished ? TimeSpan.Zero : LowOrSameExtraFinishTime)
                    : finishTimeIsGreater ? HighExtraFinishTime : LowOrSameExtraFinishTime;

                TimeSpan startTime = TimeSpan.FromSeconds(10);

                TimeSpan finishTimeAbsolute = startTime + overrunTime + extraFinishTime;
                if (!otherHasFinished && (requireSameFinishTime || requireSameOverrunTime) && !overrunTimeIsGreater)
                {
                    // Other has not finished, but finish times must be the same.
                }
                else if (otherHasFinished && (finishTimeIsGreater || otherFinishTimeIsGreater) && requireSameOverrunTime)
                {
                    // Both finished, but overrun times must be the same.
                }
                else
                {
                    finishTimeAbsolute += StandardCourseTime;
                }

                result = result.ChangeTimings(new CompetitionRunTimings(
                    new RecordedTimeBuilder()
                        .At(startTime).Build())
                    .ChangeFinishTime(new RecordedTimeBuilder()
                        .At(finishTimeAbsolute).Build()));

                TimeSpan finishTimeElapsed = finishTimeAbsolute - startTime;
                TimeSpan actualOverrunTime = finishTimeElapsed > StandardCourseTime
                    ? finishTimeElapsed - StandardCourseTime
                    : TimeSpan.Zero;

                int fr = (int) (penaltyTime - actualOverrunTime).TotalSeconds;
                int refusals = GetRefusalsFor(fr, isEliminated);
                result = result.ChangeRefusalCount(refusals);

                int faults = fr - refusals;
                result = new FakeCompetitionRunResult(result, faults);
            }

            return result;
        }

        private static int GetRefusalsFor(int fr, bool isEliminated)
        {
            int maxRefusals = isEliminated
                ? CompetitionRunResult.MaxRefusalsValue
                : CompetitionRunResult.MaxRefusalsValue - CompetitionRunResult.RefusalStepSize;

            int refusals = 0;
            int remaining = fr;
            while (remaining >= CompetitionRunResult.RefusalStepSize && refusals < maxRefusals)
            {
                refusals += CompetitionRunResult.RefusalStepSize;
            }
            return refusals;
        }

        private static OrderExpect TranslateComparerResult(int result)
        {
            return result < 0 ? OrderExpect.WinnerIsX : result > 0 ? OrderExpect.WinnerIsY : OrderExpect.IsEven;
        }

        private static void AssertCompetitorsAreCompatibleWithProposedScenario(
            [NotNull] CompetitionRunResult xCompetitor, [NotNull] CompetitionRunResult yCompetitor,
            [NotNull] OrderingScenario scenario, [NotNull] CompetitionClassModel model)
        {
            var xCalculator = new CompetitorAssessmentCalculator(xCompetitor, model);
            var yCalculator = new CompetitorAssessmentCalculator(yCompetitor, model);

            if (scenario[0])
            {
                xCompetitor.HasFinished.Should().BeTrue();
            }
            else
            {
                xCompetitor.HasFinished.Should().BeFalse();
            }
            if (scenario[1])
            {
                xCompetitor.IsEliminated.Should().BeTrue();
            }
            else
            {
                xCompetitor.IsEliminated.Should().BeFalse();
            }
            if (scenario[2])
            {
                yCompetitor.HasFinished.Should().BeTrue();
            }
            else
            {
                yCompetitor.HasFinished.Should().BeFalse();
            }
            if (scenario[3])
            {
                yCompetitor.IsEliminated.Should().BeTrue();
            }
            else
            {
                yCompetitor.IsEliminated.Should().BeFalse();
            }

            if (scenario[4])
            {
                xCalculator.PenaltyTime.Should().BeGreaterThan(yCalculator.PenaltyTime);
            }
            else
            {
                xCalculator.PenaltyTime.Should().BeLessOrEqualTo(yCalculator.PenaltyTime);
            }
            if (scenario[5])
            {
                xCalculator.OverrunTime.Should().BeGreaterThan(yCalculator.OverrunTime);
            }
            else
            {
                xCalculator.OverrunTime.Should().BeLessOrEqualTo(yCalculator.OverrunTime);
            }
            if (scenario[6])
            {
                yCalculator.PenaltyTime.Should().BeGreaterThan(xCalculator.PenaltyTime);
            }
            else
            {
                yCalculator.PenaltyTime.Should().BeLessOrEqualTo(xCalculator.PenaltyTime);
            }
            if (scenario[7])
            {
                yCalculator.OverrunTime.Should().BeGreaterThan(xCalculator.OverrunTime);
            }
            else
            {
                yCalculator.OverrunTime.Should().BeLessOrEqualTo(xCalculator.OverrunTime);
            }

            if (scenario[8])
            {
                xCalculator.FinishTime.Should().BeGreaterThan(yCalculator.FinishTime);
            }
            else
            {
                xCalculator.FinishTime.Should().BeLessOrEqualTo(yCalculator.FinishTime);
            }
            if (scenario[9])
            {
                xCompetitor.Competitor.Number.Should().BeGreaterThan(yCompetitor.Competitor.Number);
            }
            else
            {
                xCompetitor.Competitor.Number.Should().BeLessOrEqualTo(yCompetitor.Competitor.Number);
            }
            if (scenario[10])
            {
                yCalculator.FinishTime.Should().BeGreaterThan(xCalculator.FinishTime);
            }
            else
            {
                yCalculator.FinishTime.Should().BeLessOrEqualTo(xCalculator.FinishTime);
            }
            if (scenario[11])
            {
                yCompetitor.Competitor.Number.Should().BeGreaterThan(xCompetitor.Competitor.Number);
            }
            else
            {
                yCompetitor.Competitor.Number.Should().BeLessOrEqualTo(xCompetitor.Competitor.Number);
            }
        }

        /// <summary>
        /// Enables running with extreme values for Faults (negative or too high), which is needed to be able to generate sample
        /// data for all known scenarios.
        /// </summary>
        private sealed class FakeCompetitionRunResult : CompetitionRunResult
        {
            public FakeCompetitionRunResult([NotNull] CompetitionRunResult source, int fakeFaultCount)
                : base(source.Competitor, source.Timings, fakeFaultCount, source.RefusalCount, source.IsEliminated,
                    source.Placement)
            {
            }
        }
    }
}