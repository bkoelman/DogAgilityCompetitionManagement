using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.Engine.Visualization;

/// <summary>
/// Represents the base class for visualization changes with an optional parameter.
/// </summary>
public abstract class NullableVisualizationChange<T> : VisualizationChange
{
    protected T? Value { get; }

    protected NullableVisualizationChange(T? value)
    {
        Value = value;
    }

    [Pure]
    public override string ToString()
    {
        return $"{GetType().Name} Value: {(Value is null ? "(null)" : Value.ToString())}";
    }
}
