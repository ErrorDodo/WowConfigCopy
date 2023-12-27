namespace WowConfigCopy.Common.Helpers;

public class FileHelpers
{
            
    public IEnumerable<string> GetDirectoriesSafe(string path)
    {
        return Directory.Exists(path) ? Directory.GetDirectories(path) : Array.Empty<string>();
    }
        
    public IEnumerable<string> GetFilesSafe(string path)
    {
        return Directory.Exists(path) ? Directory.GetFiles(path) : Array.Empty<string>();
    }
}