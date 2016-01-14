using System;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

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
        [NotNull]
        public static readonly AlertSource None = new AlertSource(AlertPictureSourceItem.None, AlertSoundSourceItem.None);

        [NotNull]
        public AlertPictureSourceItem Picture { get; }

        [NotNull]
        public AlertSoundSourceItem Sound { get; private set; }

        public AlertSource([NotNull] AlertPictureSourceItem picture, [NotNull] AlertSoundSourceItem sound)
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