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
        if (mainWindow == null) return;

        if (mainWindow.Tag == null)
        {
            mainWindow.Tag = new Rect(mainWindow.Left, mainWindow.Top, mainWindow.Width, mainWindow.Height);
            
            var workingArea = SystemParameters.WorkArea;
            
            mainWindow.Left = workingArea.Left;
            mainWindow.Top = workingArea.Top;
            mainWindow.Width = workingArea.Width;
            mainWindow.Height = workingArea.Height;
        }
        else
        {
            if (mainWindow.Tag is Rect originalSize)
            {
                mainWindow.Left = originalSize.Left;
                mainWindow.Top = originalSize.Top;
                mainWindow.Width = originalSize.Width;
                mainWindow.Height = originalSize.Height;
            }

            mainWindow.Tag = null;
        }
    }

}