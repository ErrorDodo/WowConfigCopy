using Prism.Ioc;
using WowConfigCopy.Api.Core;
using WowConfigCopy.Api.Interfaces;
using WowConfigCopy.Api.Services;
using WowConfigCopy.Common.Interfaces;
using WowConfigCopy.Common.Services;
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
    
    public static void RegisterApiServices(this IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<BlizzardApiClientFactory>();
        containerRegistry.RegisterSingleton<IGenericApiService, GenericApiService>();
    }
    
    public static void RegisterCommonServices(this IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<IConfigFiles, ConfigFiles>();
        containerRegistry.RegisterSingleton<IRegistryHelper, RegistryHelper>();
    }
}