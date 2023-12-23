using Prism.Mvvm;
using Prism.Navigation;

namespace WowConfigCopy.UI.Interfaces;

public interface IViewModelFactory
{
    BindableBase Create(string viewModelName, NavigationParameters parameters = null);
}