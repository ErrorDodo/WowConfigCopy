using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using WowConfigCopy.Common.Interfaces;

namespace WowConfigCopy.Common.Services;

public class RegistryHelper : IRegistryHelper
{
    private readonly ILogger<RegistryHelper> _logger;

    public RegistryHelper(ILogger<RegistryHelper> logger)
    {
        _logger = logger;
    }
    
    public string? GetWowInstallPath()
    {
        // Note: The registry path depends on the most recently loaded WoW classic version
        _logger.LogInformation("Attempting to retrieve WoW Install Path from the Registry...");

        var keyPath = Environment.Is64BitOperatingSystem 
            ? @"SOFTWARE\WOW6432Node\Blizzard Entertainment\World of Warcraft" 
            : @"SOFTWARE\Blizzard Entertainment\World of Warcraft";

        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(keyPath);
            if (key?.GetValue("InstallPath") is string installPath)
            {
                _logger.LogInformation($"WoW Install Path found: {installPath}");
                return installPath;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving WoW install path: {ex.Message}");
        }

        _logger.LogWarning("WoW Install Path not found in Registry.");
        return null;
    }
}