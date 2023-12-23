using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Extensions.Logging;
using WowConfigCopy.Common.Interfaces;
using WowConfigCopy.UI.Interfaces;
using WowConfigCopy.UI.Models;

namespace WowConfigCopy.UI.Services;

public class AccountConfigService : IAccountConfigService
{
    private readonly IConfigFiles _configFiles;
    private readonly ILogger<AccountConfigService> _logger;

    public AccountConfigService(IConfigFiles configFiles, ILogger<AccountConfigService> logger)
    {
        _configFiles = configFiles;
        _logger = logger;
    }
    
    public ObservableCollection<AccountsModel> ReadConfig()
    {
        var wowVersion = "SoD";
        var accounts = _configFiles.ReadConfigFiles(wowVersion);

        if (accounts == null || !accounts.Any())
        {
            _logger.LogWarning("No accounts found for WoW version: {WowVersion}", wowVersion);
            return new ObservableCollection<AccountsModel>();
        }
        
        var accountsModel = new ObservableCollection<AccountsModel>();
        foreach (var account in accounts)
        {
            // Log all the details
            _logger.LogDebug("Account: {Account}", account.FolderName);
            foreach (var realm in account.Realms)
            {
                _logger.LogDebug("Realm: {Realm}", realm.RealmName);
                foreach (var character in realm.Accounts)
                {
                    _logger.LogDebug("Character: {Character}", character.AccountName);
                }
            }
        }

        return new ObservableCollection<AccountsModel>();
    }

}