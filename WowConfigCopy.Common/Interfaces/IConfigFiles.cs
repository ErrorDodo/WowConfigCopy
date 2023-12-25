using System.Collections.ObjectModel;
using WowConfigCopy.Common.Models;

namespace WowConfigCopy.Common.Interfaces;

public interface IConfigFiles
{
    Task<ObservableCollection<AccountModel>> ReadConfigFilesAsync(string wowVersion);
    Task<ObservableCollection<RealmAccountsModel>> GetRealmsAccounts();
    Task<ObservableCollection<ConfigFileModel>> GetConfigFiles(string configPath, bool getAccountConfigFiles = false);
}