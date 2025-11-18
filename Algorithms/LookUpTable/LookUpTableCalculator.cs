using OpenCvSharp;
using System.Drawing;
using Algorithms.LookUpTable;
using OpenCvSharp.Extensions;

namespace Algorithms;

public class LookUpTableCalculator
{
    public static LookUpTableColor CalculateColor(byte[] pixels, int bytesPerPixel)
    {
        ArgumentNullException.ThrowIfNull(pixels);

        var blue = new byte[256];
        var green = new byte[256];
        var red = new byte[256];

        for (var i = 0; i < pixels.Length; i += bytesPerPixel)
        {
            var b = pixels[i];
            var g = pixels[i + 1];
            var r = pixels[i + 2];
            blue[b]++;
            green[g]++;
            red[r]++;
        }

        var rows = new LookUpTableColorRow[256];

        for (var i = 0; i < 256; i++)
        {
            rows[i] = new LookUpTableColorRow
            {
                Blue = blue[i],
                Green = green[i],
                Red = red[i],
            };
        }

        return new LookUpTableColor
        {
            Rows = rows,
        };
    }

    public static LookUpTableGrayscale CalculateGrayscale(byte[] pixels, int bytesPerPixel)
    {
        ArgumentNullException.ThrowIfNull(pixels);

        var frequencies = new int[256];

        for (var i = 0; i < pixels.Length; i += bytesPerPixel)
        {
            byte grayValue;

            if (bytesPerPixel == 1)
            {
                // obraz jest w skali szarości
                grayValue = pixels[i];
            }
            else if (bytesPerPixel >= 3)
            {
                // RGB/RGBA - konwersja na skalę szarości
                var b = pixels[i];
                var g = pixels[i + 1];
                var r = pixels[i + 2];
                grayValue = (byte)(0.299 *r + 0.587 * g + 0.114 * b);
            }
            else continue;

            frequencies[grayValue]++;
        }

        return new LookUpTableGrayscale
        {
            Rows = frequencies.Select(f => new LookUpTableGrayscaleRow
            {
                Intensity = (byte)f,
            }).ToArray()
        };
    }
}