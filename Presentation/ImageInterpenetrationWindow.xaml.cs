using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Algorithms;
using Presentation.Point.Math;
using Presentation.WindowManagement;

namespace Presentation;

public partial class ImageInterpenetrationWindow : Window
{
    public IEnumerable<WindowViewmodel> Windows { get; init; }
    private readonly ManagedWindow[] _windows;
    public Bitmap Image { get; private set;  }
    private readonly ManagedWindow _originalImageWindow;
    private WindowViewmodel? _selectedWindow;
    private byte _sliderValue = 0;
    public ImageInterpenetrationWindow(ManagedWindow originalImageWindow)
    {
        InitializeComponent();
        _windows = WindowManager.GetInstance().GetWindows();
        Windows = _windows
            .Where(w => w.Id != originalImageWindow.Id) // exclude the original image window
            .Where(w => w.Width == originalImageWindow.Width && w.Height == originalImageWindow.Height) // include only images with the same size as the original image
            .Select((w, i) => new WindowViewmodel
            {
                Index = i,
                Title = w.Title,
                Id = w.Id
            });
        DataContext = this;
        _originalImageWindow = originalImageWindow;
        Image = originalImageWindow.Image;
        
        Height = originalImageWindow.Height + 180;
        
        using var memory = new MemoryStream();
        Image.Save(memory, ImageFormat.Bmp);
        memory.Position = 0;
        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.StreamSource = memory;
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.EndInit();

        OriginalImage.Source = bitmapImage;
    }
    
    private void SubmitButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedWindow is null)
        {
            MessageBox.Show("Please select a window with image to interpenetrate with.");
            return;
        }
        Image = ImageInterpenetration.Interpenetrate(_originalImageWindow.Image, _windows.First(w => w.Id == _selectedWindow.Id).Image, _sliderValue);
        Close();
    }
    
    private void SelectImage_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SelectImage.SelectedItem is not WindowViewmodel selected) return;
        _selectedWindow = selected;
        
        using var memory = new MemoryStream();
        _windows.First(w => w.Id == _selectedWindow.Id).Image.Save(memory, ImageFormat.Bmp);
        memory.Position = 0;
        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.StreamSource = memory;
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.EndInit();

        SecondImage.Source = bitmapImage;
        
        UpdatePreview();
    }

    private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        _sliderValue = (byte)e.NewValue;
        UpdatePreview();
    }

    private void UpdatePreview()
    {
        // For performance reasons we are not calculating the result every time we want to update the preview.
        SecondImage.Opacity = _sliderValue / 16.0;
        OriginalImage.Opacity = (16.0 - _sliderValue) / 16.0;
    }
}