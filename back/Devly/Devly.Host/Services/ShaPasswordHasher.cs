using System.Security.Cryptography;
using System.Text;
using Devly.Configs;

namespace Devly.Services;

public class ShaPasswordHasher : IPasswordHasher
{
    private readonly AuthConfig _config;

    public ShaPasswordHasher(AuthConfig config)
    {
        _config = config;
    }

    public string HashPassword(string password)
    {
        password += _config.Salt;
        using var sha256 = new SHA256Managed();
        return BitConverter.ToString(sha256.ComputeHash
            (Encoding.UTF8.GetBytes(password))).Replace("-", "");
    }
}