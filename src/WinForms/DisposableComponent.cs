using System.ComponentModel;
using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.WinForms;

/// <summary>
/// Wraps a disposable object that gets disposed when its parent WinForms form closes.
/// </summary>
public sealed class DisposableComponent<T> : IComponent
    where T : class, IDisposable
{
    public T Component { get; }

    public ISite? Site
    {
        get => null;
        set
        {
        }
    }

    public event EventHandler? Disposed;

    public DisposableComponent(T disposable, ref IContainer? container)
    {
        Guard.NotNull(disposable, nameof(disposable));

        Component = disposable;

        // The container is managed by the Forms designer and instantiated only when the form/control
        // contains components. Because we are not a component the designer knows about, we need to
        // ensure a container ourselves in order for Dispose to be called from generated code.
        container ??= new Container();

        container.Add(this);
    }

    public void Dispose()
    {
        Component.Dispose();
        Disposed?.Invoke(this, EventArgs.Empty);
    }
}
