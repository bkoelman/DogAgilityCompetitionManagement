using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary>
    /// Represents a <see cref="System.Windows.Forms.Label" /> whose text is always scaled and stretched to occupy the entire control area.
    /// </summary>
    public sealed class ScaleTextToFitLabel : Label
    {
        private static readonly PointF TopLeftPoint = new(0, 0);

        private RectangleF previousTextBounds;

        [Browsable(false)]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool AutoSize
        {
            get => false;
            set
            {
            }
        }

        [Browsable(false)]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ContentAlignment TextAlign
        {
            get => ContentAlignment.TopLeft;
            set
            {
            }
        }

        [Browsable(false)]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool AutoEllipsis => false;

        public bool EnableStabilization { get; set; }

        public ScaleTextToFitLabel()
        {
            EnableStabilization = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Guard.NotNull(e, nameof(e));

            Rectangle clientBounds = Reflected.DeflateRect(ClientRectangle, Padding);

            if (string.IsNullOrWhiteSpace(Text))
            {
                e.Graphics.Clear(BackColor);

                if (Image != null)
                {
                    Rectangle imageBounds = Rectangle.Round(clientBounds);
                    DrawImage(e.Graphics, Image, imageBounds, RtlTranslateAlignment(ImageAlign));
                }
            }
            else
            {
                using StringFormat stringFormat = Reflected.GetStringFormat(this);
                using var textPath = new GraphicsPath();

                textPath.AddString(Text, Font.FontFamily, (int)Font.Style, clientBounds.Height, TopLeftPoint, stringFormat);

                PointF[] transformPoints =
                {
                    new(clientBounds.Left, clientBounds.Top),
                    new(clientBounds.Right, clientBounds.Top),
                    new(clientBounds.Left, clientBounds.Bottom)
                };

                RectangleF textBounds = Stabilize(textPath.GetBounds());
                e.Graphics.Transform = new Matrix(textBounds, transformPoints);

                e.Graphics.Clear(BackColor);
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

                if (Image != null)
                {
                    Rectangle imageBounds = Rectangle.Round(textBounds);
                    DrawImage(e.Graphics, Image, imageBounds, RtlTranslateAlignment(ImageAlign));
                }

                using (var brush = new SolidBrush(Enabled ? ForeColor : ControlPaint.LightLight(ForeColor)))
                {
                    e.Graphics.FillPath(brush, textPath);
                }

                e.Graphics.ResetTransform();
            }
        }

        private RectangleF Stabilize(RectangleF rectangle)
        {
            if (EnableStabilization)
            {
                int epsilonX = ClientSize.Width / 50;
                int epsilonY = ClientSize.Height / 50;

                if (Math.Abs(previousTextBounds.X - rectangle.X) <= epsilonX && Math.Abs(previousTextBounds.Width - rectangle.Width) <= epsilonX &&
                    Math.Abs(previousTextBounds.Y - rectangle.Y) <= epsilonY && Math.Abs(previousTextBounds.Height - rectangle.Height) <= epsilonY)
                {
                    return previousTextBounds;
                }
            }

            previousTextBounds = rectangle;
            return rectangle;
        }

        private static class Reflected
        {
            [NotNull]
            private static readonly MethodInfo DeflateRectMethod;

            [NotNull]
            private static readonly MethodInfo CreateStringFormatMethod;

            static Reflected()
            {
                Assembly assembly = typeof(Label).Assembly;
                Type type = assembly.GetType("System.Windows.Forms.Layout.LayoutUtils", true);
                DeflateRectMethod = type.GetMethod("DeflateRect", BindingFlags.Public | BindingFlags.Static);

                CreateStringFormatMethod = typeof(Label).GetMethod("CreateStringFormat", BindingFlags.NonPublic | BindingFlags.Instance);
            }

            public static Rectangle DeflateRect(Rectangle rect, Padding padding)
            {
                return (Rectangle)DeflateRectMethod.Invoke(null, new object[]
                {
                    rect,
                    padding
                });
            }

            [NotNull]
            public static StringFormat GetStringFormat([NotNull] Control target)
            {
                return (StringFormat)CreateStringFormatMethod.Invoke(target, new object[0]);
            }
        }
    }
}
