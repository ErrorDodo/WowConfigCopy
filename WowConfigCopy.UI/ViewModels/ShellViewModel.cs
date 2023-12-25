using System.Collections.Generic;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using Microsoft.Extensions.Logging;
using Prism.Navigation;
using WowConfigCopy.Common.Models;
using WowConfigCopy.UI.Dto;
using WowConfigCopy.UI.Extensions;
using WowConfigCopy.UI.Interfaces;

namespace WowConfigCopy.UI.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private readonly ILogger<ShellViewModel> _logger;
        private readonly INavigationService _navigationService;
        private readonly IWindowService _windowService;

        private string _applicationName = "WoW Config Helper";
        public string CurrentViewName => _navigationService.GetCurrentViewName();
        public BindableBase CurrentViewModel => _navigationService.GetCurrentViewModel();

        public string ApplicationName
        {
            get => _applicationName;
            set => SetProperty(ref _applicationName, value);
        }

        public DebounceCommand ExitCommand { get; private set; }
        public DebounceCommand SaveCommand { get; private set; }
        public DebounceCommand GoForwardCommand { get; private set; }
        public DebounceCommand GoBackwardCommand { get; private set; }
        public DebounceCommand MinimizeCommand { get; private set; }
        public DebounceCommand MaximizeCommand { get; private set; }
        public DebounceCommand<string> NavigateCommand { get; private set; }

        public ShellViewModel(ILogger<ShellViewModel> logger, INavigationService navigationService, IWindowService windowService)
        {
            _logger = logger;
            _navigationService = navigationService;
            _windowService = windowService;
            
            _navigationService.NavigationStateChanged += OnNavigationStateChanged;

            SaveCommand = new DebounceCommand(SaveSettings);
            ExitCommand = new DebounceCommand(() => _windowService.CloseWindow());
            MinimizeCommand = new DebounceCommand(() => _windowService.MinimizeWindow());
            MaximizeCommand = new DebounceCommand(() => _windowService.MaximizeRestoreWindow());
            NavigateCommand = new DebounceCommand<string>(Navigate);
            GoBackwardCommand = new DebounceCommand(_navigationService.GoBackward, _navigationService.CanGoBackward);
            GoForwardCommand = new DebounceCommand(_navigationService.GoForward, _navigationService.CanGoForward);
        }

        private void Navigate(string viewName)
        {
            _navigationService.NavigateTo(viewName);
            RaisePropertyChanged(nameof(CurrentViewModel));
            RaisePropertyChanged(nameof(CurrentViewName));
        }

        public void NavigateToRealmDetails(RegionDetails regionDetails)
        {
            var parameters = new NavigationParameters
            {
                { "region", regionDetails.RealmName }, 
                { "accounts", regionDetails.Accounts }
            };
            _navigationService.NavigateTo("RegionDetails", parameters);
        }
        
        public void NavigateToAccountDetails(RealmAccountsModel model)
        {
            var parameters = new NavigationParameters
            {
                { "accountName", model.AccountName },
                { "configLocation", model.ConfigPath }
            };
            _navigationService.NavigateTo("AccountDetails", parameters);
        }

        private void OnNavigationStateChanged()
        {
            RaisePropertyChanged(nameof(CurrentViewModel));
            RaisePropertyChanged(nameof(CurrentViewName));
            GoBackwardCommand.RaiseCanExecuteChanged();
            GoForwardCommand.RaiseCanExecuteChanged();
        }

        private void SaveSettings()
        {
            _logger.LogInformation("Saving settings");
        }

        ~ShellViewModel()
        {
            _navigationService.NavigationStateChanged -= OnNavigationStateChanged;
        }
        
        public void Initialize()
        {
            Navigate("Regions");
        }
    }
}
