using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using Prism.Mvvm;
using Prism.Navigation;
using WowConfigCopy.Common.Interfaces;
using WowConfigCopy.Common.Models;
using WowConfigCopy.UI.Interfaces;

namespace WowConfigCopy.UI.ViewModels;

public class CopyFilesViewModel : BindableBase, IInitializeWithParameters
{
    private readonly ILogger<CopyFilesViewModel> _logger;
    private readonly IConfigFiles _configFiles;

    private string _accountName;
    private string _sourceConfigLocation;
    
    public string AccountName
    {
        get => _accountName;
        set => SetProperty(ref _accountName, value);
    }
    
    private ObservableCollection<RealmAccountsModel> _accounts;
    public ObservableCollection<RealmAccountsModel> Accounts
    {
        get => _accounts;
        set => SetProperty(ref _accounts, value);
    }

    private RealmAccountsModel _selectedAccount;
    public RealmAccountsModel SelectedAccount
    {
        get => _selectedAccount;
        set
        {
            if (SetProperty(ref _selectedAccount, value))
            {
                LogSelectedAccountInfo();
            }
        }
    }
    
    private void LogSelectedAccountInfo()
    {
        if (_selectedAccount != null)
        {
            _logger.LogInformation($"Selected account: {_selectedAccount.AccountName}");
            _logger.LogInformation($"Config location: {_selectedAccount.ConfigPath}");
        }
    }

    public CopyFilesViewModel(ILogger<CopyFilesViewModel> logger, IConfigFiles configFiles)
    {
        _logger = logger;
        _configFiles = configFiles;
        LoadAccounts();
    }

    private async void LoadAccounts()
    {
        var accounts = await _configFiles.GetRealmsAccounts();
        Accounts = new ObservableCollection<RealmAccountsModel>(accounts);
    }
    
    public void InitializeWithParameters(NavigationParameters parameters)
    {
        _logger.LogInformation("Initializing CopyFilesViewModel with parameters");
        
        if (parameters.TryGetValue("accountName", out string accountName))
        {
            _logger.LogInformation($"Account name: {accountName}");
            AccountName = accountName;
        }
        
        if (parameters.TryGetValue("configLocation", out string configLocation))
        {
            _logger.LogInformation($"Source path: {configLocation}");
            _sourceConfigLocation = configLocation;
        }
    }
}