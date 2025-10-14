using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace View;

public partial class ImageWindow : Window
{
    private int originalWidth = 0;
    private int originalHeight = 0;
    public ImageWindow() => InitializeComponent();

    public async Task LoadImageAsync(Stream imageStream, string? displayName = null)
    {
        try
        {
            using var memory = new MemoryStream();
            await imageStream.CopyToAsync(memory);

            memory.Position = 0;
            var bitmap = new Bitmap(memory);

            PreviewImage.Source = new Avalonia.Media.Imaging.Bitmap(memory);

            originalHeight = bitmap.Height;
            originalWidth = bitmap.Width;
            Width = bitmap.Width;
            Height = bitmap.Width;
            CanResize = true;
            SizeToContent = SizeToContent.Manual;
        }
        catch (Exception ex)
        {
        }
    }

    public void ResizeToOriginal(object sender, RoutedEventArgs e)
    {
        Width = originalWidth;
        Height = originalHeight;
    }
}