using System.Threading.Tasks;
using System.Windows;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using WowConfigCopy.UI.Interfaces;

namespace WowConfigCopy.UI.ViewModels;

public class EditFileViewModel : BindableBase, IInitializeWithParameters
{
    private readonly ILogger<EditFileViewModel> _logger;
    private readonly IFileService _fileService;
    private readonly INotificationService _notificationService;
    
    private string _fileName = string.Empty;
    private string _fileContents;
    private string _fileLocation;
    private string _originalFileContents;
    private IHighlightingDefinition _syntaxHighlighting;
    private string _statusMessage;
    private Visibility _statusVisibility = Visibility.Collapsed;
    private string _statusColour;
    
    public string StatusColour
    {
        get => _statusColour;
        set => SetProperty(ref _statusColour, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public Visibility StatusVisibility
    {
        get => _statusVisibility;
        set => SetProperty(ref _statusVisibility, value);
    }

    
    public string FileName
    {
        get => _fileName;
        set => SetProperty(ref _fileName, value);
    }
    
    public string FileContents
    {
        get => _fileContents;
        set => SetProperty(ref _fileContents, value);
    }
    
    public IHighlightingDefinition SyntaxHighlighting
    {
        get => _syntaxHighlighting;
        set => SetProperty(ref _syntaxHighlighting, value);
    }
    
    public DelegateCommand SaveFileCommand { get; set; }

    public EditFileViewModel(ILogger<EditFileViewModel> logger, IFileService fileService, INotificationService notificationService)
    {
        _logger = logger;
        _fileService = fileService;
        _notificationService = notificationService;
        SaveFileCommand = new DelegateCommand(SaveFile);
    }
    
    public void InitializeWithParameters(NavigationParameters parameters)
    {
        _logger.LogInformation("EditFileViewModel initialized with parameters");
        
        if (parameters.TryGetValue("fileLocation", out string configLocation))
        {
            _logger.LogInformation($"Config location: {configLocation}");
            _fileLocation = configLocation;
        }
        
        if (parameters.TryGetValue("fileName", out string fileName))
        {
            _logger.LogInformation($"File name: {fileName}");
            _fileName = fileName;
        }
        
        LoadFile(configLocation);
    }

    private async void LoadFile(string filePath)
    {
        FileContents = await _fileService.ViewFileContents(filePath);
        _originalFileContents = FileContents;
        DetermineSyntaxHighlighting(filePath);
    }

    private async void SaveFile()
    {
        if (!HasFileChanged())
        {
            _logger.LogInformation("File has not changed, no need to save.");
            StatusMessage = "File has not changed, no need to save.";
            StatusVisibility = Visibility.Visible;
            StatusColour = "#FF0000";
            _notificationService.ShowNotification("File has not changed, no need to save.");
            return;
        }
        await _fileService.SaveFileContents(_fileLocation, FileContents);
        _originalFileContents = FileContents;
        
        StatusMessage = "File saved successfully!";
        StatusVisibility = Visibility.Visible;
        StatusColour = "#00FF00";
        _notificationService.ShowNotification("File saved successfully!");
        
        await Task.Delay(2000);
        StatusVisibility = Visibility.Collapsed;
    }
    
    private bool HasFileChanged()
    {
        return FileContents != _originalFileContents;
    }

    
    private void DetermineSyntaxHighlighting(string filePath)
    {
        //TODO: Add logic to determine syntax highlighting based on file extension
        SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("Plain Text");
    }
}