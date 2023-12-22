using Microsoft.Extensions.Logging;
using Prism.Navigation.Regions;
using WowConfigCopy.UI.Interfaces;

namespace WowConfigCopy.UI.Services;

public class NavigationService : INavigationService
{
    private readonly IRegionManager _regionManager;
    private IRegionNavigationJournal _navigationJournal;
    private readonly ILogger<NavigationService> _logger;

    public NavigationService(IRegionManager regionManager, ILogger<NavigationService> logger)
    {
        _regionManager = regionManager;
        _logger = logger;
    }

    public void GoBack()
    {
        _logger.LogInformation("Navigating back");
        _navigationJournal?.GoBack();
    }

    public void GoForward()
    {
        _logger.LogInformation("Navigating forward");
        _navigationJournal?.GoForward();
    }

    public bool CanGoBack => _navigationJournal?.CanGoBack == true;
    public bool CanGoForward => _navigationJournal?.CanGoForward == true;

    public void UpdateJournal(IRegionNavigationJournal navigationJournal)
    {
        _navigationJournal = navigationJournal;
    }
}