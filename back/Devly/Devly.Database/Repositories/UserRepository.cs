using Devly.Database.Basics.Repository;
using Devly.Database.Context;
using Devly.Database.Models;

namespace Devly.Database.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly IDbRepository<DevlyDbContext> _repository;

    public UserRepository(IDbRepository<DevlyDbContext> repository)
    {
        _repository = repository;
    }

    public async Task InsertAsync(User user)
    {
        await _repository.InsertAsync(user).ConfigureAwait(false);
    }

    public async Task UpdateAsync(User user)
    {
        await _repository.UpdateAsync(user);
    }

    public async Task<User> FindUserByLoginAsync(string login)
    {
        return await _repository.FindAsync<User>(u => u.Login == login).ConfigureAwait(false);
    }

    public async Task<User> GetRandomUser()
    {
        return await _repository.FindAsync<User>(u => u.Name != null, CancellationToken.None,
            x => x.Contact).ConfigureAwait(false);
    }
}