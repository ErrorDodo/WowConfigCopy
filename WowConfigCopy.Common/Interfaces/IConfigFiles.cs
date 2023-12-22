using WowConfigCopy.Common.Models;

namespace WowConfigCopy.Common.Interfaces;

public interface IConfigFiles
{
    List<AccountModel> ReadConfigFiles(string wowVersion);
}