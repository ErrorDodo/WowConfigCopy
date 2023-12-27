using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using WowConfigCopy.Common.Models;
using WowConfigCopy.UI.Interfaces;

namespace WowConfigCopy.UI.ViewModels
{
    public class RegionDetailsViewModel : BindableBase, IInitializeWithParameters
    {
        private readonly ILogger<RegionDetailsViewModel> _logger;
        private string _regionName = string.Empty;
        private readonly INavigationService _navigationService;
        
        private ObservableCollection<RealmAccountsModel> _realmAccounts;

        public string RegionName
        {
            get => _regionName;
            set => SetProperty(ref _regionName, value);
        }
        
        public ObservableCollection<RealmAccountsModel> RealmAccounts
        {
            get => _realmAccounts;
            set => SetProperty(ref _realmAccounts, value);
        }
        
        public DelegateCommand<RealmAccountsModel> ViewAccountDetailsCommand { get; }

        public RegionDetailsViewModel(ILogger<RegionDetailsViewModel> logger, INavigationService navigationService)
        {
            _logger = logger;
            _navigationService = navigationService;
            ViewAccountDetailsCommand = new DelegateCommand<RealmAccountsModel>(ViewAccountDetails);
        }
        
        private void ViewAccountDetails(RealmAccountsModel model)
        {
            _logger.LogInformation($"Moving to account details of account: {model.AccountName}");
            _navigationService.NavigateToAccountDetails(model);
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
                RealmAccounts = accounts;
                _logger.LogInformation($"Received accounts parameter. Number of accounts: {accounts.Count}");
            }
            else
            {
                _logger.LogWarning("Accounts parameter not found");
            }
        }
    }
}
