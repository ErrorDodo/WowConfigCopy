using System.IO.Compression;
using Microsoft.Extensions.Logging;
using WowConfigCopy.Common.Helpers;
using WowConfigCopy.Common.Interfaces;
using WowConfigCopy.Common.Models;

namespace WowConfigCopy.Common.Services;

public class BackupService : IBackupService
{
    private readonly ILogger<BackupService> _logger;
    private readonly FileHelpers _fileHelpers = new();
    private readonly string _backupFolder;

    public BackupService(ILogger<BackupService> logger)
    {
        _logger = logger;
        _backupFolder = SetupBackupFolder();
    }
    
    public string SetupBackupFolder()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var backupFolder = Path.Combine(appData, "WowConfigCopyBackups");
        if (!Directory.Exists(backupFolder))
        {
            Directory.CreateDirectory(backupFolder);
            _logger.LogInformation($"Created backup folder at {backupFolder}");
        }
        return backupFolder;
    }

    public void ExtractBackup(string accountName)
    {
        // TODO: Update this function to handle more than one backup file, this will most likely entail a new UI for selecting which backup to restore
        var backupFile = Path.Combine(_backupFolder, $"{accountName} - Backup.zip");
        if (!File.Exists(backupFile))
        {
            _logger.LogError($"Backup file {backupFile} does not exist");
            return;
        }
        
        ZipFile.ExtractToDirectory(backupFile, _backupFolder);
        _logger.LogInformation($"Extracted backup file {backupFile} to {_backupFolder}");
    }

    public void BackupFile(string accountName, string configPath)
    {
        var lastBackupFile = FindLatestBackup(accountName);
        if (lastBackupFile != null && (DateTime.Now - lastBackupFile.CreationTime).TotalMinutes < 5)
        {
            _logger.LogInformation($"A recent backup for {accountName} already exists. Skipping new backup.");
            return;
        }

        var timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
        var backupFileName = $"{accountName} {timestamp}.zip";
        var backupFile = Path.Combine(_backupFolder, backupFileName);

        var filesToBackup = GetFilesToBackup(configPath);
        var configFileModels = filesToBackup as ConfigFileModel[] ?? filesToBackup.ToArray();
        CopyFilesToBackupFolder(configFileModels);

        CreateZipFile(backupFile, configFileModels);
        _logger.LogInformation($"Created backup file at {backupFile}");

        DeleteCopiedFiles(configFileModels);
    }
    
    public bool HasRecentBackup(string accountName)
    {
        var lastBackupFile = FindLatestBackup(accountName);
        return lastBackupFile != null && (DateTime.Now - lastBackupFile.CreationTime).TotalMinutes < 5;
    }

    private FileInfo? FindLatestBackup(string accountName)
    {
        var directoryInfo = new DirectoryInfo(_backupFolder);
        return directoryInfo.GetFiles($"{accountName} *.zip").MaxBy(f => f.CreationTime);
    }
    
    private void CreateZipFile(string backupFile, IEnumerable<ConfigFileModel> configFileModels)
    {
        using var zipFileStream = new FileStream(backupFile, FileMode.Create);
        using var archive = new ZipArchive(zipFileStream, ZipArchiveMode.Create);
        foreach (var file in configFileModels)
        {
            var backupPath = Path.Combine(_backupFolder, file.Name);
            archive.CreateEntryFromFile(backupPath, file.Name);
            _logger.LogInformation($"Added {file.Name} to zip file");
        }
    }


    private void DeleteExistingBackupFile(string backupFile)
    {
        if (File.Exists(backupFile))
        {
            File.Delete(backupFile);
            _logger.LogInformation($"Deleted existing backup file at {backupFile}");
        }
    }

    private IEnumerable<ConfigFileModel> GetFilesToBackup(string configPath)
    {
        var files = _fileHelpers.GetFilesSafe(configPath);
        return files.Where(file => !file.EndsWith(".old", StringComparison.OrdinalIgnoreCase))
            .Select(file => new ConfigFileModel { Name = Path.GetFileName(file), Path = file, IsGlobal = false });
    }

    private void CopyFilesToBackupFolder(IEnumerable<ConfigFileModel> filesToBackup)
    {
        foreach (var file in filesToBackup)
        {
            var backupPath = Path.Combine(_backupFolder, file.Name);
            File.Copy(file.Path, backupPath);
            _logger.LogInformation($"Copied {file.Name} to {backupPath}");
        }
    }

    private void DeleteCopiedFiles(IEnumerable<ConfigFileModel> filesToBackup)
    {
        foreach (var file in filesToBackup)
        {
            var backupPath = Path.Combine(_backupFolder, file.Name);
            File.Delete(backupPath);
            _logger.LogInformation($"Deleted {backupPath}");
        }
    }
}