using System;
using System.Drawing;

namespace DogAgilityCompetition.Controller.UI.Controls
{
    /// <summary>
    /// Converts colors between different color models.
    /// </summary>
    public static class ColorConverter
    {
        /// <summary>
        /// Creates a <see cref="Color" /> in ARGB format from the specified alpha, hue, saturation and lightness values.
        /// </summary>
        /// <param name="alpha">
        /// The alpha value, as returned by <see cref="Color.A" />.
        /// </param>
        /// <param name="hue">
        /// The hue, as returned by <see cref="Color.GetHue" />.
        /// </param>
        /// <param name="saturation">
        /// The saturation, as returned by <see cref="Color.GetSaturation" />.
        /// </param>
        /// <param name="lightness">
        /// The lightness, as returned by <see cref="Color.GetBrightness" />.
        /// </param>
        /// <returns>
        /// The color in ARGB format that is equivalent to the specified values.
        /// </returns>
        /// <remarks>
        /// Uses the CSS Color Module Level 3 algorithm, which is described at <see href="http://www.w3.org/TR/css3-color/#hsl-color" />.
        /// </remarks>
        public static Color FromAhsl(byte alpha, double hue, double saturation, double lightness)
        {
            if (hue < 0.0 || hue > 360.0)
            {
                throw new ArgumentOutOfRangeException(nameof(hue), hue, "hue must be in range [0-360].");
            }

            if (saturation < 0.0 || saturation > 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(saturation), saturation, "saturation must be in range [0-1].");
            }

            if (lightness < 0.0 || lightness > 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(lightness), lightness, "lightness must be in range [0-1].");
            }

            // System.Drawing.Color returns hue in degrees (0 - 360) rather than a number between 0 and 1.
            double hueCorrected = hue / 360.0;

            double mid2 = lightness <= .5 ? lightness * (saturation + 1.0) : lightness + saturation - lightness * saturation;
            double mid1 = lightness * 2.0 - mid2;

            double red = HueToRgb(mid1, mid2, hueCorrected + 1.0 / 3.0);
            double green = HueToRgb(mid1, mid2, hueCorrected);
            double blue = HueToRgb(mid1, mid2, hueCorrected - 1.0 / 3.0);

            byte redByte = Convert.ToByte(red * 255.0);
            byte greenByte = Convert.ToByte(green * 255.0);
            byte blueByte = Convert.ToByte(blue * 255.0);
            return Color.FromArgb(alpha, redByte, greenByte, blueByte);
        }

        private static double HueToRgb(double mid1, double mid2, double hue)
        {
            if (hue < 0.0)
            {
                hue++;
            }

            if (hue > 1.0)
            {
                hue--;
            }

            if (hue * 6.0 < 1.0)
            {
                return mid1 + (mid2 - mid1) * hue * 6.0;
            }

            if (hue * 2.0 < 1.0)
            {
                return mid2;
            }

            if (hue * 3.0 < 2.0)
            {
                return mid1 + (mid2 - mid1) * (2.0 / 3.0 - hue) * 6.0;
            }

            return mid1;
        }
    }
}
