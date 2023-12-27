using Microsoft.Extensions.Logging;
using Prism.Mvvm;
using Prism.Navigation;
using WowConfigCopy.UI.Interfaces;

namespace WowConfigCopy.UI.ViewModels;

public class CopyFilesViewModel : BindableBase, IInitializeWithParameters
{
    private readonly ILogger<CopyFilesViewModel> _logger;

    private string _accountName;
    private string _sourceConfigLocation;
    
    public string AccountName
    {
        get => _accountName;
        set => SetProperty(ref _accountName, value);
    }

    public CopyFilesViewModel(ILogger<CopyFilesViewModel> logger)
    {
        _logger = logger;
    }

    public void InitializeWithParameters(NavigationParameters parameters)
    {
        _logger.LogInformation("Initializing CopyFilesViewModel with parameters");
        
        if (parameters.TryGetValue("accountName", out string accountName))
        {
            _logger.LogInformation($"Account name: {accountName}");
            AccountName = accountName;
        }
        
        if (parameters.TryGetValue("configLocation", out string configLocation))
        {
            _logger.LogInformation($"Source path: {configLocation}");
            _sourceConfigLocation = configLocation;
        }
    }
}