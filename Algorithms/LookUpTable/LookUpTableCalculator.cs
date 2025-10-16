using OpenCvSharp;
using System.Drawing;
using OpenCvSharp.Extensions;

namespace Algorithms;

public class LookUpTableCalculator
{
    public static async Task<LookUpTableColor> CalculateColor(Bitmap bitmap)
    {
        try
        {
            var mat = BitmapConverter.ToMat(bitmap);
            
            var channels = Cv2.Split(mat);

            int[] histSize = [256];
            Rangef[] ranges = [new(0, 256)];

            using var histBlue = new Mat();
            using var histGreen = new Mat();
            using var histRed = new Mat();
            
            Cv2.CalcHist([channels[0]], [0], new Mat(), histBlue, 1, histSize, ranges);
            Cv2.CalcHist([channels[1]], [0], new Mat(), histGreen, 1, histSize, ranges);
            Cv2.CalcHist([channels[2]], [0], new Mat(), histRed, 1, histSize, ranges);
            
            var rows = new LookUpTableColorRow[256];
            
            for (var i = 0; i < 256; i++)
            {
                rows[i] = new LookUpTableColorRow
                {
                    Blue = (byte)histBlue.Get<float>(i),
                    Green = (byte)histGreen.Get<float>(i),
                    Red = (byte)histRed.Get<float>(i)
                };
            }

            foreach (var channel in channels)
            {
                channel.Dispose();
            }
            
            return new LookUpTableColor
            {
                Rows = rows
            };
        }
        catch (Exception e)
        {
            // ignored
        }

        return new LookUpTableColor
        {
            Rows = []
        };
    }

    public static async Task<LookUpTableGrayscale> CalculateGrayscale(Bitmap bitmap)
    {
        try
        {
            var mat = BitmapConverter.ToMat(bitmap);

            Mat hist = new Mat();
            int[] histSize = [256];
            Rangef[] ranges = [new(0, 256)];

            Cv2.CalcHist([mat], [0], new Mat(), hist, 1, histSize, ranges);

            var rows = new LookUpTableGrayscaleRow[256];

            for (int i = 0; i < 256; i++)
            {
                rows[i] = new LookUpTableGrayscaleRow
                {
                    Intensity = (byte)hist.Get<float>(i)
                };
            }

            return new LookUpTableGrayscale
            {
                Rows = rows
            };
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error calculating grayscale LUT: {e.Message}");
        }
        return new LookUpTableGrayscale
        {
            Rows = []
        };
    }
}