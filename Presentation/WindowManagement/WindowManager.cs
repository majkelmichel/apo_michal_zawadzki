using System.Drawing;

namespace Presentation.WindowManagement;

public class WindowManager
{
    private static WindowManager? _manager;
    private readonly List<ManagedWindow> _windows;

    private WindowManager()
    {
        _windows = [];
    }

    public static WindowManager GetInstance()
    {
        _manager ??= new WindowManager();

        return _manager;
    }
    
    public ManagedWindow AddWindow(string title, Bitmap bitmap)
    {
        var managedWindow = new ManagedWindow(title, false, bitmap);
        _windows.Add(managedWindow);
        return managedWindow;
    }

    public void RemoveWindow(Guid id)
    {
        var window = _windows.Find(x => x.Id == id);
        if (window != null) _windows.Remove(window);
    }
    
    public ManagedWindow? GetWindow(Guid id) => _windows.Find(x => x.Id == id);
    
    public ManagedWindow[] GetWindows() => _windows.ToArray();
}