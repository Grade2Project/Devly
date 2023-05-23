using Devly.Database.Basics.Repository;
using Devly.Database.Context;
using Devly.Database.Models;
using Devly.Database.Repositories.Abstract;

namespace Devly.Database.Repositories.Impl;

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
        return await _repository.GetNextRandom<User>
            (CancellationToken.None, user => user.Contact, user => user.Grade, user => user.FavoriteLanguages);
    }

    public async Task<IReadOnlyList<User>>? GetUsersByGrade(int gradeId)
    {
        return await _repository.FindAllAsync<User>(user => user.GradeId <= gradeId,
            CancellationToken.None, user => user.Contact, user => user.Grade, user => user.FavoriteLanguages);
    }
}