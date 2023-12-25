using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WowConfigCopy.Common.Interfaces;
using WowConfigCopy.Common.Models;
using WowConfigCopy.UI.Interfaces;

namespace WowConfigCopy.UI.Services
{
    public class AccountConfigService : IAccountConfigService
    {
        private readonly IConfigFiles _configFiles;
        private readonly ILogger<AccountConfigService> _logger;

        public AccountConfigService(IConfigFiles configFiles, ILogger<AccountConfigService> logger)
        {
            _configFiles = configFiles;
            _logger = logger;
        }
        
        public async Task<ObservableCollection<AccountModel>> ReadConfigAsync()
        {
            var wowVersion = "SoD";
            var accounts = await _configFiles.ReadConfigFilesAsync(wowVersion);

            if (accounts == null || !accounts.Any())
            {
                _logger.LogWarning("No accounts found for WoW version: {WowVersion}", wowVersion);
                return new ObservableCollection<AccountModel>();
            }
            
            foreach (var account in accounts)
            {
                _logger.LogInformation("Account: {Account}", account.FolderName);
                foreach (var realm in account.Realms)
                {
                    _logger.LogInformation("Realm: {Realm}", realm.RealmName);
                    foreach (var character in realm.Accounts)
                    {
                        _logger.LogInformation("Character: {Character}", character.AccountName);
                    }
                }
            }

            return accounts;
        }
        
        public async Task<ObservableCollection<RealmAccountsModel>> GetRealmsAccountsAsync()
        {
            var realmAccounts = await _configFiles.GetRealmsAccounts();

            if (!realmAccounts.Any())
            {
                _logger.LogWarning("No realm accounts found.");
                return new ObservableCollection<RealmAccountsModel>();
            }
            
            foreach (var realmAccount in realmAccounts)
            {
                _logger.LogInformation("Realm Account: {AccountName}, Config Path: {ConfigPath}", 
                    realmAccount.AccountName, realmAccount.ConfigPath);
            }

            return realmAccounts;
        }
    }
}