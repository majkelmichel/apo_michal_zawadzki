using System.Drawing;

namespace Presentation.WindowManagement;

public class ManagedWindow(string title, bool isMaximized, Bitmap image)
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Title { get; } = title;
    public int Width { get; set; } = image.Width;
    public int Height { get; set; } = image.Height;
    public bool IsMaximized { get; } = isMaximized;
    public Bitmap Image { get; } = image;
}