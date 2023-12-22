namespace WowConfigCopy.Common.Models;

public class AccountModel
{
    public string FolderName { get; set; }
    public List<RealmModel> Realms { get; set; } = new();
}