using Devly.Database.Models;

namespace Devly.Database.Repositories;

public interface IProgrammingLanguagesRepository
{
    public Task<IReadOnlyList<ProgrammingLanguage>> GetAllLanguages();
    public Task<ProgrammingLanguage> FindLanguageAsync(string languageName);
}