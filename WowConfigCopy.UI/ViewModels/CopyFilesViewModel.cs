using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using WowConfigCopy.Common.Helpers;
using WowConfigCopy.Common.Interfaces;
using WowConfigCopy.Common.Models;
using WowConfigCopy.UI.Interfaces;

namespace WowConfigCopy.UI.ViewModels;

public class CopyFilesViewModel : BindableBase, IInitializeWithParameters, IDestructible
{
    private readonly ILogger<CopyFilesViewModel> _logger;
    private readonly IConfigFiles _configFiles;
    private readonly IConfigCopy _configCopy;
    private readonly IProcessViewer _processViewer;

    private CancellationTokenSource _cts = new CancellationTokenSource();
    private string _accountName;
    private string _sourceConfigLocation;
    private Visibility _copyButtonVisibility = Visibility.Collapsed;
    private Visibility _progressBarVisibility = Visibility.Collapsed;
    private int _progressBarValue;
    private string _currentLog;
    private bool _isOperationInProgress;
    
    public string CurrentLog
    {
        get => _currentLog;
        set => SetProperty(ref _currentLog, value);
    }

    public bool IsOperationInProgress
    {
        get => _isOperationInProgress;
        set => SetProperty(ref _isOperationInProgress, value);
    }
    
    public int ProgressBarValue
    {
        get => _progressBarValue;
        set => SetProperty(ref _progressBarValue, value);
    }
    
    public Visibility ProgressBarVisibility
    {
        get => _progressBarVisibility;
        set => SetProperty(ref _progressBarVisibility, value);
    }
    
    public Visibility CopyButtonVisibility
    {
        get => _copyButtonVisibility;
        set => SetProperty(ref _copyButtonVisibility, value);
    }
    
    public string AccountName
    {
        get => _accountName;
        set => SetProperty(ref _accountName, value);
    }
    
    private ObservableCollection<RealmAccountsModel> _accounts;
    public ObservableCollection<RealmAccountsModel> Accounts
    {
        get => _accounts;
        set => SetProperty(ref _accounts, value);
    }

    private RealmAccountsModel _selectedAccount;
    public RealmAccountsModel SelectedAccount
    {
        get => _selectedAccount;
        set
        {
            if (SetProperty(ref _selectedAccount, value))
            {
                LogSelectedAccountInfo();
                CopyButtonVisibility = value != null ? Visibility.Visible : Visibility.Collapsed;
                ProgressBarVisibility = Visibility.Collapsed;
            }
        }
    }
    
    private void LogSelectedAccountInfo()
    {
        if (_selectedAccount != null)
        {
            _logger.LogInformation($"Selected account: {_selectedAccount.AccountName}");
            _logger.LogInformation($"Config location: {_selectedAccount.ConfigPath}");
        }
    }
    
    public DelegateCommand StartCopyCommand { get; set; }

    public CopyFilesViewModel(ILogger<CopyFilesViewModel> logger, IConfigFiles configFiles, IConfigCopy configCopy, IProcessViewer processViewer)
    {
        _logger = logger;
        _configFiles = configFiles;
        _configCopy = configCopy;
        _processViewer = processViewer;
        StartCopyCommand = new DelegateCommand(async () => await StartCopy());
        
        LoadAccounts();
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        _configCopy.FileCopying += OnFileCopying;
        _configCopy.ProgressChanged += OnProgressChanged;
        ProcessHelper.RequestCloseWoW += OnRequestCloseWoW;
        ProcessHelper.RequestStartWoW += OnRequestStartWoW;
    }
    
    private void OnRequestCloseWoW(object sender, string message)
    {
        MessageBox.Show(message, "Notification", MessageBoxButton.OK);
    }

    private void OnRequestStartWoW(object sender, string message)
    {
        MessageBox.Show(message, "Notification", MessageBoxButton.OK);
    }
    
    private void OnFileCopying(object? sender, string fileName)
    {
        CurrentLog += $"\nCopied file: {fileName}";
    }
    
    private void OnProgressChanged(object? sender, int progress)
    {
        ProgressBarValue = progress;
    }

    private async void LoadAccounts()
    {
        var accounts = await _configFiles.GetRealmsAccounts();
        accounts.Remove(accounts.FirstOrDefault(x => x.AccountName.Equals(AccountName)));
        Accounts = new ObservableCollection<RealmAccountsModel>(accounts);
    }

    private async Task StartCopy()
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        IsOperationInProgress = true;
        ProgressBarVisibility = Visibility.Visible;
        CopyButtonVisibility = Visibility.Collapsed;

        await ProcessHelper.EnsureWoWIsClosed(_processViewer);

        try
        {
            await Task.Run(() => 
                    _configCopy.CopyConfigFiles(SelectedAccount.ConfigPath, _sourceConfigLocation, _cts.Token, true, true), 
                _cts.Token);

            await ProcessHelper.PromptToStartWoW(_processViewer);
            

            await Task.Run(() => 
                    _configCopy.CopyConfigFiles(SelectedAccount.ConfigPath, _sourceConfigLocation, _cts.Token, false, false), 
                _cts.Token);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Copy operation was cancelled.");
            // Handle the cancellation (e.g., cleanup, user notification)
        }
        finally
        {
            IsOperationInProgress = false;
            ProgressBarVisibility = Visibility.Collapsed;
            CopyButtonVisibility = Visibility.Visible;
        }
    }


    
    public void InitializeWithParameters(NavigationParameters parameters)
    {
        _logger.LogInformation("Initializing CopyFilesViewModel with parameters");
        
        if (parameters.TryGetValue("accountName", out string accountName))
        {
            _logger.LogInformation($"Account name: {accountName}");
            AccountName = accountName;
        }
        
        if (parameters.TryGetValue("configLocation", out string configLocation))
        {
            _logger.LogInformation($"Source path: {configLocation}");
            _sourceConfigLocation = configLocation;
        }
    }
    
    private void CancelCopyOperation()
    {
        _cts?.Cancel();
    }


    public void Destroy()
    {
        CancelCopyOperation();
    }
}