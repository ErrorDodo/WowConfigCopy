using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Prism.Mvvm;
using Prism.Navigation;
using WowConfigCopy.UI.Interfaces;

namespace WowConfigCopy.UI.Services
{
    public partial class NavigationService : INavigationService
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly ILogger<NavigationService> _logger;
        private readonly Stack<BindableBase> _backStack = new Stack<BindableBase>();
        private readonly Stack<BindableBase> _forwardStack = new Stack<BindableBase>();
        private BindableBase _currentViewModel;
        private string _currentViewName;
        private const int MaxStackSize = 10;

        public event Action NavigationStateChanged;

        public NavigationService(IViewModelFactory viewModelFactory, ILogger<NavigationService> logger)
        {
            _viewModelFactory = viewModelFactory;
            _logger = logger;
        }

        public void NavigateTo(string viewName, NavigationParameters parameters = null)
        {
            if (_currentViewModel != null)
            {
                if (_backStack.Count >= MaxStackSize)
                {
                    _backStack.Pop();
                }
                _backStack.Push(_currentViewModel);
            }

            _logger.LogInformation($"Navigating to {viewName}");
            var viewModel = _viewModelFactory.Create(viewName, parameters);

            if (viewModel is IInitializeWithParameters parameterizedViewModel && parameters != null)
            {
                parameterizedViewModel.InitializeWithParameters(parameters);
            }

            _currentViewModel = viewModel;
            _currentViewName = FormatViewName(viewName);
            _forwardStack.Clear();
            OnNavigationStateChanged();
        }

        public void GoBackward()
        {
            if (CanGoBackward())
            {
                _forwardStack.Push(_currentViewModel);
                _currentViewModel = _backStack.Pop();
                _currentViewName = FormatViewName(_currentViewModel.GetType().Name);
                OnNavigationStateChanged();
            }
        }

        public void GoForward()
        {
            if (CanGoForward())
            {
                _backStack.Push(_currentViewModel);
                _currentViewModel = _forwardStack.Pop();
                _currentViewName = FormatViewName(_currentViewModel.GetType().Name);
                OnNavigationStateChanged();
            }
        }

        public bool CanGoBackward() => _backStack.Count > 0;

        public bool CanGoForward() => _forwardStack.Count > 0;

        public BindableBase GetCurrentViewModel() => _currentViewModel;
        public string GetCurrentViewName() => _currentViewName;

        private void OnNavigationStateChanged()
        {
            NavigationStateChanged?.Invoke();
        }
        
        private string FormatViewName(string viewName)
        {
            if (string.IsNullOrEmpty(viewName))
                return string.Empty;
            
            var cleanedName = viewName.Replace("ViewModel", "").Replace("View Model", "");
            
            var readableName = ReadableRegex().Replace(cleanedName, " $1").Trim();

            return readableName;
        }

        [System.Text.RegularExpressions.GeneratedRegex("([A-Z])")]
        private static partial System.Text.RegularExpressions.Regex ReadableRegex();
    }
}
