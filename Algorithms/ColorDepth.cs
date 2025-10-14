using System.Drawing;
using System.Drawing.Imaging;

namespace Algorithms;

public class ColorDepth
{
    public static bool IsGrayscale(Bitmap bitmap)
    {
        return bitmap.PixelFormat is PixelFormat.Format8bppIndexed or PixelFormat.Format16bppGrayScale;
    }
}