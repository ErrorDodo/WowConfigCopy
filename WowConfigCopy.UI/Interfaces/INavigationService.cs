using System;
using Prism.Mvvm;
using Prism.Navigation;

namespace WowConfigCopy.UI.Interfaces;

public interface INavigationService
{
    void NavigateTo(string viewName, NavigationParameters parameters = null);
    void GoBackward();
    void GoForward();
    bool CanGoBackward();
    bool CanGoForward();
    BindableBase GetCurrentViewModel();
    string GetCurrentViewName();
    event Action NavigationStateChanged;
}
