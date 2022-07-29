using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.WinForms.Properties;

namespace DogAgilityCompetition.WinForms;

/// <summary>
/// Loads a TrueType font file from an embedded resource and assigns it to a <see cref="Label" />. This circumvents the need to install the font in
/// Windows first.
/// </summary>
[SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable",
    Justification = "This registers a callback to the allocated object.")]
public sealed class TimerFontContainer
{
    private readonly DisposableComponent<PrivateFontCollection> fontCollectionWrapper;

    public TimerFontContainer(ref IContainer? components)
    {
        fontCollectionWrapper = new DisposableComponent<PrivateFontCollection>(new PrivateFontCollection(), ref components);

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
                fontCollectionWrapper.Component.AddMemoryFont((IntPtr)pFontData, fontBuffer.Length);
                NativeMethods.AddFontMemResourceEx((IntPtr)pFontData, (uint)fontBuffer.Length, IntPtr.Zero, ref dummy);
            }
        }
    }

    public void ApplyTo(params Label[] labels)
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
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        public static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);
    }
}
