using System.Drawing;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Algorithms.Point;
using Presentation.WindowManagement;

namespace Presentation.Point.Math;

public partial class SubtractImages : Window
{
    public IEnumerable<WindowViewmodel> Windows { get; init; }
    private ManagedWindow[] _windows;
    private Bitmap _resultImage;
    public Bitmap Image => _resultImage;

    private ManagedWindow _currentWindow;
    private WindowViewmodel _selectedWindow;
    
    public SubtractImages(ManagedWindow currentWindow)
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
        _currentWindow = currentWindow;
    }

    private void SubmitButton_Click(object sender, RoutedEventArgs e)
    {
        _resultImage = MathOperations.SubtractImages(_currentWindow.Image, _windows.First(w => w.Id == _selectedWindow.Id).Image);
        Close();
    }

    private void SelectImage_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SelectImage.SelectedItem is not WindowViewmodel selected) return;
        _selectedWindow = selected;
    }
}