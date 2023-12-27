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

    public void ExtractBackup(FileInfo? backupFile)
    {
        // This method should not be used until I figure out how to make it look nice and work properly
        // I'm not sure if I want to overwrite the files here or inside a different logic file/viewmodel
        if (backupFile is not {Exists: true})
        {
            _logger.LogError($"Backup file does not exist");
            return;
        }

        try
        {
            ZipFile.ExtractToDirectory(backupFile.FullName, _backupFolder);
            _logger.LogInformation($"Extracted backup file {backupFile.Name} to {_backupFolder}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while extracting backup file {backupFile.Name}");
        }
    }

    
    public IEnumerable<FileInfo> GetBackupFiles(string accountName)
    {
        var accountBackupFolder = Path.Combine(_backupFolder, accountName);
        if (!Directory.Exists(accountBackupFolder))
        {
            _logger.LogInformation($"No backup folder found for account {accountName}");
            return Enumerable.Empty<FileInfo>();
        }

        var directoryInfo = new DirectoryInfo(accountBackupFolder);
        return directoryInfo.GetFiles("*.zip").OrderByDescending(f => f.CreationTime);
    }


    public void BackupFile(string accountName, string configPath)
    {
        var accountBackupFolder = Path.Combine(_backupFolder, accountName);
        if (!Directory.Exists(accountBackupFolder))
        {
            Directory.CreateDirectory(accountBackupFolder);
            _logger.LogInformation($"Created backup folder for account {accountName}");
        }

        var backupFileName = $"{Guid.NewGuid()}.zip";
        var backupFile = Path.Combine(accountBackupFolder, backupFileName);

        var filesToBackup = GetFilesToBackup(configPath);
        var configFileModels = filesToBackup as ConfigFileModel[] ?? filesToBackup.ToArray();
        CopyFilesToBackupFolder(configFileModels);

        CreateZipFile(backupFile, configFileModels);
        _logger.LogInformation($"Created backup file at {backupFile}");

        DeleteCopiedFiles(configFileModels);
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
    
    public bool HasRecentBackup(string accountName)
    {
        var latestBackup = FindLatestBackup(accountName);
        if (latestBackup == null)
        {
            return false;
        }

        var backupAge = DateTime.Now - latestBackup.CreationTime;
        return backupAge < TimeSpan.FromDays(1);
    }

    private FileInfo? FindLatestBackup(string accountName)
    {
        var accountBackupFolder = Path.Combine(_backupFolder, accountName);

        if (!Directory.Exists(accountBackupFolder))
        {
            _logger.LogInformation($"No backup folder found for account {accountName}");
            return null;
        }

        var directoryInfo = new DirectoryInfo(accountBackupFolder);
        return directoryInfo.GetFiles("*.zip").MaxBy(f => f.CreationTime);
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