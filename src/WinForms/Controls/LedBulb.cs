using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.WinForms.Controls
{
    /// <summary>
    /// The LEDBulb is a .Net control for Windows Forms that emulates an LED light with two states On and Off.  The purpose of the control is to provide a
    /// sleek looking representation of an LED light that is sizable, has a transparent background and can be set to different colors.
    /// </summary>
    /// <remarks>
    /// Based on <see href="http://www.codeproject.com/Articles/114122/A-Simple-Vector-Based-LED-User-Control" />
    /// </remarks>
    public sealed class LedBulb : Control
    {
        private readonly Color reflectionColor = Color.FromArgb(180, 255, 255, 255);

        [NotNull]
        private readonly Color[] surroundColors =
        {
            Color.FromArgb(0, 255, 255, 255)
        };

        [NotNull]
        private readonly Timer timer = new();

        private Color color;
        private bool isOn = true;

        /// <summary>
        /// Gets or sets the color of the LED light.
        /// </summary>
        [DefaultValue(typeof(Color), "153, 255, 54")]
        public Color Color
        {
            get => color;
            set
            {
                color = value;
                DarkColor = ControlPaint.Dark(color);
                DarkDarkColor = ControlPaint.DarkDark(color);
                Invalidate(); // Redraw the control.
            }
        }

        /// <summary>
        /// Dark shade of the LED color used for gradient.
        /// </summary>
        public Color DarkColor { get; private set; }

        /// <summary>
        /// Very dark shade of the LED color used for gradient.
        /// </summary>
        public Color DarkDarkColor { get; private set; }

        /// <summary>
        /// Gets or Sets whether the light is turned on.
        /// </summary>
        [DefaultValue(typeof(bool), "True")]
        public bool On
        {
            get => isOn;
            set
            {
                isOn = value;
                Invalidate();
            }
        }

        public LedBulb()
        {
            SetStyle(
                ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint |
                ControlStyles.SupportsTransparentBackColor, true);

            Color = Color.FromArgb(255, 153, 255, 54);

            timer.Tick += (_, _) => On = !On;
        }

        /// <summary>
        /// Handles the Paint event for this UserControl.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            Guard.NotNull(e, nameof(e));

            // Create an off-screen graphics object for double buffering.
            using var offScreenBmp = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            using Graphics g = Graphics.FromImage(offScreenBmp);

            g.SmoothingMode = SmoothingMode.HighQuality;
            // Draw the control.
            DrawControl(g, On);
            // Draw the image to the screen.
            e.Graphics.DrawImageUnscaled(offScreenBmp, 0, 0);
        }

        /// <summary>
        /// Renders the control to an image.
        /// </summary>
        private void DrawControl([NotNull] Graphics g, bool on)
        {
            // Is the bulb on or off?
            Color lightColor = on ? Color : Color.FromArgb(150, DarkColor);
            Color darkColor = on ? DarkColor : DarkDarkColor;

            // Calculate the dimensions of the bulb.
            int width = Width - (Padding.Left + Padding.Right);
            int height = Height - (Padding.Top + Padding.Bottom);
            // Diameter is the lesser of width and height.
            int diameter = Math.Min(width, height);
            // Subtract 1 pixel so ellipse doesn't get cut off.
            diameter = Math.Max(diameter - 1, 1);

            // Draw the background ellipse.
            var rectangle = new Rectangle(Padding.Left, Padding.Top, diameter, diameter);

            using (var darkBrush = new SolidBrush(darkColor))
            {
                g.FillEllipse(darkBrush, rectangle);
            }

            // Draw the glow gradient.
            using (var glowPath = new GraphicsPath())
            {
                glowPath.AddEllipse(rectangle);

                using (var pathBrush = new PathGradientBrush(glowPath)
                {
                    CenterColor = lightColor,
                    SurroundColors = new[]
                    {
                        Color.FromArgb(0, lightColor)
                    }
                })
                {
                    g.FillEllipse(pathBrush, rectangle);

                    // Draw the white reflection gradient.
                    int offset = Convert.ToInt32(diameter * .15F);
                    int diameter1 = Convert.ToInt32(rectangle.Width * .8F);
                    var whiteRect = new Rectangle(rectangle.X - offset, rectangle.Y - offset, diameter1, diameter1);

                    using (var whitePath = new GraphicsPath())
                    {
                        whitePath.AddEllipse(whiteRect);
                    }

                    using (var glowBrush = new PathGradientBrush(glowPath)
                    {
                        CenterColor = reflectionColor,
                        SurroundColors = surroundColors
                    })
                    {
                        g.FillEllipse(glowBrush, whiteRect);
                    }
                }
            }

            // Draw the border.
            g.SetClip(ClientRectangle);

            if (On)
            {
                using var borderPen = new Pen(Color.FromArgb(85, Color.Black), 1F);
                g.DrawEllipse(borderPen, rectangle);
            }
        }

        /// <summary>
        /// Causes the Led to start blinking.
        /// </summary>
        /// <param name="milliseconds">
        /// Number of milliseconds to blink for. 0 stops blinking.
        /// </param>
        public void Blink(int milliseconds)
        {
            if (milliseconds > 0)
            {
                On = true;
                timer.Interval = milliseconds;
                timer.Enabled = true;
            }
            else
            {
                timer.Enabled = false;
                On = false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                timer.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
