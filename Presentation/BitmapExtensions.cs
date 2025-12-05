using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Presentation;

public static class BitmapExtensions
{
    public static BitmapSource ToBitmapSource(this Bitmap bitmap)
    {
        var hBitmap = bitmap.GetHbitmap();
        try
        {
            return Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }
        finally
        {
            DeleteObject(hBitmap);
        }
    }
    
    [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool DeleteObject([In] IntPtr hObject);

    public static byte[] ToPixels(this Bitmap original)
    {
        
        var bitmap = original.ToBitmapSource();
        
        var width = original.Width;
        var height = original.Height;
        var stride = (width * bitmap.Format.BitsPerPixel + 7) / 8;
        var pixels = new byte[stride * height];
        bitmap.CopyPixels(pixels, stride, 0);
        
        return pixels;
    }

    public static int GetBytesPerPixel(this Bitmap original)
    {
        var bitmap = original.ToBitmapSource();
        var bytesPerPixel = (bitmap.Format.BitsPerPixel + 7) / 8;
        return bytesPerPixel; 
    }

    public static Bitmap ToGrayscale(this Bitmap original)
    {
        var recodingTable = new byte[256];
        for (var i = 0; i < 256; i++) recodingTable[i] = (byte)i;
        var result = original.Recode(recodingTable);
        return result;
    }
    
    public static Bitmap Recode(this Bitmap original, byte[] recodingTable)
    {
        var bitmap = new Bitmap(original.Width, original.Height, PixelFormat.Format8bppIndexed);
        
        var palette = bitmap.Palette;
        for (var i = 0; i < 256; i++)
        {
            palette.Entries[i] = Color.FromArgb(i, i, i);
        }
        bitmap.Palette = palette;
        
        var bitmapData = bitmap.LockBits(
            new Rectangle(0, 0, bitmap.Width, bitmap.Height),
            System.Drawing.Imaging.ImageLockMode.WriteOnly,
            PixelFormat.Format8bppIndexed);
        
        try
        {
            unsafe
            {
                var ptr = (byte*)bitmapData.Scan0;
                
                for (var y = 0; y < bitmap.Height; y++)
                {
                    for (var x = 0; x < bitmap.Width; x++)
                    {
                        var currentPixelValue = original.GetPixel(x, y).B;
                        var newPixelValue = recodingTable[currentPixelValue];
                        ptr[y * bitmapData.Stride + x] = newPixelValue;
                    }
                }
            }
        }
        finally
        {
            bitmap.UnlockBits(bitmapData);
        }

        return bitmap;
    }
}