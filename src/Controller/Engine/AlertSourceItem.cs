using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Controller.Engine;

/// <summary>
/// A media source, to be played when an important accomplishment is achieved during a competition run.
/// </summary>
/// <remarks>
/// Deeply immutable by design to allow for safe cross-thread member access.
/// </remarks>
public abstract class AlertSourceItem
{
    public bool IsEnabled { get; }
    public string? Path { get; }

    public string? EffectivePath => !IsEnabled ? null : Path;

    protected AlertSourceItem(bool isEnabled, string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            if (isEnabled)
            {
                Guard.NotNullNorEmpty(path, nameof(path));
            }
        }
        else
        {
            Path = path;
        }

        IsEnabled = isEnabled;
    }
}
