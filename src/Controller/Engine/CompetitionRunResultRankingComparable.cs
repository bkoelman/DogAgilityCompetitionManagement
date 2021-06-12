using System;
using System.Diagnostics.CodeAnalysis;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Storage;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary>
    /// Implementation of sorting competitor run results.
    /// </summary>
    [SuppressMessage("Design", "CA1036:Override methods on comparable types")]
    public sealed class CompetitionRunResultRankingComparable : IComparable<CompetitionRunResultRankingComparable>
    {
        private const int WinnerIsThis = -1;
        private const int WinnerIsOther = 1;
        private const int NoDifference = 0;

        private readonly CompetitionRunResult runResult;
        private readonly RankingComparisonMode comparisonMode;
        private readonly CompetitorAssessmentCalculator calculator;

        public CompetitionRunResultRankingComparable(CompetitionRunResult runResult, CompetitionClassModel model, RankingComparisonMode comparisonMode)
        {
            Guard.NotNull(runResult, nameof(runResult));
            Guard.NotNull(model, nameof(model));

            this.runResult = runResult;
            this.comparisonMode = comparisonMode;
            calculator = new CompetitorAssessmentCalculator(runResult, model);
        }

        public int CompareWithoutNumberTo(CompetitionRunResultRankingComparable? other)
        {
            if (other != null && CompetitionRunResult.AreEquivalentRun(runResult, other.runResult))
            {
                return NoDifference;
            }

            return CompareTo(other);
        }

        public int CompareTo(CompetitionRunResultRankingComparable? other)
        {
            if (other == null)
            {
                return WinnerIsThis;
            }

            int result;

            switch (comparisonMode)
            {
                case RankingComparisonMode.OnlyPhaseCompletion:
                    result = ComparePhaseCompletion(other);
                    break;
                case RankingComparisonMode.OnlyPhasePenaltyOverrun:
                    result = ComparePhasePenaltyOverrun(other);
                    break;
                case RankingComparisonMode.OnlyPhaseFinishNumber:
                    result = ComparePhaseFinishNumber(other);
                    break;
                default:
                    result = ComparePhaseCompletion(other);

                    if (result == 0)
                    {
                        result = ComparePhasePenaltyOverrun(other);

                        if (result == 0)
                        {
                            result = ComparePhaseFinishNumber(other);
                        }
                    }

                    break;
            }

            return result;
        }

        private int ComparePhaseCompletion(CompetitionRunResultRankingComparable other)
        {
            bool bit0 = runResult.HasFinished;
            bool bit1 = runResult.IsEliminated;
            bool bit2 = other.runResult.HasFinished;
            bool bit3 = other.runResult.IsEliminated;

            bool isEvenTerm1 = !bit0 && !bit1 && !bit2 && !bit3;
            bool isEvenTerm2 = !bit0 && bit1 && !bit2 && bit3;
            bool isEvenTerm3 = bit0 && bit1 && bit2 && bit3;
            bool isEvenTerm4 = bit0 && !bit1 && bit2 && !bit3;

            bool matchesIsEven = isEvenTerm1 || isEvenTerm2 || isEvenTerm3 || isEvenTerm4;

            if (matchesIsEven)
            {
                return NoDifference;
            }

            bool winnerIsYTerm1 = !bit0 && bit3;
            bool winnerIsYTerm2 = !bit0 && bit2;
            bool winnerIsYTerm3 = bit1 && bit2;

            bool matchesWinnerIsY = winnerIsYTerm1 || winnerIsYTerm2 || winnerIsYTerm3;

            return matchesWinnerIsY ? WinnerIsOther : WinnerIsThis;
        }

        private int ComparePhasePenaltyOverrun(CompetitionRunResultRankingComparable other)
        {
            bool bit0 = calculator.PenaltyTime > other.calculator.PenaltyTime;
            bool bit1 = calculator.OverrunTime > other.calculator.OverrunTime;
            bool bit2 = other.calculator.PenaltyTime > calculator.PenaltyTime;
            bool bit3 = other.calculator.OverrunTime > calculator.OverrunTime;

            bool matchesIsEven = !bit0 && !bit1 && !bit2 && !bit3;

            if (matchesIsEven)
            {
                return NoDifference;
            }

            bool winnerIsYTerm1 = !bit2 && bit3;
            bool winnerIsYTerm2 = bit0;

            bool matchesWinnerIsY = winnerIsYTerm1 || winnerIsYTerm2;

            return matchesWinnerIsY ? WinnerIsOther : WinnerIsThis;
        }

        private int ComparePhaseFinishNumber(CompetitionRunResultRankingComparable other)
        {
            bool bit0 = calculator.FinishTime > other.calculator.FinishTime;
            //bool bit1 = runResult.Competitor.Number > other.runResult.Competitor.Number;
            bool bit2 = other.calculator.FinishTime > calculator.FinishTime;
            bool bit3 = other.runResult.Competitor.Number > runResult.Competitor.Number;

            bool winnerIsYTerm1 = bit0;
            bool winnerIsYTerm2 = !bit2 && !bit3;

            bool matchesWinnerIsY = winnerIsYTerm1 || winnerIsYTerm2;

            return matchesWinnerIsY ? WinnerIsOther : WinnerIsThis;
        }

        [Pure]
        public override string ToString()
        {
            return $"{GetType().Name}: HasFinished={runResult.HasFinished}, Eliminated={runResult.IsEliminated}, " +
                $"PenaltyTime={calculator.PenaltyTime}, FaultRefusalTime={calculator.FaultRefusalTime}, " +
                $"OverrunTime={calculator.OverrunTime}, FinishTime={calculator.FinishTime}, Number={runResult.Competitor.Number}";
        }

        public static bool operator <(CompetitionRunResultRankingComparable? left, CompetitionRunResultRankingComparable? right)
        {
            return left == null ? right != null : left.CompareTo(right) == -1;
        }

        public static bool operator <=(CompetitionRunResultRankingComparable? left, CompetitionRunResultRankingComparable? right)
        {
            return left == null || left.CompareTo(right) <= 0;
        }

        public static bool operator >(CompetitionRunResultRankingComparable? left, CompetitionRunResultRankingComparable? right)
        {
            return left?.CompareTo(right) == 1;
        }

        public static bool operator >=(CompetitionRunResultRankingComparable? left, CompetitionRunResultRankingComparable? right)
        {
            return left == null ? right == null : left.CompareTo(right) >= 0;
        }
    }
}
