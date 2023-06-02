namespace Devly.Helpers;

internal class FileHelper : IFileHelper
{
    public FileStream OpenWrite(string path)
    {
        return File.OpenWrite(path);
    }

    public FileStream OpenRead(string path)
    {
        return File.OpenRead(path);
    }
}