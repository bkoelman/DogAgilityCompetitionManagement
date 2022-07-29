namespace DogAgilityCompetition.Circe;

/// <summary />
public sealed class EventArgs<T> : EventArgs
{
    public T Argument { get; }

    public EventArgs(T argument)
    {
        Guard.NotNull(argument, nameof(argument));

        Argument = argument;
    }
}
