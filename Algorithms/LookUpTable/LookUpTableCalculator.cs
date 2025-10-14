using OpenCvSharp;
using System.Drawing;
using OpenCvSharp.Extensions;

namespace Algorithms;

public class LookUpTableCalculator
{
    public static async Task<LookUpTableColor> CalculateColor(Stream imageStream)
    {
        try
        {
            using var memory = new MemoryStream();
            await imageStream.CopyToAsync(memory);

            memory.Position = 0;
            var bitmap = new Bitmap(memory);

            var lut = new int[256];

            for (int y = 0; y < bitmap.Height; y++)
            {
                
            }
        }
        catch (Exception e) {}

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