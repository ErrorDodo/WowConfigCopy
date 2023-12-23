using System.Collections.ObjectModel;

namespace WowConfigCopy.Common.Models;

public class RealmModel
{
    public string RealmName { get; set; }
    public string RealmRegion { get; set; }
    public ObservableCollection<RealmAccountsModel> Accounts { get; set; } = new();
}