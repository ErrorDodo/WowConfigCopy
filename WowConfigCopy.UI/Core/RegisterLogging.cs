using Microsoft.Extensions.Logging;
using Prism.Ioc;

namespace WowConfigCopy.UI.Core
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

            containerRegistry.RegisterInstance<ILoggerFactory>(loggerFactory);
            containerRegistry.Register(typeof(ILogger<>), typeof(Logger<>));
        }
    }
}