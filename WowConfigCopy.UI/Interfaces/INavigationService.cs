namespace WowConfigCopy.UI.Interfaces;

public interface INavigationService
{
    void GoBack();
    void GoForward();
    bool CanGoBack { get; }
    bool CanGoForward { get; }
}