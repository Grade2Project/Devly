using Devly.Database.Basics.Repository;
using Devly.Database.Context;
using Devly.Database.Models;

namespace Devly.Database.Repositories;

internal class ProgrammingLanguagesRepository : IProgrammingLanguagesRepository
{
    private readonly IDbRepository<DevlyDbContext> _repository;

    public ProgrammingLanguagesRepository(IDbRepository<DevlyDbContext> repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<ProgrammingLanguage>> GetAllLanguages()
    {
        return await _repository.FindAllAsync<ProgrammingLanguage>(x => true).ConfigureAwait(false);
    }

    public async Task<ProgrammingLanguage> FindLanguageAsync(string languageName)
    {
        return await _repository.FindAsync<ProgrammingLanguage>
            (language => language.LanguageName == languageName).ConfigureAwait(false);
    }
}