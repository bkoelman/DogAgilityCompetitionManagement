using System;
using DogAgilityCompetition.Controller.Engine;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Specs.Builders
{
    /// <summary>
    /// Enables composition of <see cref="RecordedTime" /> objects in tests.
    /// </summary>
    public sealed class RecordedTimeBuilder : ITestDataBuilder<RecordedTime>
    {
        private static readonly DateTime FrozenUtcNow = new DateTime(2010, 1, 1, 13, 26, 00);
        private TimeSpan hardwareOffset = TimeSpan.FromSeconds(3);

        public RecordedTime Build()
        {
            return new RecordedTime(hardwareOffset, FrozenUtcNow + hardwareOffset);
        }

        [NotNull]
        public RecordedTimeBuilder At(TimeSpan offset)
        {
            hardwareOffset = offset;
            return this;
        }
    }
}