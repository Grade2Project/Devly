using Devly.Database.Basics.Repository;
using Devly.Database.Context;
using Devly.Database.Models;

namespace Devly.Database.Repositories;

internal class UserPasswordRepository : IUserPasswordRepository
{
    private readonly IDbRepository<DevlyDbContext> _repository;

    public UserPasswordRepository(IDbRepository<DevlyDbContext> repository)
    {
        _repository = repository;
    }

    public async Task InsertAsync(int userId, string hashedPassword)
    {
        await _repository.InsertAsync(new UserPassword()
        {
            UserId = userId,
            HashedPass = hashedPassword
        }).ConfigureAwait(false);
    }

    public async Task<UserPassword> FindByUserLoginAsync(string login)
    {
        return await _repository.FindAsync<UserPassword>(u => u.User.Login == login).ConfigureAwait(false);
    }
}