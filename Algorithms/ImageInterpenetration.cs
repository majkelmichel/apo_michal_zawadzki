using System.Drawing;
using Algorithms.Point;

namespace Algorithms;

public static class ImageInterpenetration
{
    public static Bitmap Interpenetrate(Bitmap original, Bitmap second, byte ratio)
    {
        var width = original.Width;
        var height = original.Height;
        
        var result = new Bitmap(width, height);
        
        var secondImageOpacity = ratio / 16.0;
        var originalImageOpacity = (16.0 - ratio) / 16.0;
        
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