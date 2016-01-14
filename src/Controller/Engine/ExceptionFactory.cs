using System;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine
{
    public static class ExceptionFactory
    {
        [NotNull]
        public static NotSupportedException CreateNotSupportedExceptionFor<T>(T enumValue) where T : struct
        {
            return new NotSupportedException($"Unsupported {typeof (T).Name} '{enumValue}'.");
        }
    }
}