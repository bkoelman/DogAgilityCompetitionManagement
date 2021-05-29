using System.Collections.Generic;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Controller.Engine.Visualization;
using DogAgilityCompetition.WinForms;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI
{
    /// <summary>
    /// Applies visualization changes on a WinForms form/control on the UI thread.
    /// </summary>
    public sealed class WinFormsVisualizer : ICompetitionRunVisualizer
    {
        [NotNull]
        private readonly Control invokeContext;

        [NotNull]
        private readonly IVisualizationActor target;

        public WinFormsVisualizer([NotNull] Control invokeContext, [NotNull] IVisualizationActor target)
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
}
