using System.Drawing;
using System.Net.Mime;
using System.Windows;
using System.Windows.Controls;
using Algorithms.Point;
using Presentation.WindowManagement;

namespace Presentation.Point.Math;

public partial class AddImages : Window
{
    public IEnumerable<WindowViewmodel> Windows { get; init; }
    private Bitmap _resultImage;
    public Bitmap Image => _resultImage;
    private bool[] _selectedWindows;
    private ManagedWindow[] _windows;
    private bool _saturate;
    
    public AddImages(ManagedWindow currentWindow)
    {
        InitializeComponent();
        _windows = WindowManager.GetInstance().GetWindows();
        Windows = _windows
            .Where(w => w.Id != currentWindow.Id)
            .Select((w, i) => new WindowViewmodel
            {
                Index = i,
                Title = w.Title,
                Id = w.Id
            });
        DataContext = this;
        _resultImage = currentWindow.Image;
        _selectedWindows = new bool[System.Math.Max(1, Windows.Count())];
    }

    private void SubmitButton_Click(object sender, RoutedEventArgs e)
    {
        var imagesToAdd = new List<Bitmap> { _resultImage };
        for (var i = 0; i < _selectedWindows.Length; i++)
        {
            if (_selectedWindows[i])
            {
                var windowViewModel = Windows.First(w => w.Index == i);
                var window = _windows.First(w => w.Id == windowViewModel.Id);
                imagesToAdd.Add(window.Image);
            }
        }

        try
        {
            _resultImage = MathOperations.AddImages(imagesToAdd.ToArray(), _saturate);
            Close();
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
    

    private void CheckboxChecked(object sender, RoutedEventArgs e)
    {
        if (sender is not CheckBox checkBox) return;
        // Check if the checkbox is checked
        var isChecked = checkBox.IsChecked == true;
        
        // Get the DataContext which contains your WindowViewmodel
        if (checkBox.DataContext is not WindowViewmodel viewModel) return;
        var index = viewModel.Index;
        _selectedWindows[index] = isChecked;
    }

    private void SaturateChecked(object sender, RoutedEventArgs e)
    {
        if (sender is not CheckBox checkBox) return;
        // Check if the checkbox is checked
        var isChecked = checkBox.IsChecked == true;
        
        _saturate = isChecked;
    }
}