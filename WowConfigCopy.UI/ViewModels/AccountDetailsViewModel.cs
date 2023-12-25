using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Mvvm;
using Prism.Navigation;
using WowConfigCopy.Common.Models;
using WowConfigCopy.UI.Interfaces;

namespace WowConfigCopy.UI.ViewModels;

public class AccountDetailsViewModel : BindableBase, IInitializeWithParameters
{
    private readonly ILogger<AccountDetailsViewModel> _logger;
    private readonly IAccountConfigService _accountConfigService;
    
    private string _accountName = string.Empty;
    
    public string AccountName
    {
        get => _accountName;
        set => SetProperty(ref _accountName, value);
    }
    
    private ObservableCollection<RealmAccountsModel> _realmAccounts = new();
    
    public ObservableCollection<RealmAccountsModel> RealmAccounts
    {
        get => _realmAccounts;
        set => SetProperty(ref _realmAccounts, value);
    }

    public AccountDetailsViewModel(ILogger<AccountDetailsViewModel> logger, IAccountConfigService accountConfigService)
    {
        _logger = logger;
        _accountConfigService = accountConfigService;
    }

    public void InitializeWithParameters(NavigationParameters parameters)
    {
        _logger.LogInformation("Initializing AccountDetailsViewModel with parameters");
        
        if (parameters.TryGetValue("accountName", out string accountName))
        {
            _logger.LogInformation($"Received account parameter: {accountName}");
        }
        else
        {
            _logger.LogWarning("Account parameter not found");
        }
        
        LoadRealmAccounts().ConfigureAwait(true);
    }
    
    private async Task LoadRealmAccounts()
    {
        _logger.LogInformation("Loading realm accounts");
        RealmAccounts = await _accountConfigService.GetRealmsAccountsAsync();
    }
}