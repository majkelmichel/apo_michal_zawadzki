using System.Drawing;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace Algorithms.Blur;

public class NeighbourOperations
{
    private static Mat GetFromArray(float[,] array)
    {
        var mat = new Mat(array.GetLength(0), array.GetLength(1), MatType.CV_32F, Scalar.All(0));
        for (var y = 0; y < array.GetLength(0); y++)
        {
            for (var x = 0; x < array.GetLength(1); x++)
            {
                mat.Set(y, x, array[y, x]);
            }
        }

        return mat;
    }
    private static Mat GetBlurMatrix1()
    {
        float[,] arr = { { 0, 1f/5, 0 }, { 1f/5, 1f/5, 1f/5 }, { 0, 1f/5, 0} };
        return GetFromArray(arr);
    }
    
    private static Mat GetBlurMatrix2()
    {
        float[,] arr = { { 1f/17, 2f/17, 1f/17 }, { 2f/17, 5f/17, 2f/17 }, { 1f/17, 2f/17, 1f/17} };
        return GetFromArray(arr);
    }
    
    public static Bitmap Blur(Bitmap bitmap, BorderRuleTypes borderType, int borderConstant = 0, int selectedMatrix = 0)
    {
        var mat = bitmap.ToMat();
        var kernel = selectedMatrix == 0 ? GetBlurMatrix1() : GetBlurMatrix2();
        if (borderType == BorderRuleTypes.BorderConstant)
        {
            var matWithBorder = mat.CopyMakeBorder(1,1,1,1, BorderTypes.Constant, Scalar.All(0));
            var result = matWithBorder.Filter2D(-1, kernel, borderType: BorderTypes.Default);
            return result.ToBitmap();
        }

        if (borderType == BorderRuleTypes.BorderByUser)
        {
            var matWithBorder = mat.CopyMakeBorder(1,1,1,1, BorderTypes.Constant, Scalar.All(borderConstant));
            var result = matWithBorder.Filter2D(-1, kernel, borderType: BorderTypes.Default);
            return result.ToBitmap();
        }

        if (borderType == BorderRuleTypes.BorderReflect)
        {
            var result = mat.Filter2D(-1, kernel, borderType: BorderTypes.Reflect);
            return result.ToBitmap();
        }
        return bitmap;
    }

    public static Mat GetSharpenMatrix1()
    {
        float[,] kernel = { {0, 1, 0}, {1, -4, 1}, {0, 1, 0} };
        return GetFromArray(kernel);
    }

    public static Mat GetSharpenMatrix2()
    {
        float[,] kernel = { {-1, -1, -1}, {-1, 8, -1}, {-1, -1, -1} };
        return GetFromArray(kernel);
    }

    public static Mat GetSharpenMatrix3()
    {
        float[,] kernel = { { -1, 2, -1 }, { 2, -4, 2 }, { -1, 2, -1 } };
        return GetFromArray(kernel);
    }

    public static Bitmap Sharpen(Bitmap bitmap, BorderRuleTypes borderType, int borderConstant = 0, int selectedMatrix = 0)
    {
        var mat = bitmap.ToMat();
        var kernel = selectedMatrix switch
        {
            0 => GetSharpenMatrix1(),
            1 => GetSharpenMatrix2(),
            _ => GetSharpenMatrix3()
        };
        
        if (borderType == BorderRuleTypes.BorderConstant)
        {
            var matWithBorder = mat.CopyMakeBorder(1,1,1,1, BorderTypes.Constant, Scalar.All(0));
            var result = matWithBorder.Filter2D(-1, kernel, borderType: BorderTypes.Default);
            return result.ToBitmap();
        }

        if (borderType == BorderRuleTypes.BorderByUser)
        {
            var matWithBorder = mat.CopyMakeBorder(1,1,1,1, BorderTypes.Constant, Scalar.All(borderConstant));
            var result = matWithBorder.Filter2D(-1, kernel, borderType: BorderTypes.Default);
            return result.ToBitmap();
        }

        if (borderType == BorderRuleTypes.BorderReflect)
        {
            var result = mat.Filter2D(-1, kernel, borderType: BorderTypes.Reflect);
            return result.ToBitmap();
        }
        return bitmap;
    }

    private static Mat GetPrewittMatrixN()
    {
        float[,] arr = { {1, 1, 1}, {0, 0, 0}, {-1, -1, -1} };
        return GetFromArray(arr);
    }

    private static Mat GetPrewittMatrixNE()
    {
        float[,] arr = { {0, 1, 1}, {-1, 0, 1}, {-1, -1, 0} };
        return GetFromArray(arr);
    }

    private static Mat GetPrewittMatrixE()
    {
        float[,] arr = { {-1, 0, 1}, {-1, 0, 1}, {-1, 0, 1} };
        return GetFromArray(arr);
    }

