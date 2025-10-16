using System.Windows.Media.Imaging;

namespace Algorithms.Histogram;

public static class HistogramCalculator
{
    public static HistogramData CalculateGrayscale(byte[] pixels, int width, int height, int bytesPerPixel)
    {
        ArgumentNullException.ThrowIfNull(pixels);

        var frequencies = new int[256];
        var totalPixels = 0;
        long sum = 0;

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
            sum += grayValue;
            totalPixels++;
        }

        var mean = totalPixels > 0 ? (double)sum / totalPixels : 0;

        double variance = 0;
        for (var i = 0; i < 256; i++)
        {
            if (frequencies[i] > 0)
            {
                variance += frequencies[i] * Math.Pow(i - mean, 2);
            }
        }

        var standardDeviation = totalPixels > 0 ? Math.Sqrt(variance / totalPixels) : 0;

        var min = 0;
        var max = 255;
        for (var i = 0; i < 256; i++)
        {
            if (frequencies[i] <= 0) continue;
            min = i;
            break;
        }

        for (var i = 255; i >= 0; i--)
        {
            if (frequencies[i] <= 0) continue;
            max = i;
            break;
        }

        var median = CalculateMedia(frequencies, totalPixels);

        return new HistogramData
        {
            Frequencies = frequencies,
            Mean = mean,
            StandardDeviation = standardDeviation,
            Median = median,
            Max = max,
            Min = min,
            TotalPixels = totalPixels
        };
    }

    private static double CalculateMedia(int[] frequencies, int totalPixels)
    {
        var halfPixels = totalPixels / 2;
        var count = 0;

        for (var i = 0; i < 256; i++)
        {
            count += frequencies[i];
            if (count >= halfPixels) return i;
        }

        return 0;
    }
}