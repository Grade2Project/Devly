using Devly.Database.Basics.Repository;
using Devly.Database.Context;
using Devly.Database.Filters;
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

    public async Task<User?> FindUserByLoginAsync(string login)
    {
        return await _repository.FindAsync<User>(u => u.Login == login, CancellationToken.None, 
            user => user.Contact, user => user.Grade, user => user.FavoriteLanguages).ConfigureAwait(false);
    }

    public async Task<User> GetRandomUser()
    {
        return await _repository.GetNextRandom<User>
            (CancellationToken.None, user => user.Contact, user => user.Grade, user => user.FavoriteLanguages);
    }

    public async Task<IReadOnlyList<User>>? GetUsersLeqThanGrade(int gradeId)
    {
        return await _repository.FindAllAsync<User>(user => user.GradeId <= gradeId,
            CancellationToken.None, user => user.Contact, user => user.Grade, user => user.FavoriteLanguages);
    }

    public async Task<IReadOnlyList<User>> GetUsersGeqThanGrade(int gradeId)
    {
        return await _repository.FindAllAsync<User>(user => user.GradeId >= gradeId,
            CancellationToken.None, user => user.Contact, user => user.Grade, user => user.FavoriteLanguages);
    }

    public async Task<IReadOnlyList<User>> GetAllUsersFilter(UserFilter userFilter, string[]? except = null)
    {
        return await _repository.FindAllAsync<User>(user =>
                (except == null || !except.Contains(user.Login)) &&
            (userFilter.City == default || user.City == userFilter.City) &&
            (userFilter.ExperienceFrom == 0 || user.Experience >= userFilter.ExperienceFrom) &&
            (userFilter.UserName == default || user.Name.Contains(userFilter.UserName)) &&
            (userFilter.GradeIds == default || userFilter.GradeIds.Contains(user.GradeId)) &&
            (userFilter.LanguageIds == default ||
             user.FavoriteLanguages.Any(x => userFilter.LanguageIds.Contains(x.ProgrammingLanguageId))),
            CancellationToken.None, user => user.Contact, user => user.Grade, user => user.FavoriteLanguages);
    }

    public async Task<IReadOnlyList<User>> GetUsersEqGrade(int gradeId)
    {
        return await _repository.FindAllAsync<User>(user => user.GradeId == gradeId,
            CancellationToken.None, user => user.Contact, user => user.Grade, user => user.FavoriteLanguages);
    }
}