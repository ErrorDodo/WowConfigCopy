using Prism.Ioc;
using WowConfigCopy.UI.Core;
using WowConfigCopy.UI.Interfaces;
using WowConfigCopy.UI.Services;
using WowConfigCopy.UI.ViewModels;

namespace WowConfigCopy.UI.Extensions;

public static class ContainerExtensions
{
    public static void RegisterUiServices(this IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<IAccountConfigService, AccountConfigService>();
        containerRegistry.RegisterSingleton<IViewModelFactory, ViewModelFactory>();
        containerRegistry.RegisterSingleton<INavigationService, NavigationService>();
        containerRegistry.RegisterSingleton<IWindowService, WindowService>();


    }
    
    public static void RegisterUiViews(this IContainerRegistry containerRegistry)
    {
        containerRegistry.Register<RegionsViewModel>();
        containerRegistry.Register<SettingsViewModel>();
        containerRegistry.Register<RegionDetailsViewModel>();
    }
}