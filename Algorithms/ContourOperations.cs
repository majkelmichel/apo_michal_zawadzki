using System.Drawing;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace Algorithms;

public static class ContourOperations
{
    public static IEnumerable<OpenCvSharp.Point[]> CalculateContour(Bitmap bitmap)
    {
        var mat = bitmap.ToMat().CvtColor(ColorConversionCodes.BGR2GRAY);
        var binaryMat = mat.Threshold(127, 255, ThresholdTypes.Binary);

        Cv2.FindContours(binaryMat, out var contours, out _, RetrievalModes.CComp, ContourApproximationModes.ApproxSimple);
        
        return contours;
    }

    public static Moments CalculateMoments(OpenCvSharp.Point[] contour)
    {
        return Cv2.Moments(contour);
    }
    
    public static double CalculateArea(OpenCvSharp.Point[] contour)
    {
        return Cv2.ContourArea(contour);
    }

    public static double CalculatePerimeter(OpenCvSharp.Point[] contour)
    {
        return Cv2.ArcLength(contour, true);
    }

    public static double CalculateAspectRatio(OpenCvSharp.Point[] contour)
    {
        var boundingRectangle = Cv2.BoundingRect(contour);

        return (double)boundingRectangle.Width / boundingRectangle.Height;
    }

    public static double CalculateExtent(OpenCvSharp.Point[] contour)
    {
        var boundingRectangle = Cv2.BoundingRect(contour);

        return CalculateArea(contour) / (boundingRectangle.Width * boundingRectangle.Height);
    }

    public static double CalculateSolidity(OpenCvSharp.Point[] contour)
    {
        var hull = Cv2.ConvexHull(contour);
        var hullArea = Cv2.ContourArea(hull);
        
        return CalculateArea(contour) / hullArea;
    }
    
    public static double CalculateEquivalentDiameter(OpenCvSharp.Point[] contour)
    {
        return Math.Sqrt(4 * CalculateArea(contour) / Math.PI);
    }
}