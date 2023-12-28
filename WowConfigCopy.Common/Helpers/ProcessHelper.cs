using WowConfigCopy.Common.Interfaces;

namespace WowConfigCopy.Common.Helpers;

public static class ProcessHelper
{
    public static event EventHandler<string> RequestCloseWoW;
    public static event EventHandler<string> RequestStartWoW;

    public static async Task EnsureWoWIsClosed(IProcessViewer processViewer)
    {
        if (processViewer.IsProcessRunning("WowClassic"))
        {
            RequestCloseWoW?.Invoke(null, "Please close World of Warcraft before proceeding.");
            while (processViewer.IsProcessRunning("WowClassic"))
            {
                await Task.Delay(5000);
            }
        }
    }

    public static async Task PromptToStartWoW(IProcessViewer processViewer)
    {
        RequestStartWoW?.Invoke(null, "Please start World of Warcraft.");
        while (!processViewer.IsProcessRunning("WowClassic"))
        {
            await Task.Delay(5000);
        }
    }
}