using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using Prism.Mvvm;
using WowConfigCopy.UI.Interfaces;
using WowConfigCopy.UI.Models;

namespace WowConfigCopy.UI.ViewModels;

public class AccountsViewModel : BindableBase
{
    private readonly IAccountConfigService _accountConfigService;
    private readonly ILogger<AccountsViewModel> _logger;
    private ObservableCollection<AccountsModel> _accounts = new();

    public ObservableCollection<AccountsModel> Accounts
    {
        get => _accounts;
        set => SetProperty(ref _accounts, value);
    }
    public AccountsViewModel(ILogger<AccountsViewModel> logger, IAccountConfigService accountConfigService)
    {
        _logger = logger;
        _accountConfigService = accountConfigService;
    }
}