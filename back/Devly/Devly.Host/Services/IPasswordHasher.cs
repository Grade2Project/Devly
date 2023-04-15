namespace Devly.Services;

public interface IPasswordHasher
{
    string HashPassword(string password);
}