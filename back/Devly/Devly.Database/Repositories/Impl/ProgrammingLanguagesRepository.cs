using Devly.Database.Basics.Repository;
using Devly.Database.Context;
using Devly.Database.Models;
using Devly.Database.Repositories.Abstract;

namespace Devly.Database.Repositories.Impl;

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

    public async Task<IReadOnlyList<ProgrammingLanguage>> FindLanguagesAsync(params string[] languageNames)
    {
        return await _repository.FindAllAsync<ProgrammingLanguage>(p => languageNames.Contains(p.LanguageName))
            .ConfigureAwait(false);
    }
}