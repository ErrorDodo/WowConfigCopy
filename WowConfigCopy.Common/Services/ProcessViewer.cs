using System.Diagnostics;
using Microsoft.Extensions.Logging;
using WowConfigCopy.Common.Interfaces;

namespace WowConfigCopy.Common.Services;

public class ProcessViewer : IProcessViewer
{
    private readonly ILogger<ProcessViewer> _logger;

    public ProcessViewer(ILogger<ProcessViewer> logger)
    {
        _logger = logger;
    }
    
    public bool IsProcessRunning(string processName)
    {
        var processes = GetProcessesByName(processName);
        if (processes.Length == 0)
        {
            _logger.LogInformation($"Process {processName} is not running.");
            return false;
        }

        _logger.LogInformation($"Process {processName} is running.");
        return true;
    }
    
    private static Process[] GetProcessesByName(string processName)
    {
        return Process.GetProcessesByName(processName);
    }
}