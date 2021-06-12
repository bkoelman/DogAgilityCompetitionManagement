using System;
using DogAgilityCompetition.Controller.Engine;

namespace DogAgilityCompetition.Specs.Builders
{
    /// <summary>
    /// Enables composition of <see cref="RecordedTime" /> objects in tests.
    /// </summary>
    public sealed class RecordedTimeBuilder : ITestDataBuilder<RecordedTime>
    {
        private static readonly DateTime FrozenUtcNow = new(2010, 1, 1, 13, 26, 00);
        private TimeSpan hardwareOffset = TimeSpan.FromSeconds(3);

        public RecordedTime Build()
        {
            return new(hardwareOffset, FrozenUtcNow + hardwareOffset);
        }

        public RecordedTimeBuilder At(TimeSpan offset)
        {
            hardwareOffset = offset;
            return this;
        }
    }
}
