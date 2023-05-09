using Devly.Database.Models;

namespace Devly.Database.Repositories.Abstract;

public interface IUsersFavoriteLanguagesRepository
{
    public Task<IReadOnlyList<UsersFavoriteLanguage>> GetUserFavoriteLanguages(string userLogin);
    Task InsertAllAsync(IEnumerable<ProgrammingLanguage> languages, string userLogin);
    Task UpdateAllForUserAsync(IEnumerable<ProgrammingLanguage> languages, string userLogin);
}