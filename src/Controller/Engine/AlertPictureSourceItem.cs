using System.Reflection;
using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Controller.Engine;

/// <summary>
/// Represents a picture, to be shown when an important accomplishment is achieved during a competition run.
/// </summary>
/// <remarks>
/// Deeply immutable by design to allow for safe cross-thread member access.
/// </remarks>
public sealed class AlertPictureSourceItem : AlertSourceItem, IDisposable
{
    private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod()!.DeclaringType!);

    public static readonly AlertPictureSourceItem None = new(false, null);

    private Bitmap? item;

    public Bitmap? EffectiveItem => !IsEnabled ? null : item;

    public AlertPictureSourceItem(bool isEnabled, string? picturePath)
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

    private static Bitmap LoadBitmapWithoutHoldingFileLock(string imagePath)
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
