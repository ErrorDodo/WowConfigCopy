using System;
using Prism.Mvvm;

namespace WowConfigCopy.UI.Interfaces;

public interface INavigationService
{
    void NavigateTo(string viewName);
    void GoBackward();
    void GoForward();
    bool CanGoBackward();
    bool CanGoForward();
    BindableBase GetCurrentViewModel();
    string GetCurrentViewName();
    event Action NavigationStateChanged;
}
