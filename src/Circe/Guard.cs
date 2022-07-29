using JetBrains.Annotations;
using IsNotNullOnReturn = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace DogAgilityCompetition.Circe;

/// <summary>
/// Provides assertions for preconditions.
/// </summary>
public static class Guard
{
    [AssertionMethod]
    public static void NotNull<T>([IsNotNullOnReturn] [NoEnumeration] T? value, [InvokerParameterName] string name)
    {
        if (ReferenceEquals(value, null))
        {
            throw new ArgumentNullException(name);
        }
    }

    [AssertionMethod]
    public static void NotNullNorEmpty<T>([IsNotNullOnReturn] IEnumerable<T?>? value, [InvokerParameterName] string name)
    {
        NotNull(value, name);

        if (!value.Any())
        {
            throw new ArgumentException($"{name} cannot be empty.", name);
        }
    }

    [AssertionMethod]
    public static void NotNullNorWhiteSpace([IsNotNullOnReturn] string? value, [InvokerParameterName] string name)
    {
        NotNull(value, name);

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{name} cannot be empty or contain only whitespace.", name);
        }
    }

    [AssertionMethod]
    public static void InRangeInclusive(int value, [InvokerParameterName] string name, int minValue, int maxValue)
    {
        if (value < minValue || value > maxValue)
        {
            if (minValue == maxValue)
            {
                throw new ArgumentOutOfRangeException(name, value, $"{name} must be {minValue}.");
            }

            throw new ArgumentOutOfRangeException(name, value, $"{name} must be in range [{minValue}-{maxValue}].");
        }
    }

    [AssertionMethod]
    public static void InRangeInclusive(ulong value, [InvokerParameterName] string name, ulong minValue, ulong maxValue)
    {
        if (value < minValue || value > maxValue)
        {
            if (minValue == maxValue)
            {
                throw new ArgumentOutOfRangeException(name, value, $"{name} must be {minValue}.");
            }

            throw new ArgumentOutOfRangeException(name, value, $"{name} must be in range [{minValue}-{maxValue}].");
        }
    }

    [AssertionMethod]
    public static void GreaterOrEqual(int value, [InvokerParameterName] string name, int minValue)
    {
        if (value < minValue)
        {
            throw new ArgumentOutOfRangeException(name, value, $"{name} must be {minValue} or higher.");
        }
    }

    [AssertionMethod]
    public static void GreaterOrEqual(TimeSpan value, [InvokerParameterName] string name, TimeSpan minValue)
    {
        if (value < minValue)
        {
            throw new ArgumentOutOfRangeException(name, value, $"{name} must be {minValue} or higher.");
        }
    }

    [AssertionMethod]
    public static void LessOrEqual(int value, [InvokerParameterName] string name, int maxValue)
    {
        if (value > maxValue)
        {
            throw new ArgumentOutOfRangeException(name, value, $"{name} must be {maxValue} or lower.");
        }
    }
}
