namespace Devly.Helpers;

internal class FileHelper : IFileHelper
{
    public FileStream OpenWrite(string path)
    {
        return File.OpenWrite(path);
    }
}