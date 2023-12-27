using System;
using Prism.Mvvm;
using Prism.Navigation;
using WowConfigCopy.Common.Models;
using WowConfigCopy.UI.Dto;

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

    void NavigateToRealmDetails(RegionDetails regionDetails);
    void NavigateToAccountDetails(RealmAccountsModel model);
    void NavigateToEditFile(ConfigFileModel fileDetails);
    void NavigateToCopyFiles(string accountName, string configLocation);
}
