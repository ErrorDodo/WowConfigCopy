using System.Collections.ObjectModel;
using WowConfigCopy.Common.Models;

namespace WowConfigCopy.UI.Models;

public class AccountsModel
{
    public ObservableCollection<AccountModel> Accounts { get; set; } = new();
}