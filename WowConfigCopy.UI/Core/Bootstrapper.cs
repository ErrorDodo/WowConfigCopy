using System.Windows;
using Prism.Ioc;
using Prism.Unity;
using WowConfigCopy.Common.Interfaces;
using WowConfigCopy.Common.Services;
using WowConfigCopy.UI.Interfaces;
using WowConfigCopy.UI.Services;
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
        containerRegistry.RegisterSingleton<INavigationService, NavigationService>();
        
        RegisterLogging.Register(containerRegistry);
    }
}