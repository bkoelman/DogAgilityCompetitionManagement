using FluentAssertions;

namespace DogAgilityCompetition.Specs.Facilities
{
    public static class AssertionsExtensions
    {
        public static TSubject ShouldNotBeNull<TSubject>(this TSubject? source, string? because = "", params object?[]? reasonArgs)
        {
            source.Should().NotBeNull(because, reasonArgs);
            return source!;
        }
    }
}
