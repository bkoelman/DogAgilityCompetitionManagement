using System;
using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary>
    /// Multimedia items, to be played when an important accomplishment is achieved during a competition run.
    /// </summary>
    /// <remarks>
    /// Deeply immutable by design to allow for safe cross-thread member access.
    /// </remarks>
    public sealed class AlertSource : IDisposable
    {
        public static readonly AlertSource None = new(AlertPictureSourceItem.None, AlertSoundSourceItem.None);

        public AlertPictureSourceItem Picture { get; }
        public AlertSoundSourceItem Sound { get; }

        public AlertSource(AlertPictureSourceItem picture, AlertSoundSourceItem sound)
        {
            Guard.NotNull(picture, nameof(picture));
            Guard.NotNull(sound, nameof(sound));

            Picture = picture;
            Sound = sound;
        }

        public void Dispose()
        {
            Picture.Dispose();
        }
    }
}
