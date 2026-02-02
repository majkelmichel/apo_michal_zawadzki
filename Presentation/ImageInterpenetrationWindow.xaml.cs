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
        
        SliderValue.Maximum = ImageInterpenetration.Ratio; // set the maximum value of the slider
        // calculate window height based on the original image height
        Height = originalImageWindow.Height + 180;
        
        // load the original image into the preview
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

        try
        {
            Image = ImageInterpenetration.Interpenetrate(_originalImageWindow.Image,
                _windows.First(w => w.Id == _selectedWindow.Id).Image, _sliderValue)
                .ToGrayscale();
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message);
        }
        Close();
    }
    
    private void SelectImage_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SelectImage.SelectedItem is not WindowViewmodel selected) return;
        _selectedWindow = selected;
        
        // load the selected image into the preview
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
        // for performance reasons we are not calculating the result every time we want to update the preview.
        SecondImage.Opacity = _sliderValue / ImageInterpenetration.Ratio;
        OriginalImage.Opacity = (ImageInterpenetration.Ratio - _sliderValue) / ImageInterpenetration.Ratio;
    }
}