using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace Presentation;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly WindowManager _manager;
    
    public MainWindow()
    {
        InitializeComponent();
        _manager = WindowManager.GetInstance();
    }

    private async void OnOpenImage(object sender, RoutedEventArgs e) => await OpenImageAsync();
    
    private async Task OpenImageAsync()
    {
        var dialog = new OpenFileDialog
        {
            Title = "Open Image",
            Filter = "Image Files|*.png;*.jpg;*.jpeg;*.gif;*.bmp;*.tiff",
            Multiselect = false
        };

        if (dialog.ShowDialog() == true)
        {
            var filePath = dialog.FileName;
            var fileName = Path.GetFileName(filePath);

            await Task.Run(() =>
            {
                var bitmap = new Bitmap(filePath);
                var window = new ManagedWindow(
                    fileName,
                    bitmap.Width,
                    bitmap.Height,
                    false,
                    bitmap);

                _manager.AddWindow(window);

                Dispatcher.Invoke(() =>
                {
                    var imageWindow = new ImageWindow(window);
                    imageWindow.Closed += (_, _) => _manager.RemoveWindow(window.Id);
                    imageWindow.Show();
                });
            });
        }
    }
    
    protected override void OnPreviewKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.O && Keyboard.Modifiers == ModifierKeys.Control)
        {
            _ = OpenImageAsync();
            e.Handled = true;
        }
        base.OnPreviewKeyDown(e);
    }
}