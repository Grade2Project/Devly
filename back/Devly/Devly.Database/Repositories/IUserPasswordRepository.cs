using Devly.Database.Models;

namespace Devly.Database.Repositories;

public interface IUserPasswordRepository
{
    Task InsertAsync(int userId, string hashedPassword);
    Task<UserPassword> FindByUserLoginAsync(string login);
}