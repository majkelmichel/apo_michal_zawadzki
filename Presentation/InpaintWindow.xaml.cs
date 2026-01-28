using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using Algorithms;
using Presentation.Point.Math;
using Presentation.WindowManagement;

namespace Presentation;

public partial class InpaintWindow : Window
{
    public IEnumerable<WindowViewmodel> Windows { get; init; }
    private readonly ManagedWindow[] _windows;
    public Bitmap Image { get; private set;  }
    private WindowViewmodel? _selectedWindow;

    public InpaintWindow(ManagedWindow currentWindow)
    {
        InitializeComponent();
        _windows = WindowManager.GetInstance().GetWindows();
        Windows = _windows
            .Where(w => w.Id != currentWindow.Id) // exclude the original image window
            .Where(w => w.Width == currentWindow.Width && w.Height == currentWindow.Height) // include only images with the same size as the original image
            .Select((w, i) => new WindowViewmodel
            {
                Index = i,
                Title = w.Title,
                Id = w.Id
            });
        DataContext = this;
        Image = currentWindow.Image;
    }
    
    private void SubmitButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedWindow is null)
        {
            MessageBox.Show("Please select a window with image to inpaint with.");
            return;
        }
        Image = Inpainting.Inpaint(Image, _windows.First(w => w.Id == _selectedWindow.Id).Image);
        Close();
    }
    
    private void SelectImage_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SelectImage.SelectedItem is not WindowViewmodel selected) return;
        _selectedWindow = selected;
    }
}