using System.Drawing;

namespace Presentation.WindowManagement;

public class ManagedWindow(string title, bool isMaximized, Bitmap image)
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Title { get; } = title;
    public int Width { get; } = image.Width;
    public int Height { get; } = image.Height;
    public bool IsMaximized { get; } = isMaximized;
    public Bitmap Image { get; set; } = image;
}