using Prism.Ioc;
using WowConfigCopy.Common.Interfaces;
using WowConfigCopy.Common.Services;

namespace WowConfigCopy.Common.Extensions;

public static class ContainerExtensions
{
    public static void RegisterCommonServices(this IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<IConfigFiles, ConfigFiles>();
        containerRegistry.RegisterSingleton<IRegistryHelper, RegistryHelper>();
    }
}