using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary>
    /// Represents a picture, to be shown when an important accomplishment is achieved during a competition run.
    /// </summary>
    /// <remarks>
    /// Deeply immutable by design to allow for safe cross-thread member access.
    /// </remarks>
    public sealed class AlertPictureSourceItem : AlertSourceItem, IDisposable
    {
        [NotNull]
        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [NotNull]
        public static readonly AlertPictureSourceItem None = new AlertPictureSourceItem(false, null);

        [CanBeNull]
        private Bitmap item;

        [CanBeNull]
        public Bitmap EffectiveItem => !IsEnabled ? null : item;

        public AlertPictureSourceItem(bool isEnabled, [CanBeNull] string picturePath)
            : base(isEnabled, picturePath)
        {
            SafePictureLoad();
        }

        private void SafePictureLoad()
        {
            if (Path != null)
            {
                try
                {
                    item = LoadBitmapWithoutHoldingFileLock(Path);
                }
                catch (Exception ex)
                {
                    Log.Warn($"Failed to load picture at '{Path}'.", ex);
                }
            }
        }

        [NotNull]
        private static Bitmap LoadBitmapWithoutHoldingFileLock([NotNull] string imagePath)
        {
            var bitmapStream = new MemoryStream();

            using (FileStream fileStream = File.OpenRead(imagePath))
            {
                fileStream.CopyTo(bitmapStream);
            }

            return new Bitmap(bitmapStream);
        }

        public void Dispose()
        {
            if (item != null)
            {
                item.Dispose();
                item = null;
            }
        }
    }
}