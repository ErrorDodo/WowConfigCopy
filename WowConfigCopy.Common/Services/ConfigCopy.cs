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
        var sourceFiles = GetSourceFiles(sourceConfigLocation, firstRun, copySavedVariables);
        CopyFiles(sourceFiles, sourceConfigLocation, destinationConfigLocation);
    }
    
    private IEnumerable<string> GetSourceFiles(string sourceConfigLocation, bool firstRun, bool copySavedVariables)
    {
        var sourceFiles = _fileHelpers.GetFilesSafe(sourceConfigLocation);

        if (firstRun)
        {
            sourceFiles = FilterInitialRunFiles(sourceFiles);
        }

        if (copySavedVariables)
        {
            sourceFiles = IncludeSavedVariables(sourceConfigLocation, sourceFiles);
        }

        return sourceFiles;
    }

    private IEnumerable<string> FilterInitialRunFiles(IEnumerable<string> files)
    {
        _logger.LogInformation("First run detected, only copying config-cache.wtf and AddOns.txt");
        return files.Where(x => x.EndsWith("config-cache.wtf") || x.EndsWith("AddOns.txt"));
    }

    private IEnumerable<string> IncludeSavedVariables(string sourceConfigLocation, IEnumerable<string> files)
    {
        _logger.LogInformation("Copying SavedVariables directory");
        var savedVariablesDirectory = Path.Combine(sourceConfigLocation, "SavedVariables");
        return files.Concat(_fileHelpers.GetFilesSafe(savedVariablesDirectory));
    }
    
    //TODO: Copy keybinds from the root directory (One directory outside of the Realm, make this optional)
    private void CopyFiles(IEnumerable<string> sourceFiles, string sourceConfigLocation, string destinationConfigLocation)
    {
        var fileIndex = 0;
        var enumerable = sourceFiles as string[] ?? sourceFiles.ToArray();
        foreach (var file in enumerable)
        {
            var fileName = Path.GetFileName(file);
            var sourceDirectory = Path.GetDirectoryName(file);
        
            // Compute the relative directory path (This is so SavedVariables is copied to the correct location)
            var relativeDirectory = sourceDirectory.Replace(sourceConfigLocation, "").TrimStart(Path.DirectorySeparatorChar);
            var destinationDirectory = Path.Combine(destinationConfigLocation, relativeDirectory);
            
            
            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            var destinationFilePath = Path.Combine(destinationDirectory, fileName);
        
            FileCopying?.Invoke(this, fileName);

            // Actual file copy logic
            _logger.LogInformation($"Copying file: {fileName} to {destinationFilePath}");
            File.Copy(file, destinationFilePath, true);

            ProgressChanged?.Invoke(this, CalculateProgressPercentage(fileIndex, enumerable.Length));
            fileIndex++;
        }
    }

    
    private string GenerateUniqueDestinationPath(string destinationConfigLocation, string fileName, ref int fileIndex)
    {
        var newDestinationFile = Path.Combine(destinationConfigLocation, fileName);
        while (File.Exists(newDestinationFile))
        {
            newDestinationFile = Path.Combine(destinationConfigLocation, $"{Path.GetFileNameWithoutExtension(fileName)}_{fileIndex}{Path.GetExtension(fileName)}");
            fileIndex++;
        }
        return newDestinationFile;
    }

    private int CalculateProgressPercentage(int fileIndex, int totalFiles)
    {
        return (fileIndex + 1) * 100 / totalFiles;
    }
}
