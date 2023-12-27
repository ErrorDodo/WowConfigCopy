using System.Windows;
using WowConfigCopy.Common.Interfaces;

namespace WowConfigCopy.Common.Helpers;

public static class ProcessHelper
{
    public static async Task EnsureWoWIsClosed(IProcessViewer processViewer)
    {
        if (processViewer.IsProcessRunning("WowClassic"))
        {
            MessageBox.Show("Please close World of Warcraft before proceeding.", "Notification", MessageBoxButton.OK);
            while (processViewer.IsProcessRunning("WowClassic"))
            {
                await Task.Delay(5000);
            }
        }
    }

    public static async Task PromptToStartWoW(IProcessViewer processViewer)
    {
        MessageBox.Show("Please start World of Warcraft.", "Notification", MessageBoxButton.OK);
        while (!processViewer.IsProcessRunning("WowClassic"))
        {
            await Task.Delay(5000);
        }
    }
}
