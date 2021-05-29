using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary>
    /// Represents a sound, to be played when an important accomplishment is achieved during a competition run.
    /// </summary>
    /// <remarks>
    /// Deeply immutable by design to allow for safe cross-thread member access.
    /// </remarks>
    public sealed class AlertSoundSourceItem : AlertSourceItem
    {
        [NotNull]
        public static readonly AlertSoundSourceItem None = new(false, null);

        public AlertSoundSourceItem(bool isEnabled, [CanBeNull] string soundPath)
            : base(isEnabled, soundPath)
        {
        }
    }
}
