using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using WowConfigCopy.Common.Models;
using WowConfigCopy.UI.Interfaces;

namespace WowConfigCopy.UI.ViewModels;

public class AccountDetailsViewModel : BindableBase, IInitializeWithParameters
{
    private readonly ILogger<AccountDetailsViewModel> _logger;
    private readonly IAccountConfigService _accountConfigService;
    private readonly IFileService _fileService;
    
    private string _configLocation = string.Empty;
    private string _accountName = string.Empty;
    
    public string AccountName
    {
        get => _accountName;
        set => SetProperty(ref _accountName, value);
    }
    
    private ObservableCollection<RealmAccountsModel> _realmAccounts = new();
    
    public ObservableCollection<RealmAccountsModel> RealmAccounts
    {
        get => _realmAccounts;
        set => SetProperty(ref _realmAccounts, value);
    }
    
    private ObservableCollection<ConfigFileModel> _configFileModel;
    
    public ObservableCollection<ConfigFileModel> ConfigFileModel
    {
        get => _configFileModel;
        set => SetProperty(ref _configFileModel, value);
    }
    
    public DelegateCommand<ConfigFileModel> EditFileCommand { get; set; }
    public DelegateCommand<ConfigFileModel> ViewFileCommand { get; set; }

    public AccountDetailsViewModel(ILogger<AccountDetailsViewModel> logger, IAccountConfigService accountConfigService, IFileService fileService)
    {
        _logger = logger;
        _accountConfigService = accountConfigService;
        _fileService = fileService;

        EditFileCommand = new DelegateCommand<ConfigFileModel>(EditFile);
        ViewFileCommand = new DelegateCommand<ConfigFileModel>(ViewFile);
    }
    
    private void EditFile(ConfigFileModel model)
    {
        _logger.LogInformation($"Edit file command called for file: {model.Name}");
    }
    
    private void ViewFile(ConfigFileModel model)
    {
        _fileService.ViewFile(model.Path);
    }

    public void InitializeWithParameters(NavigationParameters parameters)
    {
        _logger.LogInformation("Initializing AccountDetailsViewModel with parameters");
        
        if (parameters.TryGetValue("accountName", out string accountName))
        {
            _logger.LogInformation($"Received account parameter: {accountName}");
            AccountName = $"{accountName} - Config Files";
        }
        else
        {
            _logger.LogWarning("Account parameter not found");
        }
        
        if (parameters.TryGetValue("configLocation", out string configLocation))
        {
            _logger.LogInformation($"Received config location parameter: {configLocation}");
            _configLocation = configLocation;
        }
        else
        {
            _logger.LogWarning("Config location parameter not found");
        }
        
        LoadAllAccounts().ConfigureAwait(true);
        GetAllConfigFiles().ConfigureAwait(true);
    }
    
    // We get all accounts again so we can copy the config to all accounts or vice versa
    private async Task LoadAllAccounts()
    {
        _logger.LogInformation("Loading realm accounts");
        RealmAccounts = await _accountConfigService.GetRealmsAccountsAsync();
    }
    
    private async Task GetAllConfigFiles()
    {
        _logger.LogInformation("Getting all config files");
        ConfigFileModel =  await _accountConfigService.GetConfigFilesAsync(_configLocation);
    }
}