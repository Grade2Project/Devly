using Devly.Database.Basics.Repository;
using Devly.Database.Context;
using Devly.Database.Models;

namespace Devly.Database.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly IDbRepository<DevlyDbContext> _repository;

    public UserRepository(IDbRepository<DevlyDbContext> repository, Random random)
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
        var users = await _repository.FindAllAsync<User>
            (x => x.Login != null, CancellationToken.None).ConfigureAwait(false);
        var maxId = users.Max(x => x.ContactId);
        var randomId = Random.Shared.Next(1, maxId + 1);
        return await _repository.FindAsync<User>(x => x.ContactId == randomId, CancellationToken.None,
            user => user.Contact, user => user.Grade).ConfigureAwait(false);
    }
}