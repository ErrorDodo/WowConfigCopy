using System.Linq;
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

        if (accounts == null || !accounts.Any())
        {
            _logger.LogWarning("No accounts found for WoW version: {WowVersion}", wowVersion);
            return;
        }

        _logger.LogInformation("Reading configuration for WoW version: {WowVersion}", wowVersion);

        foreach (var account in accounts)
        {
            _logger.LogInformation("Account: {AccountName}, Realms Count: {RealmsCount}", 
                account.FolderName, account.Realms.Count);

            foreach (var realm in account.Realms)
            {
                _logger.LogInformation("Realm: {RealmName}, Region: {RealmRegion}, Characters Count: {CharactersCount}", 
                    realm.RealmName, realm.RealmRegion, realm.Accounts.Count);

                foreach (var character in realm.Accounts)
                {
                    _logger.LogInformation("Character: {CharacterName}, Config Path: {ConfigPath}", 
                        character.AccountName, character.ConfigPath);
                }
            }
        }
    }

}