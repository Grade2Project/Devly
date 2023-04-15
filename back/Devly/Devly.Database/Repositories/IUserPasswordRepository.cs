using Devly.Database.Models;

namespace Devly.Database.Repositories;

public interface IUserPasswordRepository
{
    Task InsertAsync(string userLogin, string hashedPassword);
    Task<UserPassword> FindByUserLoginAsync(string login);
}