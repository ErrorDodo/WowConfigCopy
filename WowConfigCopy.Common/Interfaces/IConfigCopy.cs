namespace WowConfigCopy.Common.Interfaces;

public interface IConfigCopy
{
    void CopyConfigFiles(string sourceConfigLocation, string destinationConfigLocation, CancellationToken cancellationToken, bool firstRun = true, bool copySavedVariables = false);
    event EventHandler<string> FileCopying;
    event EventHandler<int> ProgressChanged;
}