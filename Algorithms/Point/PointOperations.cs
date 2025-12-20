using System.Drawing;
using System.Drawing.Imaging;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace Algorithms.Point;

public static class PointOperations
{
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

    public static Bitmap BinaryThreshold(Bitmap bitmap, byte threshold)
    {
        var result = new Bitmap(bitmap.Height, bitmap.Height);

        for (var y = 0; y < bitmap.Height; y++)
        {
            for (var x = 0; x < bitmap.Width; x++)
            {
                var pixel = bitmap.GetPixel(x, y);
                
                var newPixel = pixel.R > threshold ? 255 : 0;
                
                result.SetPixel(x, y, Color.FromArgb(newPixel, newPixel, newPixel));
            }
        }

        return result;
    }
    
    public static Bitmap GrayscaleThreshold(Bitmap bitmap, byte threshold)
    {
        var result = new Bitmap(bitmap.Height, bitmap.Height);
        
        for (var y = 0; y < bitmap.Height; y++)
        {
            for (var x = 0; x < bitmap.Width; x++)
            {
                var pixel = bitmap.GetPixel(x, y);
                
                var newPixel = pixel.R > threshold ? pixel.R : 0;
                
                result.SetPixel(x, y, Color.FromArgb(newPixel, newPixel, newPixel));
            }
        }
        
        return result;
    }

    // lab 3 zad 2
    public static byte[] DoubleThreshold(int p1, int p2)
    {
        var recodingTable = new byte[256];
        for (var i = 0; i < 256; i++)
        {
            if (i < p1) recodingTable[i] = 0;
            else if (i < p2) recodingTable[i] = 255;
            else recodingTable[i] = 0;
        }

        return recodingTable;
    }
    
    public static Tuple<Bitmap, double> OtsuThreshold(Bitmap bitmap)
    {
        var mat = bitmap.ToMat();

        var res = new Mat();
        
        var threshold = Cv2.Threshold(mat, res, 0, 255, ThresholdTypes.Otsu);

        var result = mat.Threshold(0, 255, ThresholdTypes.Otsu);
        
        return new Tuple<Bitmap, double>(result.ToBitmap(), threshold);
    }

    public static Bitmap AdaptiveThreshold(Bitmap bitmap)
    {
        var mat = bitmap.ToMat();
        
        var result = mat.AdaptiveThreshold(255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, 11, 2);
        
        return result.ToBitmap();
    }
}