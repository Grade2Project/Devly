using Devly.Database.Models;

namespace Devly.Database.Repositories.Abstract;

public interface IProgrammingLanguagesRepository
{
    public Task<IReadOnlyList<ProgrammingLanguage>> GetAllLanguages();
    public Task<IReadOnlyList<ProgrammingLanguage>> FindLanguagesAsync(params string[] languageNames);
}