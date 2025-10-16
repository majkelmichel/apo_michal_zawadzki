using System.Drawing;

namespace Algorithms.Binary;

public class BinaryOperations
{
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