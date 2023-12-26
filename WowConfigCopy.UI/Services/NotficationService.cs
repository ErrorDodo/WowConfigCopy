using System.Windows.Forms;
using WowConfigCopy.UI.Interfaces;

namespace WowConfigCopy.UI.Services;

public class NotficationService : INotificationService
{
    public void ShowMessage(string message)
    {
        MessageBox.Show(message);
    }
}