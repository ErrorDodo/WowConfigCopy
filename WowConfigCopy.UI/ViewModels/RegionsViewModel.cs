using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Mvvm;
using WowConfigCopy.Common.Models;
using WowConfigCopy.UI.Dto;
using WowConfigCopy.UI.Interfaces;

namespace WowConfigCopy.UI.ViewModels
{
    public class RegionsViewModel : BindableBase
    {
        private readonly IAccountConfigService _accountConfigService;
        private readonly ILogger<RegionsViewModel> _logger;
        private readonly ShellViewModel _shellViewModel;
        
        private ObservableCollection<RegionDetails> _distinctRealms = new();
        public ObservableCollection<RegionDetails> DistinctRealms
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
        
        public DelegateCommand<RegionDetails> NavigateToRegionDetailsCommand { get; private set; }
        
        public RegionsViewModel(ILogger<RegionsViewModel> logger, IAccountConfigService accountConfigService, ShellViewModel shellViewModel)
        {
            _logger = logger;
            _accountConfigService = accountConfigService;
            _shellViewModel = shellViewModel;
            InitializeAsync().ConfigureAwait(true);
            
            
            NavigateToRegionDetailsCommand = new DelegateCommand<RegionDetails>(NavigateToRegionDetails);
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
                    .Select(g => new RegionDetails
                    {
                        RealmName = g.Key,
                        Accounts = new ObservableCollection<RealmAccountsModel>(g.SelectMany(x => x.Accounts))
                    })
                    .OrderBy(regionDetails => regionDetails.RealmName);

                DistinctRealms = new ObservableCollection<RegionDetails>(realmGroups);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load accounts.");
            }
        }
    }
}