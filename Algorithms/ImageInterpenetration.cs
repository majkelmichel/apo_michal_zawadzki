using System.Drawing;

namespace Algorithms;

public static class ImageInterpenetration
{
    public const double Ratio = 16.0;
    public static Bitmap Interpenetrate(Bitmap original, Bitmap second, byte ratio)
    {
        if (ratio > Ratio) throw new ArgumentException($"Ratio cannot be greater than {Ratio}");
        if (!ColorDepth.IsGrayscale(original) || !ColorDepth.IsGrayscale(second)) throw new ArgumentException("Images must be grayscale");
        
        var width = original.Width;
        var height = original.Height;
        
        var result = new Bitmap(width, height);
        
        var secondImageOpacity = ratio / Ratio;
        var originalImageOpacity = (Ratio - ratio) / Ratio;
        
        for (var y = 0; y < width; y++)
        {
            for (var x = 0; x < height; x++)
            {
                var pixelSum = (byte)(original.GetPixel(x, y).R * originalImageOpacity + second.GetPixel(x, y).R * secondImageOpacity);

                result.SetPixel(x, y, Color.FromArgb(pixelSum, pixelSum, pixelSum));
            }
        }

        return result;
    }
}