namespace Devly.Helpers;

internal class FileHelper : IFileHelper
{
    public FileStream OpenWrite(string path)
    {
        Directory.CreateDirectory("../photos/");
        return File.Create(path);
    }

    public FileStream OpenRead(string path)
    {
        return File.OpenRead(path);
    }

    public void Delete(string path)
    {
        File.Delete(path);
    }
}