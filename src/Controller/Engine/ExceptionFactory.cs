using System;

namespace DogAgilityCompetition.Controller.Engine
{
    public static class ExceptionFactory
    {
        public static NotSupportedException CreateNotSupportedExceptionFor<T>(T enumValue)
            where T : struct
        {
            return new($"Unsupported {typeof(T).Name} '{enumValue}'.");
        }
    }
}
