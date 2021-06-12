using System.Drawing;
using System.Drawing.Drawing2D;
using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Controller.UI.Controls.Shapes
{
    /// <summary>
    /// Represents a drawable block of text.
    /// </summary>
    public sealed class TextBlock : Shape
    {
        private readonly string text;

        private readonly RectangleF boxRect;
        private readonly PointF topLeft;
        private readonly RectangleF shadowRect;

        private float LeftOffsetX => boxRect.Width * 1 / 4;
        private float CenterOffsetX => boxRect.Width / 2;
        private float RightOffsetX => boxRect.Width * 3 / 4;

        private float TopOffsetY => boxRect.Height * 1 / 4;
        private float CenterOffsetY => boxRect.Height / 2;
        private float BottomOffsetY => boxRect.Height * 3 / 4;

        //    +-TopLeft------TopCenter-----TopRight-+
        //    |                                     |
        // LeftTop                              RightTop
        //    |                                     |
        // LeftMiddle                           RightMiddle
        //    |                                     |
        // LeftBottom                           RightBottom
        //    |                                     |
        //    +-BottomLeft-BottomCenter-BottomRight-+

        public PointF TopLeftConnection => new(boxRect.X + LeftOffsetX, boxRect.Y);

        public PointF TopCenterConnection => new(boxRect.X + CenterOffsetX, boxRect.Y);

        public PointF TopRightConnection => new(boxRect.X + RightOffsetX, boxRect.Y);

        public PointF LeftTopConnection => new(boxRect.X, boxRect.Y + TopOffsetY);

        public PointF LeftMiddleConnection => new(boxRect.X, boxRect.Y + CenterOffsetY);

        public PointF LeftBottomConnection => new(boxRect.X, boxRect.Y + BottomOffsetY);

        public PointF BottomLeftConnection => new(boxRect.X + LeftOffsetX, boxRect.Y + boxRect.Height);

        public PointF BottomCenterConnection => new(boxRect.X + CenterOffsetX, boxRect.Y + boxRect.Height);

        public PointF BottomRightConnection => new(boxRect.X + RightOffsetX, boxRect.Y + boxRect.Height);

        public PointF RightTopConnection => new(boxRect.X + boxRect.Width, boxRect.Y + TopOffsetY);

        public PointF RightMiddleConnection => new(boxRect.X + boxRect.Width, boxRect.Y + CenterOffsetY);

        public PointF RightBottomConnection => new(boxRect.X + boxRect.Width, boxRect.Y + BottomOffsetY);

        public Font Font { get; set; }

        public TextBlock(string text, Font font, RectangleF rectangle)
        {
            Guard.NotNullNorWhiteSpace(text, nameof(text));
            Guard.NotNull(font, nameof(font));

            this.text = text;
            Font = font;
            boxRect = rectangle;
            topLeft = new PointF(rectangle.Left, rectangle.Top);
            shadowRect = new RectangleF(new PointF(rectangle.X + ShadowOffset, rectangle.Y + ShadowOffset), rectangle.Size);
        }

        public override void DrawShadow(Graphics graphics)
        {
            graphics.FillRoundedRectangle(Brushes.Gray, shadowRect, 7);
        }

        public override void DrawFill(Graphics graphics)
        {
            using Brush fillBrush = GetBrushForFill();
            graphics.FillRoundedRectangle(fillBrush, boxRect, 7);
        }

        public override void DrawBorder(Graphics graphics)
        {
            Guard.NotNull(graphics, nameof(graphics));

            using var fontCopy = new Font(Font, State == ShapeState.Selected ? FontStyle.Bold : FontStyle.Regular);

            graphics.DrawString(text, fontCopy, Brushes.Black, new RectangleF(topLeft, boxRect.Size), new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
                FormatFlags = StringFormatFlags.NoWrap
            });

            graphics.DrawRoundedRectangle(Pens.Black, boxRect, 7);
        }

        private Brush GetBrushForFill()
        {
            var topRight = new PointF(boxRect.X + boxRect.Size.Width, boxRect.Y);
            return new LinearGradientBrush(topLeft, topRight, Color.White, GetColorForState());
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
