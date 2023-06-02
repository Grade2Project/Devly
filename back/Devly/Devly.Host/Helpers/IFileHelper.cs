namespace Devly.Helpers;

public interface IFileHelper
{
    public FileStream OpenWrite(string path);
    public FileStream OpenRead(string path);
}