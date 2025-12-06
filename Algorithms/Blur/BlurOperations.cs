using System.Drawing;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Size = OpenCvSharp.Size;

namespace Algorithms.Blur;

public class BlurOperations
{
    public static Bitmap Blur(Bitmap bitmap, BorderTypes borderType = BorderTypes.Default, int borderWidth = 1)
    {
        var mat = bitmap.ToMat();
        // TODO: add border handling
        // https://stackoverflow.com/questions/34811509/using-opencv-blur-with-border-constant-option
        var result = mat.Blur(new Size(3, 3), borderType: borderType);
        
        return result.ToBitmap();
    }
    
    public static Bitmap MedianBlur(Bitmap bitmap)
    {
        var mat = bitmap.ToMat();
        var result = mat.MedianBlur(3);
        
        return result.ToBitmap();
    }

    public static Bitmap GaussianBlur(Bitmap bitmap)
    {
        var mat = bitmap.ToMat();
        var result = mat.GaussianBlur(new Size(3, 3), 0);
        
        return result.ToBitmap();
    }
}