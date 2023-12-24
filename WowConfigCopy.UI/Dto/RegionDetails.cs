using System.Collections.ObjectModel;
using WowConfigCopy.Common.Models;

namespace WowConfigCopy.UI.Dto;

public class RegionDetails
{
    public string RealmName { get; set; } = string.Empty;
    public ObservableCollection<RealmAccountsModel> Accounts { get; set; } = new();
}