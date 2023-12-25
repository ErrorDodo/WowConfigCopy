using Microsoft.Extensions.Logging;
using Prism.Ioc;

namespace WowConfigCopy.UI.Extensions
{
    public static class RegisterLogging
    {
        public static void Register(IContainerRegistry containerRegistry)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });

            containerRegistry.RegisterInstance(loggerFactory);
            containerRegistry.Register(typeof(ILogger<>), typeof(Logger<>));
        }
    }
}