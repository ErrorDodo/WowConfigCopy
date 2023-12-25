using System.Threading.Tasks;
using Prism.Navigation;

namespace WowConfigCopy.UI.Interfaces;

public interface IInitializeWithParameters
{
    void InitializeWithParameters(NavigationParameters parameters);
}