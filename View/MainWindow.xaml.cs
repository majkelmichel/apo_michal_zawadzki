using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace View;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void OnOpenImage(object? sender, RoutedEventArgs e) => await OpenImageAsync();
    
    private void OnExit(object? sender, RoutedEventArgs e) => Close();

    private async Task OpenImageAsync()
    {
        var options = new FilePickerOpenOptions
        {
            Title = "Open Image",
            FileTypeFilter = [FilePickerFileTypes.ImageAll],
            AllowMultiple = false,
        };
        
        var files = await StorageProvider.OpenFilePickerAsync(options);

        if (files.Count >= 1)
        {
            var file = files[0];
            
            var imageWindow = new ImageWindow();
            
            await using var stream = await file.OpenReadAsync();
            await imageWindow.LoadImageAsync(stream, file.Name);
            
            imageWindow.Show();
        }
    }
}