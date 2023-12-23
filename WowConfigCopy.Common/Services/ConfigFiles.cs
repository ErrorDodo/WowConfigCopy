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
        
        public ObservableCollection<AccountModel> ReadConfigFiles(string wowVersion)
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

            var accounts = ParseAccountFolders(accountPaths);
            _logger.LogInformation("Config file parsing completed.");
            return new ObservableCollection<AccountModel>(accounts);
        }
    
        private IEnumerable<AccountModel> ParseAccountFolders(string path)
        {
            var accountFolders = Directory.GetDirectories(path);
            var accounts = new List<AccountModel>();

            foreach (var accountFolderPath in accountFolders)
            {
                var accountFolderName = Path.GetFileName(accountFolderPath);
                var realms = ParseRealmFolders(accountFolderPath);

                accounts.Add(new AccountModel
                {
                    FolderName = accountFolderName,
                    Realms = realms
                });
            }
            
            return accounts;
        }

        private ObservableCollection<RealmModel> ParseRealmFolders(string path)
        {
            var realmFolders = Directory.GetDirectories(path);
            var realms = new List<RealmModel>();

            foreach (var realmPath in realmFolders)
            {
                var realmName = Path.GetFileName(realmPath);
                var parts = realmName.Split(new[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2) continue;

                var realm = new RealmModel
                {
                    RealmName = parts[0].Trim(),
                    RealmRegion = parts[1].Trim(),
                    Accounts = ParseAccountsInRealm(realmPath)
                };

                realms.Add(realm);
            }
            
            return new ObservableCollection<RealmModel>(realms);
        }

        private static ObservableCollection<RealmAccountsModel> ParseAccountsInRealm(string realmPath)
        {
            var accountFolders = Directory.GetDirectories(realmPath);
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
