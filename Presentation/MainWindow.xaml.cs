using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Presentation.WindowManagement;

namespace Presentation;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    
    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnOpenImage(object sender, RoutedEventArgs e) => OpenImageAsync();

    private static void OpenImageAsync()
    {
        var dialog = new OpenFileDialog
        {
            Title = "Open Image",
            Filter = "Image Files|*.png;*.jpg;*.jpeg;*.gif;*.bmp;*.tiff",
            Multiselect = false
        };

        if (dialog.ShowDialog() != true) return;
        var filePath = dialog.FileName;
        var fileName = Path.GetFileName(filePath);
        var bitmap = new Bitmap(filePath);
        var manager = WindowManager.GetInstance();

        var window = manager.AddWindow(fileName, bitmap);

        var imageWindow = new ImageWindow(window);
        imageWindow.Show();
    }

    protected override void OnPreviewKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.O && Keyboard.Modifiers == ModifierKeys.Control)
        {
            OpenImageAsync();
            e.Handled = true;
        }
        base.OnPreviewKeyDown(e);
    }
}