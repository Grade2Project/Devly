#nullable enable
using Devly.Database.Filters;
using Devly.Database.Models;

namespace Devly.Database.Repositories.Abstract;

public interface IUserRepository
{
    Task InsertAsync(User user);
    Task UpdateAsync(User user);
    Task<User?> FindUserByLoginAsync(string login);
    Task<User> GetRandomUser();
    Task<IReadOnlyList<User>> GetAllUsersFilter(UserFilter userFilter);
 
}