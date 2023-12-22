using Prism.Mvvm;

namespace WowConfigCopy.UI.ViewModels;

public class ShellViewModel : BindableBase
{
    private string _title = "Wow Config Copy";

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }
}