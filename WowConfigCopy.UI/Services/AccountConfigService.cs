using Microsoft.Extensions.Logging;
using WowConfigCopy.Common.Interfaces;
using WowConfigCopy.UI.Interfaces;

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
    
    public void ReadConfig()
    {
        var wowVersion = "SoD";
        var accounts = _configFiles.ReadConfigFiles(wowVersion);
        
        foreach (var account in accounts)
        {
            _logger.LogInformation("Account: {AccountName}", account.FolderName);
            foreach (var realm in account.Realms)
            {
                _logger.LogInformation("Realm: {RealmName}", realm.RealmName);
                foreach (var character in realm.Accounts)
                {
                    _logger.LogInformation("Character: {CharacterName}", character.AccountName);
                }
            }
        }
    }
}