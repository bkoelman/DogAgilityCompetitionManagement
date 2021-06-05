using DogAgilityCompetition.Circe.Protocol;
using FluentAssertions;
using Xunit;

namespace DogAgilityCompetition.Specs.RankingSpecs
{
    /// <summary>
    /// Basic tests for ordering of competitor run results.
    /// </summary>
    public sealed class ComparisonOperators
    {
        [Fact]
        public void WirelessNetworkAddress()
        {
            // Arrange
            WirelessNetworkAddress? w0 = null;
            var w1 = new WirelessNetworkAddress("111111");
            var w2 = new WirelessNetworkAddress("222222");

            // Act and assert
            (w1 <= w2).Should().BeTrue();
            (w2 >= w1).Should().BeTrue();
            (!(w1 >= w2)).Should().BeTrue();
            (!(w2 <= w1)).Should().BeTrue();

            // ReSharper disable ExpressionIsAlwaysNull
            (w1 >= w0).Should().BeTrue();
            (w0 <= w1).Should().BeTrue();
            (!(w1 <= w0)).Should().BeTrue();
            (!(w0 >= w1)).Should().BeTrue();
            // ReSharper restore ExpressionIsAlwaysNull

#pragma warning disable CS1718 // Comparison made to same variable
            // ReSharper disable EqualExpressionComparison
            (w1 <= w1).Should().BeTrue();
            (w1 >= w1).Should().BeTrue();
            (!(w1 > w1)).Should().BeTrue();
            (!(w1 < w1)).Should().BeTrue();

            // ReSharper disable ExpressionIsAlwaysNull
            (w0 <= w0).Should().BeTrue();
            (w0 >= w0).Should().BeTrue();
            (!(w0 > w0)).Should().BeTrue();
            (!(w0 < w0)).Should().BeTrue();
            // ReSharper restore ExpressionIsAlwaysNull
            // ReSharper restore EqualExpressionComparison
#pragma warning restore CS1718 // Comparison made to same variable
        }
    }
}
