using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Mvvm;
using WowConfigCopy.Common.Models;
using WowConfigCopy.UI.Interfaces;

namespace WowConfigCopy.UI.ViewModels
{
    public class FoldersViewModel : BindableBase
    {
        private readonly IAccountConfigService _accountConfigService;
        private readonly ILogger<FoldersViewModel> _logger;
        private readonly ShellViewModel _shellViewModel;
        private ObservableCollection<AccountModel> _accounts = new();

        public ObservableCollection<AccountModel> Accounts
        {
            get => _accounts;
            set => SetProperty(ref _accounts, value);
        }
        
        
        public DelegateCommand<string> NavigateToRegionDetailsCommand { get; private set; }

        public FoldersViewModel(ILogger<FoldersViewModel> logger, IAccountConfigService accountConfigService, ShellViewModel shellViewModel)
        {
            _logger = logger;
            _accountConfigService = accountConfigService;
            _shellViewModel = shellViewModel;
            InitializeAsync();
            
            
            NavigateToRegionDetailsCommand = new DelegateCommand<string>(NavigateToRegionDetails);
        }

        private void NavigateToRegionDetails(string regionName)
        {
            _shellViewModel.NavigateToRealmDetails(regionName);
        }

        private async Task InitializeAsync()
        {
            try
            {
                Accounts = await _accountConfigService.ReadConfigAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load accounts.");
            }
        }
    }
}