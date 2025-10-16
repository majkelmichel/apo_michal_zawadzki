using System.Drawing;

namespace Presentation.WindowManagement;

public class WindowManager
{
    private static WindowManager? manager;
    private List<ManagedWindow> Windows;

    private WindowManager()
    {
        Windows = [];
    }

    public static WindowManager GetInstance()
    {
        manager ??= new WindowManager();

        return manager;
    }
    
    public ManagedWindow AddWindow(string title, Bitmap bitmap)
    {
        var managedWindow = new ManagedWindow(title, false, bitmap);
        Windows.Add(managedWindow);
        return managedWindow;
    }

    public void RemoveWindow(Guid id)
    {
        var window = Windows.Find(x => x.Id == id);
        if (window != null) Windows.Remove(window);
    }
    
    public ManagedWindow? GetWindow(Guid id) => Windows.Find(x => x.Id == id);
}