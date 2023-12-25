using System.Windows;
using Prism.Ioc;
using Prism.Unity;
using Unity;
using WowConfigCopy.Api.Extensions;
using WowConfigCopy.Common.Extensions;
using WowConfigCopy.Common.Interfaces;
using WowConfigCopy.Common.Services;
using WowConfigCopy.UI.Extensions;
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
        RegisterLogging.Register(containerRegistry);
        containerRegistry.RegisterUiViews();
        containerRegistry.RegisterUiServices();
        containerRegistry.RegisterApiServices();
        containerRegistry.RegisterCommonServices();
    }
}