namespace Devly.Helpers;

public class PhotoHelper : IPhotoHelper
{
    private readonly IFileHelper _file;

    public PhotoHelper(IFileHelper fileHelper)
    {
        _file = fileHelper;
    }

    public async Task Save(byte[] photo, string path)
    {
        await using var writer = new BinaryWriter(_file.OpenWrite(path));
        writer.Write(photo);
    }

    public async Task<byte[]> LoadFrom(string path)
    {
        return await Task.Run(() =>
        {
            using var reader = new BinaryReader(_file.OpenRead(path));
            var length = reader.BaseStream.Length - reader.BaseStream.Position;
            return reader.ReadBytes((int)length);
        });
    }

    public async Task Delete(string path)
    {
        _file.Delete(path);
    }
}