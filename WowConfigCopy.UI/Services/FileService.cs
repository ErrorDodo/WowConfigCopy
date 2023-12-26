using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using WowConfigCopy.UI.Interfaces;

namespace WowConfigCopy.UI.Services;

public class FileService : IFileService
{
    private readonly ILogger<FileService> _logger;
    
    public FileService(ILogger<FileService> logger)
    {
        _logger = logger;
    }
    
    public void EditFile(string filePath)
    {
        _logger.LogInformation($"Edit file command called for file: {filePath}");
    }
    
    public void ViewFile(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            _logger.LogWarning("ViewFile called with a null or empty filePath.");
            return;
        }

        try
        {
            _logger.LogInformation($"View file command called for file: {filePath}");
            var processStartInfo = new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            };
            Process.Start(processStartInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while opening file: {filePath}");
        }
    }

}