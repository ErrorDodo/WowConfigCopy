using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using WowConfigCopy.Common.Interfaces;
using WowConfigCopy.Common.Models;
using WowConfigCopy.Common.Helpers;

namespace WowConfigCopy.Common.Services
{
    public class ConfigFiles : IConfigFiles
    {
        private readonly ILogger<ConfigFiles> _logger;
        private readonly IRegistryHelper _registryHelper;
        private readonly IBackupService _backupService;
        private readonly FileHelpers _fileHelpers = new();

        public ConfigFiles(ILogger<ConfigFiles> logger, IRegistryHelper registryHelper, IBackupService backupService)
        {
            _logger = logger;
            _registryHelper = registryHelper;
            _backupService = backupService;
        }

        public async Task<ObservableCollection<RealmAccountsModel>> GetRealmsAccounts()
        {
            _logger.LogInformation("Retrieving Realm Accounts");

            var wowInstallPath = _registryHelper.GetWowInstallPath();
            if (string.IsNullOrEmpty(wowInstallPath))
            {
                _logger.LogWarning("WoW installation path not found.");
                return new ObservableCollection<RealmAccountsModel>();
            }

            var accountPaths = Path.Combine(wowInstallPath, "WTF", "Account");
            var accountFolders = _fileHelpers.GetDirectoriesSafe(accountPaths);
    
            // Debugging: Log each account folder found
            var enumerable = accountFolders as string[] ?? accountFolders.ToArray();
            foreach (var folder in enumerable)
            {
                _logger.LogDebug($"Processing account folder: {folder}");
            }

            var realmAccountTasks = enumerable.SelectMany(accountFolder =>
                _fileHelpers.GetDirectoriesSafe(accountFolder)
                    .Where(realmFolder => !Path.GetFileName(realmFolder).Equals("SavedVariables", StringComparison.OrdinalIgnoreCase))
                    .Select(ParseAccountsInRealmAsync)).ToList();

            // Debugging: Log the count of tasks
            _logger.LogDebug($"Number of tasks created: {realmAccountTasks.Count}");

            var allResults = await Task.WhenAll(realmAccountTasks);
            var realmAccounts = allResults.SelectMany(x => x).ToList();

            // Debugging: Log each realm account found
            foreach (var account in realmAccounts)
            {
                _logger.LogDebug($"Found realm account: {account.AccountName}, Path: {account.ConfigPath}");
            }

            return new ObservableCollection<RealmAccountsModel>(realmAccounts);
        }

        public async Task<ObservableCollection<ConfigFileModel>> GetConfigFiles(string configPath, bool getAccountConfigFiles = false)
        {
            _logger.LogInformation($"Retrieving Config Files for {configPath}");

            var files = _fileHelpers.GetFilesSafe(configPath);

            var allFiles = (from file in files where !file.EndsWith(".old", StringComparison.OrdinalIgnoreCase) select new ConfigFileModel {Name = Path.GetFileName(file), Path = file, IsGlobal = false}).ToList();

            if (!getAccountConfigFiles) return new ObservableCollection<ConfigFileModel>(allFiles);
            var accountDirectory = Directory.GetParent(Directory.GetParent(configPath).FullName).FullName;
            _logger.LogInformation($"Retrieving Account Config Files for {accountDirectory}");

            var accountFiles = _fileHelpers.GetFilesSafe(accountDirectory);
            allFiles.AddRange(from file in accountFiles where !file.EndsWith(".old", StringComparison.OrdinalIgnoreCase) select new ConfigFileModel {Name = Path.GetFileName(file), Path = file, IsGlobal = true});

            return new ObservableCollection<ConfigFileModel>(allFiles);
        }


        
        public async Task<ObservableCollection<AccountModel>> ReadConfigFilesAsync(string wowVersion)
        {
            _logger.LogInformation("Reading WoW config files for version: {WowVersion}", wowVersion);
            var wowInstallPath = _registryHelper.GetWowInstallPath();
            
            _logger.LogInformation("Setting up backup folder");
            _backupService.SetupBackupFolder();

            if (string.IsNullOrEmpty(wowInstallPath))
            {
                _logger.LogWarning("WoW installation path not found.");
                return new ObservableCollection<AccountModel>();
            }

            var accountPaths = Path.Combine(wowInstallPath, "WTF", "Account");
            if (!Directory.Exists(accountPaths))
            {
                _logger.LogWarning("WoW account path not found at: {AccountPaths}", accountPaths);
                return new ObservableCollection<AccountModel>();
            }

            var accountFolders = _fileHelpers.GetDirectoriesSafe(accountPaths);
            var tasks = accountFolders.Select(ParseAccountFolderAsync);
            var accounts = await Task.WhenAll(tasks);

            _logger.LogInformation("Config file parsing completed.");
            
            return new ObservableCollection<AccountModel>(accounts.Where(_ => true));
        }

        private async Task<AccountModel> ParseAccountFolderAsync(string accountFolderPath)
        {
            var accountFolderName = Path.GetFileName(accountFolderPath);
            var realms = await ParseRealmFoldersAsync(accountFolderPath);

            return new AccountModel
            {
                FolderName = accountFolderName,
                Realms = realms
            };
        }

        private async Task<ObservableCollection<RealmModel>> ParseRealmFoldersAsync(string path)
        {
            var realmFolders = await Task.Run(() => _fileHelpers.GetDirectoriesSafe(path));
            var realms = realmFolders
                .Where(realmPath => !Path.GetFileName(realmPath).Equals("SavedVariables", StringComparison.OrdinalIgnoreCase))
                .Select(async realmPath =>
                {
                    var directoryName = Path.GetFileName(realmPath);
                    var realmRegion = ExtractRealmRegion(directoryName);
                    return new RealmModel
                    {
                        RealmName = directoryName,
                        RealmRegion = realmRegion,
                        Accounts = await ParseAccountsInRealmAsync(realmPath)
                    };
                });

            return new ObservableCollection<RealmModel>(await Task.WhenAll(realms));
        }

        private string ExtractRealmRegion(string directoryName)
        {
            var parts = directoryName.Split(new[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Length >= 2 ? parts[1].Trim() : string.Empty;
        }

        
        private async Task<ObservableCollection<RealmAccountsModel>> ParseAccountsInRealmAsync(string realmPath)
        {
            var accountFolders = await Task.Run(() => _fileHelpers.GetDirectoriesSafe(realmPath));
            var accounts = accountFolders.Select(accountPath => new RealmAccountsModel
            {
                AccountName = Path.GetFileName(accountPath),
                ConfigPath = accountPath
            });

            return new ObservableCollection<RealmAccountsModel>(accounts);
        }
    }
}
