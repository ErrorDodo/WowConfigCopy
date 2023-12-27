namespace WowConfigCopy.Common.Interfaces;

public interface IProcessViewer
{
    bool IsProcessRunning(string processName);
}