using System.Drawing;

namespace Algorithms.Point;

public class BoolOperations
{
    
    public static Bitmap Not(Bitmap image)
    {
        
        var result = new Bitmap(image.Width, image.Height);
        
        for (var y = 0; y < image.Height; y++)
        {
            for (var x = 0; x < image.Width; x++)
            {
                var pixel = image.GetPixel(x, y);

                var newPixel = 255 - pixel.R;
                
                result.SetPixel(x, y, Color.FromArgb(newPixel, newPixel, newPixel));
            }
        }

        return result;
    }

    public static Bitmap And(Bitmap image1, Bitmap image2)
    {
        if (image1.Width != image2.Width || image1.Height != image2.Height) throw new ArgumentException("Images are not of the same size");
        
        var result = new Bitmap(image1.Width, image1.Height);
        
        for (var y = 0; y < image1.Height; y++)
        {
            for (var x = 0; x < image1.Width; x++)
            {
                var pixel1 = image1.GetPixel(x, y);
                var pixel2 = image2.GetPixel(x, y);

                var newPixel = pixel1.B & pixel2.B;
                
                result.SetPixel(x, y, Color.FromArgb(newPixel, newPixel, newPixel));
            }
        }

        return result;
    }
    
    public static Bitmap Or(Bitmap image1, Bitmap image2)
    {
        if (image1.Width != image2.Width || image1.Height != image2.Height) throw new ArgumentException("Images are not of the same size");
        
        var result = new Bitmap(image1.Width, image1.Height);
        
        for (var y = 0; y < image1.Height; y++)
        {
            for (var x = 0; x < image1.Width; x++)
            {
                var pixel1 = image1.GetPixel(x, y);
                var pixel2 = image2.GetPixel(x, y);

                var newPixel = pixel1.B | pixel2.B;
                
                result.SetPixel(x, y, Color.FromArgb(newPixel, newPixel, newPixel));
            }
        }

        return result;
    }
    
    public static Bitmap Xor(Bitmap image1, Bitmap image2)
    {
        if (image1.Width != image2.Width || image1.Height != image2.Height) throw new ArgumentException("Images are not of the same size");
        
        var result = new Bitmap(image1.Width, image1.Height);
        
        for (var y = 0; y < image1.Height; y++)
        {
            for (var x = 0; x < image1.Width; x++)
            {
                var pixel1 = image1.GetPixel(x, y);
                var pixel2 = image2.GetPixel(x, y);

                var newPixel = pixel1.B ^ pixel2.B;
                
                result.SetPixel(x, y, Color.FromArgb(newPixel, newPixel, newPixel));
            }
        }

        return result;
    }
}