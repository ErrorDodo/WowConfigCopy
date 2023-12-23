using System.Windows;
using Prism.Ioc;
using Prism.Unity;
using Unity;
using WowConfigCopy.Common.Interfaces;
using WowConfigCopy.Common.Services;
using WowConfigCopy.UI.Interfaces;
using WowConfigCopy.UI.Services;
using WowConfigCopy.UI.ViewModels;
using WowConfigCopy.UI.Views;

namespace WowConfigCopy.UI.Core;

public class Bootstrapper : PrismBootstrapper
{
    protected override DependencyObject CreateShell()
    {
        return Container.Resolve<Shell>();
    }

    protected override void InitializeShell(DependencyObject shell)
    {
        Application.Current.MainWindow = (Window) shell;
        Application.Current.MainWindow.Show();
    }
    
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<IConfigFiles, ConfigFiles>();
        containerRegistry.RegisterSingleton<IRegistryHelper, RegistryHelper>();
        containerRegistry.RegisterSingleton<IAccountConfigService, AccountConfigService>();
        containerRegistry.RegisterSingleton<IViewModelFactory, ViewModelFactory>();
        containerRegistry.RegisterSingleton<INavigationService, NavigationService>();
        containerRegistry.RegisterSingleton<IWindowService, WindowService>();

        containerRegistry.Register<FoldersViewModel>();
        containerRegistry.Register<SettingsViewModel>();
        
        // containerRegistry.RegisterForNavigation<ViewConfig, ViewConfigViewModel>();
        // containerRegistry.RegisterForNavigation<Settings, SettingsViewModel>();

        
        RegisterLogging.Register(containerRegistry);
    }
}