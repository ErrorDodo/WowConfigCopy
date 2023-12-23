using System.Collections.ObjectModel;
using WowConfigCopy.UI.Models;

namespace WowConfigCopy.UI.Interfaces;

public interface IAccountConfigService
{
    ObservableCollection<AccountsModel> ReadConfig();
}