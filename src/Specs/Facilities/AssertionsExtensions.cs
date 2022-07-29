using FluentAssertions;
using IsNotNullOnReturn = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace DogAgilityCompetition.Specs.Facilities;

public static class AssertionsExtensions
{
#pragma warning disable 8777 // Parameter 'source' must have a non-null value when exiting.
    public static void ShouldNotBeNull<TSubject>([IsNotNullOnReturn] this TSubject? source, string? because = "", params object?[]? reasonArgs)
    {
        source.Should().NotBeNull(because, reasonArgs);
    }
#pragma warning restore 8777
}
