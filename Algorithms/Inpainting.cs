using System.Drawing;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace Algorithms;

public static class Inpainting
{
    public static Bitmap Inpaint(Bitmap bitmap, Bitmap mask)
    {
        var mat = bitmap.ToMat();
        var maskMat = mask.ToMat();
        
        var result = new Mat();
        Cv2.Inpaint(mat, maskMat, result, 3, InpaintMethod.Telea);
        
        return result.ToBitmap();
    }
}