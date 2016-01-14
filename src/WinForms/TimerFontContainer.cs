using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.WinForms.Properties;
using JetBrains.Annotations;

namespace DogAgilityCompetition.WinForms
{
    /// <summary>
    /// Loads a TrueType font file from an embedded resource and assigns it to a <see cref="Label" />. This circumvents the
    /// need to install the font in Windows first.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",
        Justification = "This class registers itself on the containing Form/Control, which disposes it.")]
    public sealed class TimerFontContainer
    {
        [NotNull]
        private readonly DisposableComponent<PrivateFontCollection> fontCollectionWrapper;

        public TimerFontContainer([CanBeNull] ref IContainer components)
        {
            fontCollectionWrapper = new DisposableComponent<PrivateFontCollection>(new PrivateFontCollection(),
                ref components);

            InitializeTimerFont();
        }

        private void InitializeTimerFont()
        {
            byte[] fontBuffer = Resources.digital_7__mono_;
            unsafe
            {
                fixed (byte* pFontData = fontBuffer)
                {
                    uint dummy = 0;
                    fontCollectionWrapper.Component.AddMemoryFont((IntPtr) pFontData, fontBuffer.Length);
                    NativeMethods.AddFontMemResourceEx((IntPtr) pFontData, (uint) fontBuffer.Length, IntPtr.Zero,
                        ref dummy);
                }
            }
        }

        public void ApplyTo([ItemNotNull] [NotNull] params Label[] labels)
        {
            Guard.NotNull(labels, nameof(labels));

            foreach (Label label in labels)
            {
                label.Font = new Font(fontCollectionWrapper.Component.Families[0], label.Font.Size);
            }
        }

        private static class NativeMethods
        {
            [DllImport("gdi32.dll")]
            public static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv,
                [In] ref uint pcFonts);
        }
    }
}