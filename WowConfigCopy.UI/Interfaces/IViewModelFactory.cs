using Prism.Mvvm;

namespace WowConfigCopy.UI.Interfaces;

public interface IViewModelFactory
{
    BindableBase Create(string viewModelName);
}