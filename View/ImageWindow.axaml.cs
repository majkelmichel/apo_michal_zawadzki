using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using OpenCvSharp;
using Window = Avalonia.Controls.Window;

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

            using var mat = Cv2.ImDecode(memory.ToArray(), ImreadModes.Color);

            memory.Position = 0;
            var bitmap = new Bitmap(memory);

            PreviewImage.Source = bitmap;

            originalHeight = mat.Height;
            originalWidth = mat.Width;
            Width = mat.Width;
            Height = mat.Height;
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