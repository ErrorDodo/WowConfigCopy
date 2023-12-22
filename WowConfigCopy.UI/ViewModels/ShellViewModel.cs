using System.Windows;
using System.Windows.Input;
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

        public ICommand ExitCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand GoForwardCommand { get; }
        public ICommand GoBackwardCommand { get; }
        public DelegateCommand MinimizeCommand { get; }
        public DelegateCommand MaximizeCommand { get; }

        public ShellViewModel(ILogger<ShellViewModel> logger, INavigationService navigationService)
        {
            _logger = logger;
            _navigationService = navigationService;

            ExitCommand = new DelegateCommand(ExitApplication);
            SaveCommand = new DelegateCommand(SaveSettings);
            MinimizeCommand = new DelegateCommand(MinimizeWindow);
            MaximizeCommand = new DelegateCommand(MaximizeWindow);

            GoForwardCommand = new DelegateCommand(
                () => _navigationService.GoForward(),
                () => _navigationService.CanGoForward)
                .ObservesProperty(() => _navigationService.CanGoForward);

            GoBackwardCommand = new DelegateCommand(
                () => _navigationService.GoBack(),
                () => _navigationService.CanGoBack)
                .ObservesProperty(() => _navigationService.CanGoBack);
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
