namespace WowConfigCopy.Common.Models;

public class RealmModel
{
    public string RealmName { get; set; }
    public string RealmRegion { get; set; }
    public List<RealmAccountsModel> Accounts { get; set; } = new();
}