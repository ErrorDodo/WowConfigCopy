using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using WowConfigCopy.Common.Models;
using WowConfigCopy.UI.Interfaces;
using WowConfigCopy.UI.Models;

namespace WowConfigCopy.UI.ViewModels
{
    public class RegionDetailsViewModel : BindableBase, IInitializeWithParameters
    {
        private readonly ILogger<RegionDetailsViewModel> _logger;
        private string _regionName = string.Empty;
        
        private ObservableCollection<AccountVisibilityModel> _accountVisibilityModels;

        public string RegionName
        {
            get => _regionName;
            set => SetProperty(ref _regionName, value);
        }
        
        public ObservableCollection<AccountVisibilityModel> AccountVisibilityModels
        {
            get => _accountVisibilityModels;
            set => SetProperty(ref _accountVisibilityModels, value);
        }
        
        public DelegateCommand<AccountVisibilityModel> ToggleButtonCommand { get; private set; }

        public RegionDetailsViewModel(ILogger<RegionDetailsViewModel> logger)
        {
            _logger = logger;
            ToggleButtonCommand = new DelegateCommand<AccountVisibilityModel>(ExecuteToggleButtonCommand);
        }
        
        // This function is for testing purposes only
        private void ExecuteToggleButtonCommand(AccountVisibilityModel account)
        {
            _logger.LogInformation($"ToggleButton pressed for account: {account.AccountName}");
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
                AccountVisibilityModels = new ObservableCollection<AccountVisibilityModel>(
                    accounts.Select(account => new AccountVisibilityModel(account)));
                _logger.LogInformation($"Received accounts parameter. Number of accounts: {accounts.Count}");
            }
            else
            {
                _logger.LogWarning("Accounts parameter not found");
            }
        }
    }
}
