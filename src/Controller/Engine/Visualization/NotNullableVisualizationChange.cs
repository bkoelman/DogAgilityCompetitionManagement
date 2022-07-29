using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Visualization;

/// <summary>
/// Represents the base class for visualization changes with a required parameter.
/// </summary>
public abstract class NotNullableVisualizationChange<T> : VisualizationChange
{
    protected T Value { get; }

    protected NotNullableVisualizationChange(T value)
    {
        Guard.NotNull(value, nameof(value));
        Value = value;
    }

    [Pure]
    public override string ToString()
    {
        return $"{GetType().Name} Value: {Value}";
    }
}
