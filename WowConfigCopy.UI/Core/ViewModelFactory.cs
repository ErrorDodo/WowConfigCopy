using System;
using Prism.Ioc;
using Prism.Mvvm;
using WowConfigCopy.UI.Interfaces;
using WowConfigCopy.UI.ViewModels;

namespace WowConfigCopy.UI.Core;

public class ViewModelFactory : IViewModelFactory
{
    private readonly IContainerProvider _containerProvider;

    public ViewModelFactory(IContainerProvider containerProvider)
    {
        _containerProvider = containerProvider;
    }

    public BindableBase Create(string viewModelName)
    {
        return viewModelName switch
        {
            "Accounts" => _containerProvider.Resolve<AccountsViewModel>(),
            "Settings" => _containerProvider.Resolve<SettingsViewModel>(),
            _ => throw new ArgumentException($"The view model {viewModelName} is not mapped")
        };
    }
}