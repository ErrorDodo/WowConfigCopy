using Prism.Commands;
using Prism.Mvvm;
using Microsoft.Extensions.Logging;
using Prism.Events;
using Prism.Ioc;
using WowConfigCopy.UI.Interfaces;

namespace WowConfigCopy.UI.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private readonly ILogger<ShellViewModel> _logger;
        private readonly INavigationService _navigationService;
        private readonly IWindowService _windowService;
        private readonly NotificationsViewModel _notificationsViewModel;

        private string _applicationName = "WoW Config Helper";
        public string CurrentViewName => _navigationService.GetCurrentViewName();
        public BindableBase CurrentViewModel => _navigationService.GetCurrentViewModel();

        public string ApplicationName
        {
            get => _applicationName;
            set => SetProperty(ref _applicationName, value);
        }

        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand GoForwardCommand { get; private set; }
        public DelegateCommand GoBackwardCommand { get; private set; }
        public DelegateCommand MinimizeCommand { get; private set; }
        public DelegateCommand MaximizeCommand { get; private set; }
        public DelegateCommand<string> NavigateCommand { get; private set; }

        public ShellViewModel(ILogger<ShellViewModel> logger, INavigationService navigationService, IWindowService windowService, IEventAggregator eventAggregator, IContainerProvider containerProvider)
        {
            _logger = logger;
            _navigationService = navigationService;
            _windowService = windowService;
            
            _notificationsViewModel = containerProvider.Resolve<NotificationsViewModel>();

            _navigationService.NavigationStateChanged += OnNavigationStateChanged;

            SaveCommand = new DelegateCommand(SaveSettings);
            ExitCommand = new DelegateCommand(() => _windowService.CloseWindow());
            MinimizeCommand = new DelegateCommand(() => _windowService.MinimizeWindow());
            MaximizeCommand = new DelegateCommand(() => _windowService.MaximizeRestoreWindow());
            NavigateCommand = new DelegateCommand<string>(Navigate);
            GoBackwardCommand = new DelegateCommand(_navigationService.GoBackward, () => _navigationService.CanGoBackward());
            GoForwardCommand = new DelegateCommand(_navigationService.GoForward, () => _navigationService.CanGoForward());
        }

        private void Navigate(string viewName)
        {
            _navigationService.NavigateTo(viewName);
            RaisePropertyChanged(nameof(CurrentViewModel));
            RaisePropertyChanged(nameof(CurrentViewName));
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
