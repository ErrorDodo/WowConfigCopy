using Prism.Ioc;
using WowConfigCopy.Api.Core;
using WowConfigCopy.Api.Interfaces;
using WowConfigCopy.Api.Services;

namespace WowConfigCopy.Api.Extensions;

public static class ContainerExtensions
{
    public static void RegisterApiServices(this IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<BlizzardApiClientFactory>();
        containerRegistry.RegisterSingleton<IGenericApiService, GenericApiService>();
    }
}