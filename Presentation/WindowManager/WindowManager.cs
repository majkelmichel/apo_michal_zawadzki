namespace Presentation;

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
    
    public void AddWindow(ManagedWindow managedWindow)
    {
        Windows.Add(managedWindow);
    }

    public void RemoveWindow(Guid id)
    {
        var window = Windows.Find(x => x.Id == id);
        if (window != null) Windows.Remove(window);
    }
    
    public ManagedWindow? GetWindow(Guid id) => Windows.Find(x => x.Id == id);
}