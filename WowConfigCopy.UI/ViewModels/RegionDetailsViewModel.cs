using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using Prism.Mvvm;
using Prism.Navigation;
using WowConfigCopy.Common.Models;
using WowConfigCopy.UI.Interfaces;

namespace WowConfigCopy.UI.ViewModels;

public class RegionDetailsViewModel : BindableBase, IInitializeWithParameters
{
    private ILogger<RegionDetailsViewModel> _logger;
    private string _regionName = string.Empty;
    private ObservableCollection<RealmAccountsModel> _accounts;

    public string RegionName
    {
        get => _regionName;
        set => SetProperty(ref _regionName, value);
    }

    public ObservableCollection<RealmAccountsModel> Accounts
    {
        get => _accounts;
        set => SetProperty(ref _accounts, value);
    }

    public RegionDetailsViewModel(ILogger<RegionDetailsViewModel> logger)
    {
        _logger = logger;
    }

    public void InitializeWithParameters(NavigationParameters parameters)
    {
        _logger.LogInformation("Initializing RegionDetailsViewModel with parameters");
        
        if (parameters.TryGetValue("region", out string region))
        {
            RegionName = region;
            _logger.LogInformation($"Received region parameter: {region}");
        }
        else
        {
            _logger.LogWarning("Region parameter not found");
        }
        
        if (parameters.TryGetValue("accounts", out ObservableCollection<RealmAccountsModel> accounts))
        {
            Accounts = accounts;
            _logger.LogInformation($"Received accounts parameter. Number of accounts: {accounts.Count}");
            foreach (var account in accounts)
            {
                _logger.LogInformation($"Account: {account.AccountName}, ConfigPath: {account.ConfigPath}");
            }
        }
        else
        {
            _logger.LogWarning("Accounts parameter not found");
        }
    }

}