using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
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

    public async Task<string> ViewFileContents(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            _logger.LogWarning("ViewFileContents called with a null or empty filePath.");
            return string.Empty;
        }

        try
        {
            _logger.LogInformation($"Reading file contents for {filePath}");
            return await File.ReadAllTextAsync(filePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while reading file contents: {filePath}");
            return string.Empty;
        }
    }
    
    public async Task SaveFileContents(string filePath, string content)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            _logger.LogWarning("SaveFileContents called with a null or empty filePath.");
            return;
        }

        try
        {
            _logger.LogInformation($"Attempting to write to file: {filePath}");
            await File.WriteAllTextAsync(filePath, content);
            _logger.LogInformation($"Successfully wrote to file: {filePath}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while writing to file: {filePath}");
        }
    }
}