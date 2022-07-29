using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Visualization;
using DogAgilityCompetition.WinForms;

namespace DogAgilityCompetition.Controller.UI;

/// <summary>
/// Applies visualization changes on a WinForms form/control on the UI thread.
/// </summary>
public sealed class WinFormsVisualizer : ICompetitionRunVisualizer
{
    private readonly Control invokeContext;
    private readonly IVisualizationActor target;

    public WinFormsVisualizer(Control invokeContext, IVisualizationActor target)
    {
        Guard.NotNull(invokeContext, nameof(invokeContext));
        Guard.NotNull(target, nameof(target));

        this.invokeContext = invokeContext;
        this.target = target;
    }

    public void Apply(IReadOnlyCollection<VisualizationChange> changes)
    {
        invokeContext.EnsureOnMainThread(() =>
        {
            foreach (VisualizationChange change in changes)
            {
                change.ApplyTo(target);
            }
        });
    }
}
