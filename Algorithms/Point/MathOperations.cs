using System.Drawing;

namespace Algorithms.Point;

public class MathOperations
{
    // Lab 2 Zad 1
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

    public static Bitmap SubtractImages(Bitmap image1, Bitmap image2)
    {
        if (image1.Width != image2.Width || image1.Height != image2.Height) throw new ArgumentException("Images are not of the same size");
        
        var result = new Bitmap(image1.Width, image1.Height);
        
        for (var y = 0; y < image1.Height; y++)
        {
            for (var x = 0; x < image1.Width; x++)
            {
                var pixel1 = image1.GetPixel(x, y);
                var pixel2 = image2.GetPixel(x, y);

                var newPixel = Math.Max(pixel1.R - pixel2.R, 0);
                
                result.SetPixel(x, y, Color.FromArgb(newPixel, newPixel, newPixel));
            }
        }

        return result;
    }

    public static class Number
    {
        // Lab 2 Zad 1
        public static byte[] Add(byte value, bool saturate = false, int maxValueInImage = 0)
        {
            if (!saturate)
            {
                var dividend = 1;
                if (value + maxValueInImage > 255) dividend = (value + maxValueInImage) / 255 + 1;
                return Enumerable.Range(0, 256).Select(i => (byte)Math.Min(255, i + value / dividend)).ToArray();
            }
            return Enumerable.Range(0, 256).Select(i => (byte)Math.Min(255, i + value)).ToArray();
        }

        // Lab 2 Zad 1
        public static byte[] Multiply(byte value, bool saturate = false)
        {
            if (!saturate) return Enumerable.Range(0, 256).Select(i => (byte)Math.Min(255, i / value * value)).ToArray();
            return Enumerable.Range(0, 256).Select(i => (byte)Math.Min(255, i * value)).ToArray();
        }

        // Lab 2 Zad 1
        public static byte[] Divide(int value, bool saturate = false)
        {
            return Enumerable.Range(0, 256).Select(i => (byte)(i / value)).ToArray();
        }
    }
}