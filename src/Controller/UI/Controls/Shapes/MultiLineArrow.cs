using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Controls.Shapes
{
    /// <summary>
    /// Represents a drawable sequence of horizontal/vertical lines that ends in an arrow.
    /// </summary>
    public sealed class MultiLineArrow : ArrowShape
    {
        [NotNull]
        private readonly PointF[] arrowPoints;

        [NotNull]
        private readonly PointF[] shadowPoints;

        private MultiLineArrow([NotNull] IEnumerable<PointF> points)
        {
            Guard.NotNull(points, nameof(points));

            arrowPoints = points.ToArray();
            shadowPoints =
                arrowPoints.Select(point => new PointF(point.X + ShadowOffset, point.Y + ShadowOffset)).ToArray();
        }

        public override void DrawShadow(Graphics graphics)
        {
            Guard.NotNull(graphics, nameof(graphics));

            using (var path = new GraphicsPath())
            {
                using (var arrowPen = new Pen(Brushes.Gray, 6) { EndCap = LineCap.ArrowAnchor })
                {
                    path.AddLines(shadowPoints);
                    graphics.DrawPath(arrowPen, path);
                }
            }
        }

        public override void DrawFill(Graphics graphics)
        {
            Guard.NotNull(graphics, nameof(graphics));

            using (var path = new GraphicsPath())
            {
                using (Brush fillBrush = new SolidBrush(GetColorForState()))
                {
                    using (var arrowPen = new Pen(fillBrush, 6) { EndCap = LineCap.ArrowAnchor })
                    {
                        path.AddLines(arrowPoints);
                        graphics.DrawPath(arrowPen, path);
                    }
                }
            }
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

        internal sealed class Builder
        {
            [NotNull]
            private readonly IList<PointF> arrowPoints;

            public Builder(PointF start)
            {
                arrowPoints = new[] { start }.ToList();
            }

            [NotNull]
            public Builder Up(float length)
            {
                return Down(-length);
            }

            [NotNull]
            public Builder Down(float length)
            {
                PointF lastPoint = arrowPoints.Last();
                arrowPoints.Add(new PointF(lastPoint.X, lastPoint.Y + length));
                return this;
            }

            [NotNull]
            public Builder Left(float length)
            {
                return Right((-length));
            }

            [NotNull]
            public Builder Right(float length)
            {
                PointF lastPoint = arrowPoints.Last();
                arrowPoints.Add(new PointF(lastPoint.X + length, lastPoint.Y));
                return this;
            }

            [NotNull]
            public MultiLineArrow Build()
            {
                return new MultiLineArrow(arrowPoints);
            }
        }
    }
}