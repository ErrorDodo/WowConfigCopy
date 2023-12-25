using System;
using System.Linq;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Navigation;
using WowConfigCopy.UI.Interfaces;
using System.Reflection;
using WowConfigCopy.UI.ViewModels;

namespace WowConfigCopy.UI.Core;

public class ViewModelFactory : IViewModelFactory
{
    private readonly IContainerProvider _containerProvider;

    public ViewModelFactory(IContainerProvider containerProvider)
    {
        _containerProvider = containerProvider;
    }

    public BindableBase Create(string viewModelName, NavigationParameters parameters = null)
    {
        var fullViewModelName = $"{viewModelName}ViewModel";
        
        var viewModelType = Assembly.GetExecutingAssembly()
            .GetTypes()
            .FirstOrDefault(t => t is {IsClass: true, Namespace: "WowConfigCopy.UI.ViewModels"} && t.Name == fullViewModelName);

        if (viewModelType == null)
        {
            throw new ArgumentException($"The view model {viewModelName} is not mapped");
        }
        
        var viewModel = _containerProvider.Resolve(viewModelType) as BindableBase;

        switch (viewModel)
        {
            case IInitializeWithParameters parameterizedViewModel when parameters != null:
                parameterizedViewModel.InitializeWithParameters(parameters);
                break;
            case null:
                throw new ArgumentException($"Failed to resolve the view model {viewModelName}");
        }

        return viewModel;
    }
}