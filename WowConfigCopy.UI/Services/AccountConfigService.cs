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

        return new ObservableCollection<AccountsModel>();
    }

}