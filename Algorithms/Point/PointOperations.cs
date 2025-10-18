using System.Drawing;

namespace Algorithms.Point;

public class PointOperations
{
    public static Bitmap AddImages(Bitmap[] images, bool saturate = false)
    {
        return images[0]; // TODO:
    }
    
    public static Bitmap ReduceGrayLevels(Bitmap bitmap, byte grayLevels)
    {
        var result = new Bitmap(bitmap.Width, bitmap.Height);
        var newLevelLut = new int[256];
        var jump = 256 / grayLevels;
        for (var i = 0; i < 256; i += jump)
        {
            for (var j = 0; j < jump && i + j < 256; j++)
            {
                newLevelLut[i + j] = i;
            }
        }

        for (var y = 0; y < bitmap.Height; y++)
        {
            for (var x = 0; x < bitmap.Width; x++)
            {
                var pixel = bitmap.GetPixel(x, y);
                var newPixel = newLevelLut[pixel.R];
                result.SetPixel(x, y, Color.FromArgb(newPixel, newPixel, newPixel));
            }
        }
        
        return result;
    }
    
    public static Bitmap ApplyNegation(Bitmap bitmap)
    {
        var result = new Bitmap(bitmap.Width, bitmap.Height);
        
        for (var y = 0; y < bitmap.Height; y++)
        {
            for (var x = 0; x < bitmap.Width; x++)
            {
                var pixel = bitmap.GetPixel(x, y);
                
                var negated = 255 - pixel.R;
                
                result.SetPixel(x, y, Color.FromArgb(negated, negated, negated));
            }
        }
        
        return result;
    }
}