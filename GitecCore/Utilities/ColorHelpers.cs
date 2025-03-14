using System;
using System.Drawing;
using System.Globalization;

namespace Gitec.Utilities
{
    public static class ColorHelpers
    {
        /// <summary>
        /// Converts a hex color string to a System.Drawing.Color.
        /// Supports #RRGGBB and #RRGGBBAA formats.
        /// </summary>
        /// <param name="hexColor">Hex color string (e.g., "#FF5733" or "#FF5733CC").</param>
        /// <returns>Corresponding Color object.</returns>
        /// <exception cref="ArgumentException">Thrown if the hex string is invalid.</exception>
        public static Color ConvertTo(string hexColor)
        {
            if (string.IsNullOrWhiteSpace(hexColor))
                throw new ArgumentException("Hex color cannot be null or empty.", nameof(hexColor));

            hexColor = hexColor.TrimStart('#');

            if (hexColor.Length is not (6 or 8))
                throw new ArgumentException("Hex color must be in the format #RRGGBB or #RRGGBBAA.", nameof(hexColor));

            byte r = byte.Parse(hexColor[..2], NumberStyles.HexNumber);
            byte g = byte.Parse(hexColor[2..4], NumberStyles.HexNumber);
            byte b = byte.Parse(hexColor[4..6], NumberStyles.HexNumber);
            byte a = (hexColor.Length == 8) ? byte.Parse(hexColor[6..8], NumberStyles.HexNumber) : (byte)255;

            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// Converts a System.Drawing.Color to a hex color string.
        /// Includes alpha if not fully opaque.
        /// </summary>
        /// <param name="color">Color object to convert.</param>
        /// <returns>Hex color string in "#RRGGBB" or "#RRGGBBAA" format.</returns>
        public static string ConvertTo(Color color)
        {
            return color.A == 255
                ? $"#{color.R:X2}{color.G:X2}{color.B:X2}"
                : $"#{color.R:X2}{color.G:X2}{color.B:X2}{color.A:X2}";
        }
    }
}