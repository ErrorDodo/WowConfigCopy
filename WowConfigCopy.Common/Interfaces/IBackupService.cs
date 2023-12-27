namespace WowConfigCopy.Common.Interfaces;

public interface IBackupService
{
    string SetupBackupFolder();
    void BackupFile(string accountName, string configPath);
    bool HasRecentBackup(string accountName);
}