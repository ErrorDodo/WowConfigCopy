using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WowConfigCopy.Common.Models;

namespace WowConfigCopy.UI.Interfaces;

public interface IAccountConfigService
{
    Task<ObservableCollection<AccountModel>> ReadConfigAsync();
    Task<ObservableCollection<RealmAccountsModel>> GetRealmsAccountsAsync();
}