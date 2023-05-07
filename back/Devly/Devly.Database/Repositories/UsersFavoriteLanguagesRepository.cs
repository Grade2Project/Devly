using Devly.Database.Basics.Repository;
using Devly.Database.Context;
using Devly.Database.Models;

namespace Devly.Database.Repositories;

internal class UsersFavoriteLanguagesRepository : IUsersFavoriteLanguagesRepository
{
    private readonly IDbRepository<DevlyDbContext> _repository;
    private readonly IProgrammingLanguagesRepository _programmingLanguagesRepository;

    public UsersFavoriteLanguagesRepository(IDbRepository<DevlyDbContext> repository,
        IProgrammingLanguagesRepository programmingLanguagesRepository)
    {
        _repository = repository;
        _programmingLanguagesRepository = programmingLanguagesRepository;
    }
    
    public async Task<IReadOnlyList<UsersFavoriteLanguage>> GetUserFavoriteLanguages(string userLogin)
    {
        return await _repository.FindAllAsync<UsersFavoriteLanguage>(x => x.UserLogin == userLogin)
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