using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using WowConfigCopy.Common.Interfaces;
using WowConfigCopy.Common.Models;

namespace WowConfigCopy.Common.Services
{
    public class ConfigFiles : IConfigFiles
    {
        private readonly ILogger<ConfigFiles> _logger;
        private readonly IRegistryHelper _registryHelper;

        public ConfigFiles(ILogger<ConfigFiles> logger, IRegistryHelper registryHelper)
        {
            _logger = logger;
            _registryHelper = registryHelper;
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
            var accountFolders = GetDirectoriesSafe(accountPaths);

            var tasks = accountFolders.SelectMany(accountFolder =>
                GetDirectoriesSafe(accountFolder)
                    .Where(realmFolder => !Path.GetFileName(realmFolder).Equals("SavedVariables", StringComparison.OrdinalIgnoreCase))
                    .SelectMany(GetDirectoriesSafe)
                    .Select(ParseAccountsInRealmAsync)).ToList();

            var allResults = await Task.WhenAll(tasks);
            var realmAccounts = allResults.SelectMany(x => x).ToList();

            return new ObservableCollection<RealmAccountsModel>(realmAccounts);
        }



        public async Task<ObservableCollection<AccountModel>> ReadConfigFilesAsync(string wowVersion)
        {
            _logger.LogInformation("Reading WoW config files for version: {WowVersion}", wowVersion);
            var wowInstallPath = _registryHelper.GetWowInstallPath();

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

            var accountFolders = GetDirectoriesSafe(accountPaths);
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
            var realmFolders = await Task.Run(() => GetDirectoriesSafe(path));
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
            var accountFolders = await Task.Run(() => GetDirectoriesSafe(realmPath));
            var accounts = accountFolders.Select(accountPath => new RealmAccountsModel
            {
                AccountName = Path.GetFileName(accountPath),
                ConfigPath = accountPath
            });

            return new ObservableCollection<RealmAccountsModel>(accounts);
        }

        
        private IEnumerable<string> GetDirectoriesSafe(string path)
        {
            return Directory.Exists(path) ? Directory.GetDirectories(path) : Array.Empty<string>();
        }

    }
}
