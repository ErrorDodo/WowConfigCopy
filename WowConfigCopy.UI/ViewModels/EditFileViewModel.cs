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
    
    private string _fileName = string.Empty;
    private string _fileContents;
    private IHighlightingDefinition _syntaxHighlighting;
    
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

    public EditFileViewModel(ILogger<EditFileViewModel> logger, IFileService fileService)
    {
        _logger = logger;
        _fileService = fileService;
        SaveFileCommand = new DelegateCommand(() => SaveFile(_fileName));
    }
    
    public void InitializeWithParameters(NavigationParameters parameters)
    {
        _logger.LogInformation("EditFileViewModel initialized with parameters");
        
        if (parameters.TryGetValue("fileLocation", out string configLocation))
        {
            _logger.LogInformation($"Config location: {configLocation}");
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
        DetermineSyntaxHighlighting(filePath);
    }
    
    public async void SaveFile(string filePath)
    {
        await _fileService.SaveFileContents(filePath, FileContents);
    }
    
    private void DetermineSyntaxHighlighting(string filePath)
    {
        //TODO: Add logic to determine syntax highlighting based on file extension
        SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("Plain Text");
    }
}