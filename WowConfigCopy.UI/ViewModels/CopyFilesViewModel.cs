using Microsoft.Extensions.Logging;
using Prism.Mvvm;

namespace WowConfigCopy.UI.ViewModels;

public class CopyFilesViewModel : BindableBase
{
    private readonly ILogger<CopyFilesViewModel> _logger;

    public CopyFilesViewModel(ILogger<CopyFilesViewModel> logger)
    {
        _logger = logger;
    }
}