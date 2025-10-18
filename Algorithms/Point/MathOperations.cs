using System.Drawing;

namespace Algorithms.Point;

public class MathOperations
{
    public static Bitmap AddImages(Bitmap[] images, bool saturate = false)
    {
        if (!(images.Length > 0)) throw new ArgumentException("Images must be at least 1");
        
        var width = images[0].Width;
        var height = images[0].Height;

        if (!images.All(img => img.Width == width && img.Height == height)) throw new ArgumentException("Images are not of the same size");

        var result = new Bitmap(width, height);

        var divider = saturate ? 1 : images.Length;
        for (var y = 0; y < width; y++)
        {
            for (var x = 0; x < height; x++)
            {
                var pixelSum = Math.Min(images.Sum(image => image.GetPixel(x, y).R / divider), 255);

                result.SetPixel(x, y, Color.FromArgb(pixelSum, pixelSum, pixelSum));
            }
        }

        return result;
    }
}