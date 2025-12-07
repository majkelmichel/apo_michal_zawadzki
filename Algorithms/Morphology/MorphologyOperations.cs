using System.Drawing;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace Algorithms.Morphology;

public class MorphologyOperations
{
    private static Mat GetKernel(StructuringElements element)
    {
        if (element == StructuringElements.Cross)
        {
            return Cv2.GetStructuringElement(
                MorphShapes.Cross,
                new OpenCvSharp.Size(3, 3)
            );
        }

        return Cv2.GetStructuringElement(
            MorphShapes.Rect,
            new OpenCvSharp.Size(3, 3));
    }
    
    public static Bitmap Erode(Bitmap image, StructuringElements element)
    {
        var structuringElement = GetKernel(element);

        var mat = image.ToMat();
        var res = mat.Erode(structuringElement);
        
        return res.ToBitmap();
    }

    public static Bitmap Dilate(Bitmap image, StructuringElements element)
    {
        var structuringElement = GetKernel(element);
        
        var mat = image.ToMat();
        var res = mat.Dilate(structuringElement);
        
        return res.ToBitmap();
    }

    public static Bitmap Open(Bitmap image, StructuringElements element)
    {
        return Erode(Dilate(image, element), element);
    }

    public static Bitmap Close(Bitmap image, StructuringElements element)
    {
        return Dilate(Erode(image, element), element);
    }

    // lab 3 zad 4
    public static Bitmap Skeletonize(Bitmap image)
    {
        var mat = image.ToMat();
        var res = new Mat();
        
        OpenCvSharp.XImgProc.CvXImgProc.Thinning(mat, res);
        
        return res.ToBitmap();
    }
}