using System.Collections.ObjectModel;

namespace WowConfigCopy.Common.Models;

public class AccountModel
{
    public string FolderName { get; set; }
    public ObservableCollection<RealmModel> Realms { get; set; } = new();
}