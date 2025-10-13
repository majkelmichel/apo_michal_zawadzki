using OpenCvSharp;
using System.Drawing;

namespace Algorithms;

public class LookUpTableCalculator
{
    public static async Task Calculate(Stream imageStream)
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
    }
}