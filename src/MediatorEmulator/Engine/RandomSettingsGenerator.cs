using System;
using JetBrains.Annotations;

namespace DogAgilityCompetition.MediatorEmulator.Engine
{
    /// <summary>
    /// Provides pseudo-random values.
    /// </summary>
    public sealed class RandomSettingsGenerator
    {
        [NotNull]
        private readonly Random randomizer = new Random();

        public int GetSignalStrength()
        {
            return randomizer.Next(100, 250);
        }

        public int GetBatteryStatus()
        {
            return randomizer.Next(100, 250);
        }
    }
}