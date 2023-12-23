using System.Collections.ObjectModel;
using WowConfigCopy.Common.Models;

namespace WowConfigCopy.Common.Interfaces;

public interface IConfigFiles
{
    ObservableCollection<AccountModel> ReadConfigFiles(string wowVersion);
}