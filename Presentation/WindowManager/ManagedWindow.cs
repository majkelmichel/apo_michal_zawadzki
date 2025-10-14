using System.Drawing;

namespace Presentation;

public class ManagedWindow
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Title { get; }
    public int Width { get; set; }
    public int Height { get; set; }
    public bool IsMaximized { get; }
    public Bitmap Image { get; }

    public ManagedWindow(string title, int width, int height, bool isMaximized, Bitmap image)
    {
        Title = title;
        Width = width;
        Height = height;
        IsMaximized = isMaximized;
        Image = image;
    }
}