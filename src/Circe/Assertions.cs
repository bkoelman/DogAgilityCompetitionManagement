using System;
using JetBrains.Annotations;
using IsNotNullOnReturn = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace DogAgilityCompetition.Circe
{
    /// <summary />
    public static class Assertions
    {
        [AssertionMethod]
        public static void IsNotNull<T>([IsNotNullOnReturn] [NoEnumeration] T? value, [InvokerParameterName] string name)
        {
            if (value is null)
            {
                throw new InvalidOperationException($"Unexpected internal error: {name} is null.");
            }
        }
    }
}
