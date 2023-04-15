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

    public async Task InsertAsync(string userLogin, string hashedPassword)
    {
        await _repository.InsertAsync(new UsersPassword
        {
            UserLogin = userLogin,
            HashedPass = hashedPassword
        }).ConfigureAwait(false);
    }

    public async Task<UsersPassword> FindByUserLoginAsync(string login)
    {
        return await _repository.FindAsync<UsersPassword>(u => u.UserLogin == login).ConfigureAwait(false);
    }
}