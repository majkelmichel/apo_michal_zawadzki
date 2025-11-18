using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Algorithms;
using Algorithms.Point;
using Microsoft.Win32;
using Presentation.Point;
using Presentation.Point.Math;
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
            _windowModel.Image.Save(memory, ImageFormat.Png);
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
        var bitmap = _windowModel.Image.ToBitmapSource();
        
        var width = _windowModel.Image.Width;
        var height = _windowModel.Image.Height;
        var stride = (width * bitmap.Format.BitsPerPixel + 7) / 8;
        var pixels = new byte[stride * height];
        bitmap.CopyPixels(pixels, stride, 0);
        
        var bytesPerPixel = (bitmap.Format.BitsPerPixel + 7) / 8;

        if (isGrayScale)
        {
            var lut = LookUpTableCalculator.CalculateGrayscale(pixels, bytesPerPixel);

            var lutWindow = new LookUpTableGrayscale(lut);
            lutWindow.Show();
        }
        else
        {
            var lut = LookUpTableCalculator.CalculateColor(pixels, bytesPerPixel);
            
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
        var histogramWindow = new HistogramWindow(_windowModel);
        histogramWindow.Show();
    }

    private void Negation(object sender, RoutedEventArgs e)
    {
        var result = PointOperations.ApplyNegation(_windowModel.Image);
        _windowModel.Image = result;
        LoadImage();
    }

    private void SaveImage(object sender, RoutedEventArgs e)
    {
        var dialog = new SaveFileDialog
        {
            Title = "Save image",
            Filter = "PNG Image|*.png|JPG Image|*.jpg;*.jpeg|Bitmap Image|*.bmp|TIFF Image|*.tiff"
        };

        dialog.ShowDialog();

        if (dialog.FileName == "") return;
        var fs = new FileStream(dialog.FileName, FileMode.Create);

        switch (dialog.FilterIndex)
        {
            case 1:
                _windowModel.Image.Save(fs, ImageFormat.Png);
                break;
            case 2:
                _windowModel.Image.Save(fs, ImageFormat.Jpeg);
                break;
            case 3:
                _windowModel.Image.Save(fs, ImageFormat.Bmp);
                break;
            case 4:
                _windowModel.Image.Save(fs, ImageFormat.Tiff);
                break;
        }
        fs.Close();
    }

    private void ReduceGrayLevels(object sender, RoutedEventArgs e)
    {
        var reduceGrayLevelsWindow = new ReduceGrayLevels((Bitmap)_windowModel.Image.Clone());
        reduceGrayLevelsWindow.ShowDialog();
        
        _windowModel.Image = reduceGrayLevelsWindow.Image;
        LoadImage();
    }
    
    private void BinaryThreshold(object sender, RoutedEventArgs e)
    {
        var binaryThresholdWindow = new BinaryThreshold((Bitmap)_windowModel.Image.Clone());
        binaryThresholdWindow.ShowDialog();
        
        _windowModel.Image = binaryThresholdWindow.Image;
        LoadImage();
    }

    private void GrayscaleThreshold(object sender, RoutedEventArgs e)
    {
        var grayscaleThresholdWindow = new GrayscaleThreshold((Bitmap)_windowModel.Image.Clone());
        grayscaleThresholdWindow.ShowDialog();
        
        _windowModel.Image = grayscaleThresholdWindow.Image;
        LoadImage();
    }

    private void AddImages(object sender, RoutedEventArgs e)
    {
        var addImagesWindow = new AddImages(_windowModel);
        addImagesWindow.ShowDialog();
        
        _windowModel.Image = addImagesWindow.Image;
        LoadImage();
    }
}