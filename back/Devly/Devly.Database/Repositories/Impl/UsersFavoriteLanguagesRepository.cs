using Devly.Database.Basics.Repository;
using Devly.Database.Context;
using Devly.Database.Models;
using Devly.Database.Repositories.Abstract;

namespace Devly.Database.Repositories.Impl;

internal class UsersFavoriteLanguagesRepository : IUsersFavoriteLanguagesRepository
{
    private readonly IDbRepository<DevlyDbContext> _repository;

    public UsersFavoriteLanguagesRepository(IDbRepository<DevlyDbContext> repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<UsersFavoriteLanguage>> GetUserFavoriteLanguages(string userLogin)
    {
        return await _repository.FindAllAsync<UsersFavoriteLanguage>
            (x => x.UserLogin == userLogin,
                CancellationToken.None, language => language.ProgrammingLanguage)
            .ConfigureAwait(false);
    }

    public async Task InsertAllAsync(IEnumerable<ProgrammingLanguage> languages, string userLogin)
    {
        await _repository.InsertAllAsync(languages.Select(language => new UsersFavoriteLanguage
        {
            UserLogin = userLogin,
            ProgrammingLanguageId = language.Id
        }));
    }

    public async Task UpdateAllForUserAsync(IEnumerable<ProgrammingLanguage> languages, string userLogin)
    {
        await _repository.DeleteAllAsync(await GetUserFavoriteLanguages(userLogin));
        await InsertAllAsync(languages, userLogin);
    }
}