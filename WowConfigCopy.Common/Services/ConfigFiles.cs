using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
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

            var accounts = await ParseAccountFoldersAsync(accountPaths);
            _logger.LogInformation("Config file parsing completed.");
            return new ObservableCollection<AccountModel>(accounts);
        }

        private async Task<IEnumerable<AccountModel>> ParseAccountFoldersAsync(string path)
        {
            var accountFolders = await Task.Run(() => Directory.GetDirectories(path));
            var accounts = new List<AccountModel>();

            foreach (var accountFolderPath in accountFolders)
            {
                var accountFolderName = Path.GetFileName(accountFolderPath);
                var realms = await ParseRealmFoldersAsync(accountFolderPath);

                accounts.Add(new AccountModel
                {
                    FolderName = accountFolderName,
                    Realms = realms
                });
            }

            return accounts;
        }

        private async Task<ObservableCollection<RealmModel>> ParseRealmFoldersAsync(string path)
        {
            var realmFolders = await Task.Run(() => Directory.GetDirectories(path));
            var realms = new List<RealmModel>();

            foreach (var realmPath in realmFolders)
            {
                var realmName = Path.GetFileName(realmPath);
                var parts = realmName.Split(new[] { '(', ')' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2) continue;

                var realm = new RealmModel
                {
                    RealmName = parts[0].Trim(),
                    RealmRegion = parts[1].Trim(),
                    Accounts = await ParseAccountsInRealmAsync(realmPath)
                };

                realms.Add(realm);
            }

            return new ObservableCollection<RealmModel>(realms);
        }

        private async Task<ObservableCollection<RealmAccountsModel>> ParseAccountsInRealmAsync(string realmPath)
        {
            var accountFolders = await Task.Run(() => Directory.GetDirectories(realmPath));
            var accounts = new List<RealmAccountsModel>();

            foreach (var accountPath in accountFolders)
            {
                var accountName = Path.GetFileName(accountPath);
                accounts.Add(new RealmAccountsModel
                {
                    AccountName = accountName,
                    ConfigPath = accountPath
                });
            }

            return new ObservableCollection<RealmAccountsModel>(accounts);
        }
    }
}
