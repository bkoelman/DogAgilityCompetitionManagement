using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary>
    /// A <see cref="Label" /> with text that spans a single line (does not wrap).
    /// </summary>
    public sealed class SingleLineLabel : Label
    {
        // The only thing we want to accomplish is:
        // - Add StringFormat.FormatFlags.NoWrap when using CompatibleTextRendering
        // - Remove TextFormatFlags.WordBreak when not using CompatibleTextRendering
        // But to accomplish this, a lot of code duplication from System.Windows.Forms.Label is needed.

        [NotNull]
        private readonly Reflected reflected;

        public SingleLineLabel()
        {
            reflected = new Reflected(this);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Guard.NotNull(e, nameof(e));

            Color nearestColor;
            reflected.Animate();
            ImageAnimator.UpdateFrames(Image);
            Rectangle r = reflected.LayoutUtilsDeflateRect(ClientRectangle, Padding);
            Image image = Image;
            if (image != null)
            {
                DrawImage(e.Graphics, image, r, RtlTranslateAlignment(ImageAlign));
            }
            IntPtr hdc = e.Graphics.GetHdc();
            try
            {
                using (IDisposable graphics = reflected.WindowsGraphicsFromHdc(hdc))
                {
                    nearestColor = reflected.WindowsGraphicsGetNearestColor(graphics,
                        Enabled ? ForeColor : reflected.ControlDisabledColor);
                }
            }
            finally
            {
                e.Graphics.ReleaseHdc();
            }
            if (AutoEllipsis)
            {
                Rectangle clientRectangle = ClientRectangle;
                Size preferredSize = GetPreferredSize(new Size(clientRectangle.Width, clientRectangle.Height));
                reflected.ShowToolTip = (clientRectangle.Width < preferredSize.Width) ||
                    (clientRectangle.Height < preferredSize.Height);
            }
            else
            {
                reflected.ShowToolTip = false;
            }

            if (UseCompatibleTextRendering)
            {
                using (StringFormat format = reflected.CreateStringFormat())
                {
                    if (Enabled)
                    {
                        using (Brush brush = new SolidBrush(nearestColor))
                        {
                            e.Graphics.DrawString(Text, Font, brush, r, format);
                        }
                    }
                    else
                    {
                        ControlPaint.DrawStringDisabled(e.Graphics, Text, Font, nearestColor, r, format);
                    }
                }
            }
            else
            {
                TextFormatFlags flags = reflected.CreateTextFormatFlags();
                if (Enabled)
                {
                    TextRenderer.DrawText(e.Graphics, Text, Font, r, nearestColor, flags);
                }
                else
                {
                    Color foreColor = reflected.TextRendererDisabledTextColor(BackColor);
                    TextRenderer.DrawText(e.Graphics, Text, Font, r, foreColor, flags);
                }
            }

            var handler = (PaintEventHandler) Events[reflected.ControlEventPaint];
            handler?.Invoke(this, e);
        }

        private sealed class Reflected
        {
            [NotNull]
            private static readonly MethodInfo AnimateMethod;

            [NotNull]
            private static readonly MethodInfo LayoutUtilsDeflateRectMethod;

            [NotNull]
            private static readonly MethodInfo WindowsGraphicsFromHdcMethod;

            [NotNull]
            private static readonly MethodInfo WindowsGraphicsGetNearestColorMethod;

            [NotNull]
            private static readonly MethodInfo ControlDisabledColorPropertyGetMethod;

            [NotNull]
            private static readonly FieldInfo ShowToolTipField;

            [NotNull]
            private static readonly MethodInfo CreateStringFormatMethod;

            [NotNull]
            private static readonly MethodInfo CreateTextFormatFlagsMethod;

            [NotNull]
            private static readonly MethodInfo TextRendererDisabledTextColorMethod;

            [NotNull]
            private static readonly FieldInfo ControlEventPaintField;

            [NotNull]
            private readonly Label owner;

            static Reflected()
            {
                AnimateMethod =
                    Require(typeof (Label).GetMethod("Animate", BindingFlags.NonPublic | BindingFlags.Instance, null,
                        new Type[0], null));

                Type layoutUtilsType =
                    Require(typeof (Label).Assembly.GetType("System.Windows.Forms.Layout.LayoutUtils", true));
                LayoutUtilsDeflateRectMethod =
                    Require(layoutUtilsType.GetMethod("DeflateRect", BindingFlags.Public | BindingFlags.Static));

                Type windowsGraphicsType =
                    Require(typeof (Label).Assembly.GetType("System.Windows.Forms.Internal.WindowsGraphics", true));
                WindowsGraphicsFromHdcMethod =
                    Require(windowsGraphicsType.GetMethod("FromHdc", BindingFlags.Public | BindingFlags.Static));
                WindowsGraphicsGetNearestColorMethod =
                    Require(windowsGraphicsType.GetMethod("GetNearestColor", BindingFlags.Public | BindingFlags.Instance));

                PropertyInfo controlDisabledColorProperty =
                    Require(typeof (Control).GetProperty("DisabledColor", BindingFlags.NonPublic | BindingFlags.Instance));
                ControlDisabledColorPropertyGetMethod = Require(controlDisabledColorProperty.GetGetMethod(true));

                ShowToolTipField =
                    Require(typeof (Label).GetField("showToolTip", BindingFlags.NonPublic | BindingFlags.Instance));

                CreateStringFormatMethod =
                    Require(typeof (Label).GetMethod("CreateStringFormat",
                        BindingFlags.NonPublic | BindingFlags.Instance));

                CreateTextFormatFlagsMethod =
                    Require(typeof (Label).GetMethod("CreateTextFormatFlags",
                        BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null));

                TextRendererDisabledTextColorMethod =
                    Require(typeof (TextRenderer).GetMethod("DisabledTextColor",
                        BindingFlags.NonPublic | BindingFlags.Static));

                ControlEventPaintField =
                    Require(typeof (Control).GetField("EventPaint", BindingFlags.NonPublic | BindingFlags.Static));
            }

            [NotNull]
            private static T Require<T>([CanBeNull] T value)
            {
                if (ReferenceEquals(value, null))
                {
                    throw new Exception("Reflection failure.");
                }
                return value;
            }

            public Reflected([NotNull] Label owner)
            {
                Guard.NotNull(owner, nameof(owner));
                this.owner = owner;
            }

            public Color ControlDisabledColor
                => (Color) ControlDisabledColorPropertyGetMethod.Invoke(owner, new object[0]);

            public bool ShowToolTip
            {
                set
                {
                    ShowToolTipField.SetValue(owner, value);
                }
            }

            [CanBeNull]
            public object ControlEventPaint => ControlEventPaintField.GetValue(null);

            public void Animate()
            {
                AnimateMethod.Invoke(owner, new object[0]);
            }

            public Rectangle LayoutUtilsDeflateRect(Rectangle clientRectangle, Padding padding)
            {
                return (Rectangle) LayoutUtilsDeflateRectMethod.Invoke(null, new object[] { clientRectangle, padding });
            }

            [NotNull]
            public IDisposable WindowsGraphicsFromHdc(IntPtr hdc)
            {
                return (IDisposable) WindowsGraphicsFromHdcMethod.Invoke(null, new object[] { hdc });
            }

            public Color WindowsGraphicsGetNearestColor([NotNull] IDisposable graphics, Color color)
            {
                return (Color) WindowsGraphicsGetNearestColorMethod.Invoke(graphics, new object[] { color });
            }

            [NotNull]
            public StringFormat CreateStringFormat()
            {
                var result = (StringFormat) CreateStringFormatMethod.Invoke(owner, new object[0]);
                result.FormatFlags |= StringFormatFlags.NoWrap;
                return result;
            }

            public TextFormatFlags CreateTextFormatFlags()
            {
                var result = (TextFormatFlags) CreateTextFormatFlagsMethod.Invoke(owner, new object[0]);
                result &= ~TextFormatFlags.WordBreak;
                return result;
            }

            public Color TextRendererDisabledTextColor(Color backColor)
            {
                return (Color) TextRendererDisabledTextColorMethod.Invoke(null, new object[] { backColor });
            }
        }
    }
}