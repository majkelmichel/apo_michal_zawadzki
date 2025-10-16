using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Algorithms;
using Microsoft.Win32;
using Presentation.WindowManagement;

namespace Presentation;

public partial class ImageWindow
{
    private readonly ManagedWindow _windowModel;
    public ImageWindow(ManagedWindow window)
    {
        InitializeComponent();
        _windowModel = window;
        
        Title = _windowModel.Title;
        Width = _windowModel.Width;
        Height = _windowModel.Height;
        WindowState = _windowModel.IsMaximized ? WindowState.Maximized : WindowState.Normal;
        Closed += (sender, args) => WindowManager.GetInstance().RemoveWindow(_windowModel.Id);
        
        LoadImage();
        Owner = Application.Current.MainWindow;
    }

    private void LoadImage()
    {
        try
        {
            using var memory = new MemoryStream();
            _windowModel.Image.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
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
        Width = _windowModel.Width;
        Height = _windowModel.Height;
        WindowState = WindowState.Normal;
    }

    private async void CalculateLut(object sender, RoutedEventArgs e)
    {
        var isGrayScale = ColorDepth.IsGrayscale(_windowModel.Image);

        if (isGrayScale)
        {
            var lut = await LookUpTableCalculator.CalculateGrayscale(_windowModel.Image);

            var lutWindow = new LookUpTableGrayscale(lut);
            lutWindow.Show();
        }
        else
        {
            var lut = await LookUpTableCalculator.CalculateColor(_windowModel.Image);
            
            var lutWindow = new LookUpTableColor(lut);
            lutWindow.Show();
        }
    }

    private async void DuplicateImage(object sender, RoutedEventArgs e)
    {
        var bitmap = (Bitmap)_windowModel.Image.Clone();
        var manager = WindowManager.GetInstance();
        var window = manager.AddWindow($"Copy of {Title}", bitmap);
        
        var imageWindow = new ImageWindow(window);
        imageWindow.Show();
    }

    private void ShowHistogram(object sender, RoutedEventArgs e)
    {
        var bitmapSource = _windowModel.Image.ToBitmapSource();
        
        var histogramWindow = new HistogramWindow(bitmapSource);
        histogramWindow.Show();
    }
}