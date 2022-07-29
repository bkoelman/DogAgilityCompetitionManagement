using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Storage;

namespace DogAgilityCompetition.Controller.Engine;

/// <summary>
/// A comparer to support sorting competitor run results.
/// </summary>
public sealed class CompetitionRunResultRankingComparer : IComparer<CompetitionRunResult>
{
    private readonly CompetitionClassModel modelSnapshot;
    private readonly RankingComparisonMode comparisonMode;

    public CompetitionRunResultRankingComparer(CompetitionClassModel modelSnapshot, RankingComparisonMode comparisonMode)
    {
        Guard.NotNull(modelSnapshot, nameof(modelSnapshot));
        this.modelSnapshot = modelSnapshot;
        this.comparisonMode = comparisonMode;
    }

    public int Compare(CompetitionRunResult? x, CompetitionRunResult? y)
    {
        Guard.NotNull(x, nameof(x));
        Guard.NotNull(y, nameof(y));

        var xComparable = new CompetitionRunResultRankingComparable(x, modelSnapshot, comparisonMode);
        var yComparable = new CompetitionRunResultRankingComparable(y, modelSnapshot, comparisonMode);

        return xComparable.CompareTo(yComparable);
    }

    public bool IsPlacementEqual(CompetitionRunResult left, CompetitionRunResult right)
    {
        Guard.NotNull(left, nameof(left));
        Guard.NotNull(right, nameof(right));

        var xComparable = new CompetitionRunResultRankingComparable(left, modelSnapshot, comparisonMode);
        var yComparable = new CompetitionRunResultRankingComparable(right, modelSnapshot, comparisonMode);

        return xComparable.CompareWithoutNumberTo(yComparable) == 0;
    }
}
