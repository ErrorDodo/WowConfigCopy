using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Mvvm;
using WowConfigCopy.Common.Models;
using WowConfigCopy.UI.Interfaces;
using WowConfigCopy.UI.Models;

namespace WowConfigCopy.UI.ViewModels
{
    public class FoldersViewModel : BindableBase
    {
        private readonly IAccountConfigService _accountConfigService;
        private readonly ILogger<FoldersViewModel> _logger;
        private readonly ShellViewModel _shellViewModel;
        
        private ObservableCollection<Models.RegionDetails> _distinctRealms = new();
        public ObservableCollection<Models.RegionDetails> DistinctRealms
        {
            get => _distinctRealms;
            set => SetProperty(ref _distinctRealms, value);
        }
        
        private ObservableCollection<AccountModel> _accounts = new();

        public ObservableCollection<AccountModel> Accounts
        {
            get => _accounts;
            set => SetProperty(ref _accounts, value);
        }
        
        public DelegateCommand<Models.RegionDetails> NavigateToRegionDetailsCommand { get; private set; }
        
        public FoldersViewModel(ILogger<FoldersViewModel> logger, IAccountConfigService accountConfigService, ShellViewModel shellViewModel)
        {
            _logger = logger;
            _accountConfigService = accountConfigService;
            _shellViewModel = shellViewModel;
            InitializeAsync();
            
            
            NavigateToRegionDetailsCommand = new DelegateCommand<Models.RegionDetails>(NavigateToRegionDetails);
        }

        private void NavigateToRegionDetails(RegionDetails regionDetails)
        {
            _shellViewModel.NavigateToRealmDetails(regionDetails);
        }

        private async Task InitializeAsync()
        {
            try
            {
                Accounts = await _accountConfigService.ReadConfigAsync();
                var realmGroups = Accounts
                    .SelectMany(account => account.Realms)
                    .GroupBy(realm => realm.RealmName)
                    .Select(g => new Models.RegionDetails
                    {
                        RealmName = g.Key,
                        Accounts = new ObservableCollection<RealmAccountsModel>(g.SelectMany(x => x.Accounts))
                    });

                DistinctRealms = new ObservableCollection<Models.RegionDetails>(realmGroups);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load accounts.");
            }
        }
    }
}