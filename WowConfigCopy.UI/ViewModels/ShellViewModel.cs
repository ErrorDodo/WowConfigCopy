using Prism.Commands;
using Prism.Mvvm;
using WowConfigCopy.UI.Interfaces;
using WowConfigCopy.UI.Services;

namespace WowConfigCopy.UI.ViewModels;

public class ShellViewModel : BindableBase
{
    private string _title = "Wow Config Copy";
    private readonly IAccountConfigService _accountConfigService;

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }
    
    public DelegateCommand CheckConfigsCommand { get; private set; }


    public ShellViewModel(IAccountConfigService accountConfigService)
    {
        _accountConfigService = accountConfigService;
        CheckConfigsCommand = new DelegateCommand(CheckConfigs);
    }

    private void CheckConfigs()
    {
        _accountConfigService.ReadConfig();
    }
}