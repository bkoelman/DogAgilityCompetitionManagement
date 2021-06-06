using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary>
    /// A simple extension to the <see cref="Graphics" /> class for extended graphic routines, such as for creating rounded rectangles. Please contact:
    /// aaronreginald@yahoo.com for the most recent implementations of this class.
    /// </summary>
    /// <remarks>
    /// <see href="http://www.codeproject.com/Articles/5649/Extended-Graphics-An-implementation-of-Rounded-Rec" />
    /// <see href="http://www.codeproject.com/Articles/38436/Extended-Graphics-Rounded-rectangles-Font-metrics" />
    /// </remarks>
    public static class GraphicsExtensions
    {
        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, RectangleF rectangle, float radius)
        {
            Guard.NotNull(graphics, nameof(graphics));

            using (new SmoothingModeScope(graphics, SmoothingMode.AntiAlias))
            {
                GraphicsPath path = GetRoundedRect(rectangle, radius);
                graphics.DrawPath(pen, path);
            }
        }

        public static void FillRoundedRectangle(this Graphics graphics, Brush brush, RectangleF rectangle, float radius)
        {
            Guard.NotNull(graphics, nameof(graphics));

            using (new SmoothingModeScope(graphics, SmoothingMode.AntiAlias))
            {
                GraphicsPath path = GetRoundedRect(rectangle, radius);
                graphics.FillPath(brush, path);
            }
        }

        private static GraphicsPath GetRoundedRect(RectangleF baseRect, float radius)
        {
            var path = new GraphicsPath();

            // if corner radius is less than or equal to zero,
            // return the original rectangle
            if (radius <= 0.0F)
            {
                path.AddRectangle(baseRect);
                path.CloseFigure();
                return path;
            }

            // if the corner radius is greater than or equal to
            // half the width, or height (whichever is shorter)
            // then return a capsule instead of a lozenge
            if (radius >= Math.Min(baseRect.Width, baseRect.Height) / 2.0)
            {
                return GetCapsule(baseRect);
            }

            // create the arc for the rectangle sides and declare
            // a graphics path object for the drawing
            float diameter = radius * 2.0F;
            var sizeF = new SizeF(diameter, diameter);
            var arc = new RectangleF(baseRect.Location, sizeF);

            // top left arc
            path.AddArc(arc, 180, 90);

            // top right arc
            arc.X = baseRect.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc
            arc.Y = baseRect.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc
            arc.X = baseRect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        private static GraphicsPath GetCapsule(RectangleF baseRect)
        {
            var path = new GraphicsPath();

            try
            {
                float diameter;
                RectangleF arc;

                if (baseRect.Width > baseRect.Height)
                {
                    // return horizontal capsule
                    diameter = baseRect.Height;
                    var sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    path.AddArc(arc, 90, 180);
                    arc.X = baseRect.Right - diameter;
                    path.AddArc(arc, 270, 180);
                }
                else if (baseRect.Width < baseRect.Height)
                {
                    // return vertical capsule
                    diameter = baseRect.Width;
                    var sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    path.AddArc(arc, 180, 180);
                    arc.Y = baseRect.Bottom - diameter;
                    path.AddArc(arc, 0, 180);
                }
                else
                {
                    // return circle
                    path.AddEllipse(baseRect);
                }
            }
            catch (Exception)
            {
                path.AddEllipse(baseRect);
            }
            finally
            {
                path.CloseFigure();
            }

            return path;
        }

        private sealed class SmoothingModeScope : IDisposable
        {
            private readonly Graphics graphics;
            private readonly SmoothingMode mode;

            public SmoothingModeScope(Graphics graphics, SmoothingMode mode)
            {
                this.graphics = graphics;
                this.mode = graphics.SmoothingMode;
                graphics.SmoothingMode = mode;
            }

            public void Dispose()
            {
                graphics.SmoothingMode = mode;
            }
        }
    }
}
