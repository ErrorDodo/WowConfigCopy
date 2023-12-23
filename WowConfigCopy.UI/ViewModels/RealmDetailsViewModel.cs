using Microsoft.Extensions.Logging;
using Prism.Mvvm;
using Prism.Navigation;
using WowConfigCopy.UI.Interfaces;

namespace WowConfigCopy.UI.ViewModels;

public class RealmDetailsViewModel : BindableBase, IInitializeWithParameters
{
    private ILogger<RealmDetailsViewModel> _logger;
    
    private string _regionName = string.Empty;
    
    public string RegionName
    {
        get => _regionName;
        set => SetProperty(ref _regionName, value);
    }
    
    public RealmDetailsViewModel(ILogger<RealmDetailsViewModel> logger)
    {
        _logger = logger;
    }
    public void InitializeWithParameters(NavigationParameters parameters)
    {
        if (parameters.TryGetValue("region", out string region))
        {
            RegionName = region;
        }
    }
}