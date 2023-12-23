using System.Windows;
using WowConfigCopy.UI.Interfaces;

namespace WowConfigCopy.UI.Services;

public class WindowService : IWindowService
{
    public void CloseWindow()
    {
        Application.Current.MainWindow.Close();
    }
    
    public void MinimizeWindow()
    {
        Application.Current.MainWindow.WindowState = WindowState.Minimized;
    }
    
    public void MaximizeRestoreWindow()
    {
        var mainWindow = Application.Current.MainWindow;
        mainWindow.WindowState = mainWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }
}