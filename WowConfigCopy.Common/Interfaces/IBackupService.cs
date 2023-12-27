using WowConfigCopy.Common.Models;

namespace WowConfigCopy.Common.Interfaces;

public interface IBackupService
{
    string SetupBackupFolder();
    void BackupFile(string accountName, string configPath);
}