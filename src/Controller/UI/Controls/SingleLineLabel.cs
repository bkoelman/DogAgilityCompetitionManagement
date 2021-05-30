using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;

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
            Image? image = Image;

            if (image != null)
            {
                DrawImage(e.Graphics, image, r, RtlTranslateAlignment(ImageAlign));
            }

            IntPtr hdc = e.Graphics.GetHdc();

            try
            {
                // The original code uses internal type DeviceContextHdcScope, which is a 'ref struct'.
                // Because ref structs cannot be boxed, they can't be instantiated through reflection.
                // https://github.com/dotnet/runtime/issues/1955

                Color inColor = Enabled ? ForeColor : reflected.ControlDisabledColor;
                Color outColor = ColorTranslator.FromWin32(reflected.InteropGetNearestColor(hdc, ColorTranslator.ToWin32(inColor)));

                nearestColor = outColor.ToArgb() != inColor.ToArgb() ? outColor : inColor;
            }
            finally
            {
                e.Graphics.ReleaseHdc();
            }

            if (AutoEllipsis)
            {
                Rectangle clientRectangle = ClientRectangle;
                Size preferredSize = GetPreferredSize(new Size(clientRectangle.Width, clientRectangle.Height));
                reflected.ShowToolTip = clientRectangle.Width < preferredSize.Width || clientRectangle.Height < preferredSize.Height;
            }
            else
            {
                reflected.ShowToolTip = false;
            }

            if (UseCompatibleTextRendering)
            {
                using StringFormat format = reflected.CreateStringFormat();

                if (Enabled)
                {
                    using Brush brush = new SolidBrush(nearestColor);
                    e.Graphics.DrawString(Text, Font, brush, r, format);
                }
                else
                {
                    ControlPaint.DrawStringDisabled(e.Graphics, Text, Font, nearestColor, r, format);
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

            var handler = Events[reflected.ControlEventPaint] as PaintEventHandler;
            handler?.Invoke(this, e);
        }

        private sealed class Reflected
        {
            private static readonly MethodInfo AnimateMethod;
            private static readonly MethodInfo LayoutUtilsDeflateRectMethod;
            private static readonly ConstructorInfo InteropHdcConstructor;
            private static readonly MethodInfo InteropGetNearestColorMethod;
            private static readonly MethodInfo ControlDisabledColorPropertyGetMethod;
            private static readonly FieldInfo ShowToolTipField;
            private static readonly MethodInfo CreateStringFormatMethod;
            private static readonly MethodInfo CreateTextFormatFlagsMethod;
            private static readonly MethodInfo TextRendererDisabledTextColorMethod;
            private static readonly FieldInfo ControlEventPaintField;

            private readonly Label owner;

            public Color ControlDisabledColor => (Color)ControlDisabledColorPropertyGetMethod.Invoke(owner, new object[0])!;

            public bool ShowToolTip
            {
                set => ShowToolTipField.SetValue(owner, value);
            }

            public object ControlEventPaint => ControlEventPaintField.GetValue(null)!;

            static Reflected()
            {
                AnimateMethod = Require(typeof(Label).GetMethod("Animate", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null));

                Type layoutUtilsType = Require(typeof(Label).Assembly.GetType("System.Windows.Forms.Layout.LayoutUtils", true));
                LayoutUtilsDeflateRectMethod = Require(layoutUtilsType.GetMethod("DeflateRect", BindingFlags.Public | BindingFlags.Static));

                Type hdcType = Require(typeof(Message).Assembly.GetType("Interop+Gdi32+HDC", true));

                InteropHdcConstructor = Require(hdcType.GetConstructor(new[]
                {
                    typeof(IntPtr)
                }));

                Type gdi32Type = Require(typeof(Message).Assembly.GetType("Interop+Gdi32", true));

                InteropGetNearestColorMethod = Require(gdi32Type.GetMethod("GetNearestColor", BindingFlags.Public | BindingFlags.Static, null, new[]
                {
                    hdcType,
                    typeof(int)
                }, null));

                PropertyInfo controlDisabledColorProperty =
                    Require(typeof(Control).GetProperty("DisabledColor", BindingFlags.NonPublic | BindingFlags.Instance));

                ControlDisabledColorPropertyGetMethod = Require(controlDisabledColorProperty.GetGetMethod(true));

                ShowToolTipField = Require(typeof(Label).GetField("_showToolTip", BindingFlags.NonPublic | BindingFlags.Instance));

                CreateStringFormatMethod = Require(typeof(Label).GetMethod("CreateStringFormat", BindingFlags.NonPublic | BindingFlags.Instance));

                CreateTextFormatFlagsMethod =
                    Require(typeof(Label).GetMethod("CreateTextFormatFlags", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null));

                TextRendererDisabledTextColorMethod =
                    Require(typeof(TextRenderer).GetMethod("DisabledTextColor", BindingFlags.NonPublic | BindingFlags.Static));

                ControlEventPaintField = Require(typeof(Control).GetField("s_paintEvent", BindingFlags.NonPublic | BindingFlags.Static));
            }

            public Reflected(Label owner)
            {
                Guard.NotNull(owner, nameof(owner));
                this.owner = owner;
            }

            private static T Require<T>(T? value)
            {
                if (ReferenceEquals(value, null))
                {
                    throw new Exception("Reflection failure.");
                }

                return value;
            }

            public void Animate()
            {
                AnimateMethod.Invoke(owner, new object[0]);
            }

            public Rectangle LayoutUtilsDeflateRect(Rectangle clientRectangle, Padding padding)
            {
                return (Rectangle)LayoutUtilsDeflateRectMethod.Invoke(null, new object[]
                {
                    clientRectangle,
                    padding
                })!;
            }

            public int InteropGetNearestColor(IntPtr handle, int color)
            {
                object hdc = InteropHdcConstructor.Invoke(new object[]
                {
                    handle
                });

                return (int)InteropGetNearestColorMethod.Invoke(null, new[]
                {
                    hdc,
                    color
                })!;
            }

            public StringFormat CreateStringFormat()
            {
                var result = (StringFormat)CreateStringFormatMethod.Invoke(owner, new object[0])!;
                result.FormatFlags |= StringFormatFlags.NoWrap;
                return result;
            }

            public TextFormatFlags CreateTextFormatFlags()
            {
                var result = (TextFormatFlags)CreateTextFormatFlagsMethod.Invoke(owner, new object[0])!;
                result &= ~TextFormatFlags.WordBreak;
                return result;
            }

            public Color TextRendererDisabledTextColor(Color backColor)
            {
                return (Color)TextRendererDisabledTextColorMethod.Invoke(null, new object[]
                {
                    backColor
                })!;
            }
        }
    }
}
