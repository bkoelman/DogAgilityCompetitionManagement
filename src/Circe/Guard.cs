using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe
{
    /// <summary>
    /// Provides assertions for preconditions.
    /// </summary>
    public static class Guard
    {
        [AssertionMethod]
        [ContractAnnotation("value: null => halt")]
        public static void NotNull<T>([CanBeNull] [NoEnumeration] T value, [NotNull] [InvokerParameterName] string name)
        {
            if (ReferenceEquals(value, null))
            {
                throw new ArgumentNullException(name);
            }
        }

        [AssertionMethod]
        [ContractAnnotation("value: null => halt")]
        public static void NotNullNorEmpty<T>([CanBeNull] [ItemCanBeNull] IEnumerable<T> value,
            [NotNull] [InvokerParameterName] string name)
        {
            NotNull(value, name);

            if (!value.Any())
            {
                throw new ArgumentException(name + @" cannot be empty.", name);
            }
        }

        [AssertionMethod]
        [ContractAnnotation("value: null => halt")]
        public static void NotNullNorWhiteSpace([CanBeNull] string value, [NotNull] [InvokerParameterName] string name)
        {
            NotNull(value, name);

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(name + @" cannot be empty or contain only whitespace.", name);
            }
        }

        [AssertionMethod]
        public static void InRangeInclusive(int value, [NotNull] [InvokerParameterName] string name, int minValue,
            int maxValue)
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
        public static void InRangeInclusive(ulong value, [NotNull] [InvokerParameterName] string name, ulong minValue,
            ulong maxValue)
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
        public static void GreaterOrEqual(int value, [NotNull] [InvokerParameterName] string name, int minValue)
        {
            if (value < minValue)
            {
                throw new ArgumentOutOfRangeException(name, value, $"{name} must be {minValue} or higher.");
            }
        }

        [AssertionMethod]
        public static void GreaterOrEqual(TimeSpan value, [NotNull] [InvokerParameterName] string name,
            TimeSpan minValue)
        {
            if (value < minValue)
            {
                throw new ArgumentOutOfRangeException(name, value, $"{name} must be {minValue} or higher.");
            }
        }

        [AssertionMethod]
        public static void LessOrEqual(int value, [NotNull] [InvokerParameterName] string name, int maxValue)
        {
            if (value > maxValue)
            {
                throw new ArgumentOutOfRangeException(name, value, $"{name} must be {maxValue} or lower.");
            }
        }
    }
}