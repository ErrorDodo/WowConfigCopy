using System.Threading.Tasks;
using WowConfigCopy.UI.Interfaces;
using WowConfigCopy.UI.ViewModels;
using WowConfigCopy.UI.Views;

namespace WowConfigCopy.UI.Services;

public class NotificationService : INotificationService
{
    public void ShowNotification(string message)
    {
        var notificationWindow = new Notifications()
        {
            DataContext = new NotificationViewModel(message)
        };
        notificationWindow.Show();
        
        Task.Delay(3000).ContinueWith(_ => notificationWindow.Dispatcher.Invoke(() => notificationWindow.Close()));
    }
}
