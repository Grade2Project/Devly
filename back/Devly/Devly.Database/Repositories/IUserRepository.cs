#nullable enable
using Devly.Database.Models;

namespace Devly.Database.Repositories;

public interface IUserRepository
{
    Task InsertAsync(User user);
    Task UpdateAsync(User user);
    Task<User?> FindUserByLoginAsync(string login);
    Task<User> GetRandomUser();
}