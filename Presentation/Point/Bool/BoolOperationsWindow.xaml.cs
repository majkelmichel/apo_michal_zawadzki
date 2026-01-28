using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using Algorithms.Point;
using Presentation.Point.Math;
using Presentation.WindowManagement;

namespace Presentation.Point.Bool;

public partial class BoolOperationsWindow : Window
{
    
    public IEnumerable<WindowViewmodel> Windows { get; init; }
    private readonly ManagedWindow[] _windows;
    public Bitmap Image { get; private set; }

    private readonly ManagedWindow _currentWindow;
    private WindowViewmodel _selectedWindow;
    
    public BoolOperationsWindow(ManagedWindow currentWindow)
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

    private void SelectImage_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SelectImage.SelectedItem is not WindowViewmodel selected) return;
        _selectedWindow = selected;
    }

    private void NotClick(object sender, RoutedEventArgs e)
    {
        Image = BoolOperations.Not(_currentWindow.Image);
        Close();
    }

    private void AndClick(object sender, RoutedEventArgs e)
    {
        Image = BoolOperations.And(_currentWindow.Image, _windows.First(w => w.Id == _selectedWindow.Id).Image);
        Close();
    }

    private void OrClick(object sender, RoutedEventArgs e)
    {
        Image = BoolOperations.Or(_currentWindow.Image, _windows.First(w => w.Id == _selectedWindow.Id).Image);
        Close();
    }

    private void XorClick(object sender, RoutedEventArgs e)
    {
        Image = BoolOperations.Xor(_currentWindow.Image, _windows.First(w => w.Id == _selectedWindow.Id).Image);
        Close();
    }
}