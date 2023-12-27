using Microsoft.Extensions.Logging;
using WowConfigCopy.Common.Helpers;
using WowConfigCopy.Common.Interfaces;

namespace WowConfigCopy.Common.Services;

public class ConfigCopy : IConfigCopy
{
    private readonly ILogger<ConfigCopy> _logger;
    private readonly FileHelpers _fileHelpers = new();
    
    public event EventHandler<int> ProgressChanged;
    public event EventHandler<string> FileCopying;

    public ConfigCopy(ILogger<ConfigCopy> logger)
    {
        _logger = logger;
    }
    
    public void CopyConfigFiles(string sourceConfigLocation, string destinationConfigLocation, bool firstRun = true, bool copySavedVariables = false)
    {
        _logger.LogInformation($"Copying config files from {sourceConfigLocation} to {destinationConfigLocation}");
        var sourceFiles = _fileHelpers.GetFilesSafe(sourceConfigLocation);
        
        if (firstRun)
        {
            _logger.LogInformation("First run detected, only copying config-cache.wtf and AddOns.txt");
            sourceFiles = sourceFiles.Where(x => x.Contains("config-cache.wtf") || x.Contains("AddOns.txt"));
        }
        else
        {
            _logger.LogInformation("Not first run, copying all files");
        }
        
        if (copySavedVariables)
        {
            _logger.LogInformation("Copying SavedVariables directory");
            var savedVariablesDirectory = Path.Combine(sourceConfigLocation, "SavedVariables");
            sourceFiles = sourceFiles.Concat(_fileHelpers.GetFilesSafe(savedVariablesDirectory));
        }

        var fileIndex = 0;
        var enumerable = sourceFiles as string[] ?? sourceFiles.ToArray();
        foreach (var file in enumerable)
        {
            var fileName = Path.GetFileName(file);
            
            // If it's the first run, we want to only copy config-cache.wtf and AddOns.txt
            // We also copy the directory SavedVariables but have this as a boolean as well 
            
            FileCopying.Invoke(this, fileName);
            var destinationFile = Path.Combine(destinationConfigLocation, fileName);
            
            var newDestinationFile = destinationFile;
            while (File.Exists(newDestinationFile))
            {
                newDestinationFile = Path.Combine(destinationConfigLocation, $"{Path.GetFileNameWithoutExtension(fileName)}_{fileIndex}{Path.GetExtension(fileName)}");
                fileIndex++;
            }

            _logger.LogInformation($"Creating new file: {newDestinationFile}");
            // File.Create(newDestinationFile).Dispose(); // Uncomment to actually create the file
            
            ProgressChanged?.Invoke(this, (fileIndex + 1) * 100 / enumerable.Length);

            // Use File.Create to create a new file for debugging purposes
            // Comment this out and uncomment the File.Copy line in production
            // File.Create(newDestinationFile).Dispose(); // Uncomment this line when debugging
            // File.Copy(file, newDestinationFile, true); // Use this line in production
        }
    }
}