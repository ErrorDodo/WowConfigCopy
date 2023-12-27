using Prism.Mvvm;

namespace WowConfigCopy.UI.ViewModels;

public class NotificationViewModel : BindableBase
{
    private string _message;
    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    public NotificationViewModel(string message)
    {
        Message = message;
    }
}