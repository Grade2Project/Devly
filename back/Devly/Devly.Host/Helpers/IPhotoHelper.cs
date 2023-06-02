namespace Devly.Helpers;

public interface IPhotoHelper
{ 
    Task Save(byte[] photo, string path);
    Task<byte[]> LoadFrom(string path);
}