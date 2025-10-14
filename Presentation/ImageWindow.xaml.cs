using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Algorithms;
using Microsoft.Win32;

namespace Presentation;

public partial class ImageWindow : Window
{
    private readonly ManagedWindow windowModel;
    public ImageWindow(ManagedWindow window)
    {
        InitializeComponent();
        windowModel = window;
        
        Title = windowModel.Title;
        Width = windowModel.Width;
        Height = windowModel.Height;
        WindowState = windowModel.IsMaximized ? WindowState.Maximized : WindowState.Normal;
        
        LoadImage();
    }

    private void LoadImage()
    {
        try
        {
            using var memory = new MemoryStream();
            windowModel.Image.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
            memory.Position = 0;

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            
            PreviewImage.Source = bitmapImage;
        }
        catch (Exception e)
        {
            MessageBox.Show($"Error loading image: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ResizeToOriginal(object sender, RoutedEventArgs e)
    {
        Width = windowModel.Width;
        Height = windowModel.Height;
        WindowState = WindowState.Normal;
    }

    private async void CalculateLut(object sender, RoutedEventArgs e)
    {
        var isGrayScale = ColorDepth.IsGrayscale(windowModel.Image);

        if (isGrayScale)
        {
            var lut = await LookUpTableCalculator.CalculateGrayscale(windowModel.Image);

            Dispatcher.Invoke(() =>
            {
                var lutWindow = new LookUpTableGrayscale(lut);
                lutWindow.Show();
            });
        }
    }
}