    private static Mat GetPrewittMatrixSE()
    {
        float[,] arr = { {-1, -1, 0}, {-1, 0, 1}, {0, 1, 1} };
        return GetFromArray(arr);
    }
    
    private static Mat GetPrewittMatrixS()
    {
        float[,] arr = { {-1, -1, -1}, {0, 0, 0,}, {1, 1, 1} };
        return GetFromArray(arr);
    }

    private static Mat GetPrewittMatrixSW()
    {
        float[,] arr = { {0, -1, -1}, {1, 0, -1}, {1, 1, 0} };
        return GetFromArray(arr);
    }

    private static Mat GetPrewittMatrixW()
    {
        float[,] arr = { {1, 0, -1}, {1, 0, -1}, {1, 0, -1} };
        return GetFromArray(arr);
    }

    private static Mat GetPrewittMatrixNW()
    {
        float[,] arr = { {1, 1, 0}, {0, 0, -1}, {-1, -1, -1} };
        return GetFromArray(arr);
    }

    public static Bitmap PrewittEdge(Bitmap bitmap, BorderRuleTypes borderType, int borderConstant = 0,
        EdgeDetectionDirection direction = EdgeDetectionDirection.N)
    {
        
        var mat = bitmap.ToMat();
        var kernel = direction switch
        {
            EdgeDetectionDirection.NE => GetPrewittMatrixNE(),
            EdgeDetectionDirection.E => GetPrewittMatrixE(),
            EdgeDetectionDirection.SE => GetPrewittMatrixSE(),
            EdgeDetectionDirection.S => GetPrewittMatrixS(),
            EdgeDetectionDirection.SW => GetPrewittMatrixSW(),
            EdgeDetectionDirection.W => GetPrewittMatrixW(),
            EdgeDetectionDirection.NW => GetPrewittMatrixNW(),
            _ => GetPrewittMatrixN()
        };
        
        if (borderType == BorderRuleTypes.BorderConstant)
        {
            var matWithBorder = mat.CopyMakeBorder(1,1,1,1, BorderTypes.Constant, Scalar.All(0));
            var result = matWithBorder.Filter2D(-1, kernel, borderType: BorderTypes.Default);
            return result.ToBitmap();
        }

        if (borderType == BorderRuleTypes.BorderByUser)
        {
            var matWithBorder = mat.CopyMakeBorder(1,1,1,1, BorderTypes.Constant, Scalar.All(borderConstant));
            var result = matWithBorder.Filter2D(-1, kernel, borderType: BorderTypes.Default);
            return result.ToBitmap();
        }

        if (borderType == BorderRuleTypes.BorderReflect)
        {
            var result = mat.Filter2D(-1, kernel, borderType: BorderTypes.Reflect);
            return result.ToBitmap();
        }
        return bitmap;
    }
    
    private static Mat GetSobelMatrixX()
    {
        float[,] arr = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
        return GetFromArray(arr);
    }

    private static Mat GetSobelMatrixY()
    {
        float[,] arr = { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };
        return GetFromArray(arr);
    }
    
    public static Bitmap SobelDetectEdges(Bitmap bitmap, BorderRuleTypes borderType, int borderConstant = 0)
    {
        var src = bitmap.ToMat();
    
        // Konwersja do skali szarości jeśli obraz jest kolorowy
        if (src.Channels() > 1)
        {
            Cv2.CvtColor(src, src, ColorConversionCodes.BGR2GRAY);
        }

        // Konwersja do formatu float dla lepszej precyzji
        src.ConvertTo(src, MatType.CV_32F);

        // Obliczenie gradientów Sobela w kierunkach x i y
        var gradX = new Mat();
        var gradY = new Mat();

        var borderTypeCV = borderType switch
        {
            BorderRuleTypes.BorderConstant => BorderTypes.Constant,
            BorderRuleTypes.BorderByUser => BorderTypes.Constant,
            BorderRuleTypes.BorderReflect => BorderTypes.Reflect,
            _ => BorderTypes.Constant
        };

        // Obliczenie gradientów używając operatora Sobela
        Cv2.Sobel(src, gradX, MatType.CV_32F, 1, 0, 3, borderType: borderTypeCV);
        Cv2.Sobel(src, gradY, MatType.CV_32F, 0, 1, 3, borderType: borderTypeCV);

        // Obliczenie magnitude gradientu
        var magnitude = new Mat();
        Cv2.Magnitude(gradX, gradY, magnitude);

        // Normalizacja wyniku do zakresu 0-255
        Cv2.Normalize(magnitude, magnitude, 0, 255, NormTypes.MinMax);
        magnitude.ConvertTo(magnitude, MatType.CV_8U);

        return magnitude.ToBitmap();
    }

    public static Bitmap MedianBlur(Bitmap bitmap, int kernelSize)
    {
        var mat = bitmap.ToMat();
        var result = mat.MedianBlur(kernelSize);
        return result.ToBitmap();
    }
}