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
    /// Basic tests for ordering of competitor run results.
    /// </summary>
    [TestFixture]
    public sealed class RunResultOrdering
    {
        [Test]
        public void RunCompletion1()
        {
            // Arrange
            var scenarios = new List<OrderingScenario>
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

            var comparer = new CompetitionRunResultRankingComparer(new CompetitionClassModel(),
                RankingComparisonMode.OnlyPhaseCompletion);

            foreach (OrderingScenario scenario in scenarios.Where(s => s.Result != OrderExpect.DoNotCare))
            {
                CompetitionRunResult xCompetitor = CreateCompetitorForCompletion(scenario[0], scenario[1]);
                CompetitionRunResult yCompetitor = CreateCompetitorForCompletion(scenario[2], scenario[3]);

                // Act
                int result = comparer.Compare(xCompetitor, yCompetitor);

                // Assert
                OrderExpect actual = TranslateComparerResult(result);
                actual.Should().Be(scenario.Result, scenario.ToString());
            }
        }

        private static OrderExpect TranslateComparerResult(int result)
        {
            return result < 0 ? OrderExpect.WinnerIsX : result > 0 ? OrderExpect.WinnerIsY : OrderExpect.IsEven;
        }

        [NotNull]
        private static CompetitionRunResult CreateCompetitorForCompletion(bool hasFinished, bool isEliminated)
        {
            var result = new CompetitionRunResult(new Competitor(1, "A", "A"));

            if (isEliminated)
            {
                result = result.ChangeIsEliminated(true);
            }

            if (hasFinished)
            {
                result = result.ChangeTimings(new CompetitionRunTimings(
                    new RecordedTimeBuilder()
                        .At(TimeSpan.FromSeconds(10)).Build())
                    .ChangeFinishTime(new RecordedTimeBuilder()
                        .At(TimeSpan.FromSeconds(25)).Build()));
            }

            return result;
        }

        [Test]
        public void PenaltyOverrun2()
        {
            // Arrange
            var scenarios = new List<OrderingScenario>
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

            CompetitionClassModel model = new CompetitionClassModel()
                .ChangeClassInfo(new CompetitionClassInfo()
                    .ChangeStandardCourseTime(StandardCourseTime));
            var comparer = new CompetitionRunResultRankingComparer(model, RankingComparisonMode.OnlyPhasePenaltyOverrun);

            foreach (OrderingScenario scenario in scenarios.Where(s => s.Result != OrderExpect.DoNotCare))
            {
                CompetitionRunResult xCompetitor = CreateCompetitorForPenaltyOverrun(scenario[0], scenario[1]);
                CompetitionRunResult yCompetitor = CreateCompetitorForPenaltyOverrun(scenario[2], scenario[3]);

                AssertCompetitorsAreCompatibleWithProposedScenario(xCompetitor, yCompetitor, scenario, model);

                // Act
                int result = comparer.Compare(xCompetitor, yCompetitor);

                // Assert
                OrderExpect actual = TranslateComparerResult(result);
                actual.Should().Be(scenario.Result, scenario.ToString());
            }
        }

        private static void AssertCompetitorsAreCompatibleWithProposedScenario(
            [NotNull] CompetitionRunResult xCompetitor,
            [NotNull] CompetitionRunResult yCompetitor, [NotNull] OrderingScenario scenario,
            [NotNull] CompetitionClassModel model)
        {
            var xCalculator = new CompetitorAssessmentCalculator(xCompetitor, model);
            var yCalculator = new CompetitorAssessmentCalculator(yCompetitor, model);

            if (scenario[0])
            {
                xCalculator.PenaltyTime.Should().BeGreaterThan(yCalculator.PenaltyTime);
            }
            else
            {
                xCalculator.PenaltyTime.Should().BeLessOrEqualTo(yCalculator.PenaltyTime);
            }

            if (scenario[1])
            {
                xCalculator.OverrunTime.Should().BeGreaterThan(yCalculator.OverrunTime);
            }
            else
            {
                xCalculator.OverrunTime.Should().BeLessOrEqualTo(yCalculator.OverrunTime);
            }

            if (scenario[2])
            {
                yCalculator.PenaltyTime.Should().BeGreaterThan(xCalculator.PenaltyTime);
            }
            else
            {
                yCalculator.PenaltyTime.Should().BeLessOrEqualTo(xCalculator.PenaltyTime);
            }

            if (scenario[3])
            {
                yCalculator.OverrunTime.Should().BeGreaterThan(xCalculator.OverrunTime);
            }
            else
            {
                yCalculator.OverrunTime.Should().BeLessOrEqualTo(xCalculator.OverrunTime);
            }
        }

        private static readonly TimeSpan LowOverrunTime = TimeSpan.FromSeconds(5);
        private static readonly TimeSpan HighOverrunTime = TimeSpan.FromSeconds(10);
        private static readonly TimeSpan LowPenaltyTime = TimeSpan.FromSeconds(70);
        private static readonly TimeSpan HighPenaltyTime = TimeSpan.FromSeconds(80);

        private const int LowCompetitorNumber = 3;
        private const int HighCompetitorNumber = 7;

        private static readonly TimeSpan StandardCourseTime = TimeSpan.FromSeconds(40);

        [NotNull]
        private static CompetitionRunResult CreateCompetitorForPenaltyOverrun(bool penaltyTimeIsGreater,
            bool overrunTimeIsGreater)
        {
            TimeSpan overrunTime = overrunTimeIsGreater ? HighOverrunTime : LowOverrunTime;
            TimeSpan penaltyTime = penaltyTimeIsGreater ? HighPenaltyTime : LowPenaltyTime;

            var result = new CompetitionRunResult(new Competitor(1, "A", "A"));

            TimeSpan startTime = TimeSpan.FromSeconds(10);
            result = result.ChangeTimings(new CompetitionRunTimings(
                new RecordedTimeBuilder()
                    .At(startTime).Build())
                .ChangeFinishTime(new RecordedTimeBuilder()
                    .At(startTime + overrunTime + StandardCourseTime).Build()));

            int fr = (int) (penaltyTime - overrunTime).TotalSeconds;
            result = result.ChangeFaultCount(fr);

            return result;
        }

        [Test]
        public void FinishNumber3()
        {
            // Arrange
            var scenarios = new List<OrderingScenario>
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

            var comparer = new CompetitionRunResultRankingComparer(new CompetitionClassModel(),
                RankingComparisonMode.OnlyPhaseFinishNumber);

            foreach (OrderingScenario scenario in scenarios.Where(s => s.Result != OrderExpect.DoNotCare))
            {
                CompetitionRunResult xCompetitor = CreateCompetitorForFinishWithNumber(scenario[0], scenario[1]);
                CompetitionRunResult yCompetitor = CreateCompetitorForFinishWithNumber(scenario[2], scenario[3]);

                // Act
                int result = comparer.Compare(xCompetitor, yCompetitor);

                // Assert
                OrderExpect actual = TranslateComparerResult(result);
                actual.Should().Be(scenario.Result, scenario.ToString());
            }
        }

        [NotNull]
        private static CompetitionRunResult CreateCompetitorForFinishWithNumber(bool finishTimeIsGreater,
            bool competitorNumberIsGreater)
        {
            int competitorNumber = competitorNumberIsGreater ? HighCompetitorNumber : LowCompetitorNumber;
            TimeSpan finishTime = TimeSpan.FromSeconds(finishTimeIsGreater ? 50 : 25);

            var result = new CompetitionRunResult(new Competitor(competitorNumber, "A", "A"));

            TimeSpan startTime = TimeSpan.FromSeconds(10);
            result = result.ChangeTimings(new CompetitionRunTimings(
                new RecordedTimeBuilder()
                    .At(startTime).Build())
                .ChangeFinishTime(new RecordedTimeBuilder()
                    .At(startTime + finishTime).Build()));

            return result;
        }
    }
}