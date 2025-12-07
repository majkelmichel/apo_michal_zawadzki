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
    private ManagedWindow _currentWindow;
    private WindowViewmodel _selectedWindow;

    public InpaintWindow(ManagedWindow currentWindow)
    {
        InitializeComponent();
        _windows = WindowManager.GetInstance().GetWindows();
        Windows = _windows
            .Where(w => w.Id != currentWindow.Id).ToArray()
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
        Image = Inpainting.Inpaint(_currentWindow.Image, _windows.First(w => w.Id == _selectedWindow.Id).Image);
        Close();
    }
    
    private void SelectImage_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SelectImage.SelectedItem is not WindowViewmodel selected) return;
        _selectedWindow = selected;
    }
}