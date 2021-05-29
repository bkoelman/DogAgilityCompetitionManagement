using System.Drawing;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Controls.Shapes
{
    /// <summary>
    /// Represents the base class for drawable shapes.
    /// </summary>
    public abstract class Shape
    {
        protected const int ShadowOffset = 2;

        public ShapeState State { get; set; }

        public virtual void DrawShadow([NotNull] Graphics graphics)
        {
        }

        public virtual void DrawFill([NotNull] Graphics graphics)
        {
        }

        public virtual void DrawBorder([NotNull] Graphics graphics)
        {
        }
    }
}
