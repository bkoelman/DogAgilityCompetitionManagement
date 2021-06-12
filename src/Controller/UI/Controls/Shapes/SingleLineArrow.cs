using System.Drawing;
using System.Drawing.Drawing2D;
using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Controller.UI.Controls.Shapes
{
    /// <summary>
    /// Represents a drawable vertical line that ends in an arrow.
    /// </summary>
    public sealed class SingleLineArrow : ArrowShape
    {
        private readonly PointF topLeft;
        private readonly PointF bottomRight;

        public SingleLineArrow(PointF topLeft, PointF bottomRight)
        {
            this.topLeft = topLeft;
            this.bottomRight = bottomRight;
        }

        public override void DrawShadow(Graphics graphics)
        {
            Guard.NotNull(graphics, nameof(graphics));

            using var pen = new Pen(Brushes.Gray, 6)
            {
                EndCap = LineCap.ArrowAnchor
            };

            var shadowTopLeft = new PointF(topLeft.X + ShadowOffset, topLeft.Y + ShadowOffset);
            var shadowBottomRight = new PointF(bottomRight.X + ShadowOffset, bottomRight.Y + ShadowOffset);
            graphics.DrawLine(pen, shadowTopLeft, shadowBottomRight);
        }

        public override void DrawFill(Graphics graphics)
        {
            Guard.NotNull(graphics, nameof(graphics));

            using Brush fillBrush = new SolidBrush(GetColorForState());

            using var arrowPen = new Pen(fillBrush, 6)
            {
                EndCap = LineCap.ArrowAnchor
            };

            graphics.DrawLine(arrowPen, topLeft, bottomRight);
        }

        private Color GetColorForState()
        {
            switch (State)
            {
                case ShapeState.Candidate:
                    return Color.FromArgb(224, 255, 194);
                case ShapeState.Selected:
                    return Color.Yellow;
                case ShapeState.Disabled:
                    return Color.LightGray;
                default:
                    return Color.FromArgb(255, 224, 194);
            }
        }
    }
}
