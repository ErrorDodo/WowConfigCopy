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
    
    // TODO: Do a workaround, blizzard is annoying for this one. Whichever version of classic you loaded up recently the registry will point to that one. (SoD, Wotlk)
    // Maybe Tell the user this in the UI so they know about this issue
    public string? GetWowInstallPath()
    {
        _logger.LogInformation("Getting WoW Install Path From Registry...");
        
        const string keyPath32Bit = @"SOFTWARE\Blizzard Entertainment\World of Warcraft";
        const string keyPath64Bit = @"SOFTWARE\WOW6432Node\Blizzard Entertainment\World of Warcraft";
        
        var keyPath = Is64BitOperatingSystem() ? keyPath64Bit : keyPath32Bit;

        try
        {
            // We only check for windows
            using var key = Registry.LocalMachine.OpenSubKey(keyPath);
            var installPath = key?.GetValue("InstallPath");
            if (installPath != null)
            {
                _logger.LogInformation($"WoW Install Path: {installPath}");
                return installPath.ToString();
            }
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Error retrieving WoW install path: {ex.Message}");
        }

        _logger.LogInformation("WoW Install Path not found in Registry.");
        return null;
    }

    
    private static bool Is64BitOperatingSystem()
    {
        return Environment.Is64BitOperatingSystem;
    }
}