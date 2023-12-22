using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using Microsoft.Extensions.Logging;
using WowConfigCopy.UI.Interfaces;

namespace WowConfigCopy.UI.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private readonly ILogger<ShellViewModel> _logger;
        private readonly INavigationService _navigationService;
        private readonly IViewModelFactory _viewModelFactory;

        private BindableBase _currentViewModel;
        private string _applicationName = "WoW Config Helper";
        private string _currentViewName = "Home";

        public BindableBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public string ApplicationName
        {
            get => _applicationName;
            set => SetProperty(ref _applicationName, value);
        }

        public string CurrentViewName
        {
            get => _currentViewName;
            set => SetProperty(ref _currentViewName, value);
        }

        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand GoForwardCommand { get; private set; }
        public DelegateCommand GoBackwardCommand { get; private set; }
        public DelegateCommand MinimizeCommand { get; private set; }
        public DelegateCommand MaximizeCommand { get; private set; }
        public DelegateCommand<string> NavigateCommand { get; private set; }

        public ShellViewModel(ILogger<ShellViewModel> logger, INavigationService navigationService, IViewModelFactory viewModelFactory)
        {
            _logger = logger;
            _navigationService = navigationService;
            _viewModelFactory = viewModelFactory;

            ExitCommand = new DelegateCommand(ExitApplication);
            SaveCommand = new DelegateCommand(SaveSettings);
            MinimizeCommand = new DelegateCommand(MinimizeWindow);
            MaximizeCommand = new DelegateCommand(MaximizeWindow);
            NavigateCommand = new DelegateCommand<string>(Navigate);

            GoForwardCommand = new DelegateCommand(
                    _navigationService.GoForward, 
                    () => _navigationService.CanGoForward)
                .ObservesProperty(() => _navigationService.CanGoForward);

            GoBackwardCommand = new DelegateCommand(
                    _navigationService.GoBack, 
                    () => _navigationService.CanGoBack)
                .ObservesProperty(() => _navigationService.CanGoBack);
        }

        private void Navigate(string viewName)
        {
            _logger.LogInformation("Navigating to {ViewName}", viewName);
            CurrentViewModel = _viewModelFactory.Create(viewName);
            CurrentViewName = viewName;
        }

        private void ExitApplication()
        {
            Application.Current.Shutdown();
        }

        private void SaveSettings()
        {
            _logger.LogInformation("Saving settings");
        }
        
        private void MinimizeWindow()
        {
            var window = Application.Current.MainWindow;
            if (window != null)
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        private void MaximizeWindow()
        {
            var window = Application.Current.MainWindow;
            if (window != null)
            {
                window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            }
        }
    }
}
