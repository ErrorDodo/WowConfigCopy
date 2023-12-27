using Prism.Events;
using WowConfigCopy.UI.Events;
using WowConfigCopy.UI.Interfaces;

namespace WowConfigCopy.UI.Services;

public class NotificationService : INotificationService
{
    private readonly IEventAggregator _eventAggregator;

    public NotificationService(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
    }

    public void ShowNotification(string message)
    {
        _eventAggregator.GetEvent<ShowNotificationEvent>().Publish(message);
    }
}
