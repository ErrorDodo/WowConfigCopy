using System.Windows;
using Microsoft.Extensions.Logging;
using Prism.Events;
using Prism.Mvvm;
using WowConfigCopy.UI.Events;

namespace WowConfigCopy.UI.ViewModels;

public class NotificationsViewModel : BindableBase
{
    private readonly ILogger<NotificationsViewModel> _logger;
    private double _left;
    public double Left
    {
        get => _left;
        set => SetProperty(ref _left, value);
    }

    private double _top;
    public double Top
    {
        get => _top;
        set => SetProperty(ref _top, value);
    }
    
    private string _message;
    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    public NotificationsViewModel(ILogger<NotificationsViewModel> logger, IEventAggregator eventAggregator)
    {
        _logger = logger;
        eventAggregator.GetEvent<ShowNotificationEvent>().Subscribe(MessageReceived);
    }
    
    private void MessageReceived(string message)
    {
        Message = message;
        PositionNotification();
    }
    
    private void PositionNotification()
    {
        var screenWidth = SystemParameters.WorkArea.Width;
        var screenHeight = SystemParameters.WorkArea.Height;
        var notificationWidth = 300;
        var notificationHeight = 100;
        
        _logger.LogInformation($"Current Window Location: {Left}, {Top}");

        Left = screenWidth - notificationWidth;
        Top = screenHeight - notificationHeight;
        
        _logger.LogInformation($"Moved Window location to: {Left}, {Top}");
    }
}