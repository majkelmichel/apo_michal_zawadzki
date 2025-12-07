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
using Presentation.Neighbour;
using Presentation.Point;
using Presentation.Point.Bool;
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

    private void CalculateLut(object sender, RoutedEventArgs e)
    {
        var isGrayScale = ColorDepth.IsGrayscale(_windowModel.Image);
        var pixels = _windowModel.Image.ToPixels();
        var bytesPerPixel = _windowModel.Image.GetBytesPerPixel();

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

    private void DuplicateImage(object sender, RoutedEventArgs e)
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

    private void SubtractImages(object sender, RoutedEventArgs e)
    {
        var subtractImagesWindow = new SubtractImages(_windowModel);
        subtractImagesWindow.ShowDialog();
        
        _windowModel.Image = subtractImagesWindow.Image;
        LoadImage();
    }

    private void NumberOperations(object sender, RoutedEventArgs e)
    {
        var numberOperationsWindow = new NumberOperations(_windowModel.Image);
        numberOperationsWindow.ShowDialog();
        
        _windowModel.Image = numberOperationsWindow.Image;
        LoadImage();
    }

    private void ConvertToGrayscale(object sender, RoutedEventArgs e)
    {
        _windowModel.Image = _windowModel.Image.ToGrayscale();
        LoadImage();
    }

    private void BoolOperations(object sender, RoutedEventArgs e)
    {
        var boolOperationsWindow = new BoolOperationsWindow(_windowModel);
        boolOperationsWindow.ShowDialog();
        
        _windowModel.Image = boolOperationsWindow.Image;
        LoadImage();
    }

    private void BlurOperations(object sender, RoutedEventArgs e)
    {
        var blurOperationsWindow = new BlurOperationsWindow(_windowModel);
        blurOperationsWindow.ShowDialog();
        
        _windowModel.Image = blurOperationsWindow.Image;
        LoadImage();
    }

    private void SharpenOperations(object sender, RoutedEventArgs e)
    {
        var sharpenOperationsWindow = new SharpenOperationsWindow(_windowModel);
        sharpenOperationsWindow.ShowDialog();
        
        _windowModel.Image = sharpenOperationsWindow.Image;
        LoadImage();
    }

    private void EdgeDetectionOperations(object sender, RoutedEventArgs e)
    {
        var edgeDetectionOperationsWindow = new EdgeDetectionOperationsWindow(_windowModel.Image);
        edgeDetectionOperationsWindow.ShowDialog();
        
        _windowModel.Image = edgeDetectionOperationsWindow.Image;
        LoadImage();
    }

    private void MedianBlur(object sender, RoutedEventArgs e)
    {
        var medianBlurWindow = new MedianBlurWindow(_windowModel.Image);
        medianBlurWindow.ShowDialog();
        
        _windowModel.Image = medianBlurWindow.Image;
        LoadImage();
    }

    private void CannyEdgeDetection(object sender, RoutedEventArgs e)
    {
        var cannyEdgeDetectionWindow = new CannyEdgeDetectionWindow(_windowModel.Image);
        cannyEdgeDetectionWindow.ShowDialog();
        
        _windowModel.Image = cannyEdgeDetectionWindow.Image;
        LoadImage();
    }

    private void HistogramStretch(object sender, RoutedEventArgs e)
    {
        var histogramStretchWindow = new HistogramStretchWindow(_windowModel.Image);
        histogramStretchWindow.ShowDialog();
        
        _windowModel.Image = histogramStretchWindow.Image;
        LoadImage();
    }

    private void DoubleThreshold(object sender, RoutedEventArgs e)
    {
        var doubleThresholdWindow = new DoubleThreshold(_windowModel.Image);
        doubleThresholdWindow.ShowDialog();
        
        _windowModel.Image = doubleThresholdWindow.Image;
        LoadImage();
    }
}