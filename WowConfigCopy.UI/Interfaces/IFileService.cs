using System.Threading.Tasks;

namespace WowConfigCopy.UI.Interfaces;

public interface IFileService
{
    Task<string> ViewFileContents(string filePath);
    Task SaveFileContents(string filePath, string content);
    void ViewFile(string filePath);
}