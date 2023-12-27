using Microsoft.Extensions.Logging;
using WowConfigCopy.Common.Interfaces;

namespace WowConfigCopy.Common.Services;

public class SettingsService : ISettingsService
{
    private readonly ILogger<SettingsService> _logger;
    private readonly string _settingsFolder;

    public SettingsService(ILogger<SettingsService> logger)
    {
        _logger = logger;
        _settingsFolder = SetupSettingsFolder();
    }
    
    private string SetupSettingsFolder()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var settingsFolder = Path.Combine(appData, "WowConfigCopySettings");
        if (Directory.Exists(settingsFolder)) return settingsFolder;
        Directory.CreateDirectory(settingsFolder);
        _logger.LogInformation($"Created settings folder at {settingsFolder}");
        return settingsFolder;
    }
    
    public string GetSettingsFolder()
    {
        return _settingsFolder;
    }
}
