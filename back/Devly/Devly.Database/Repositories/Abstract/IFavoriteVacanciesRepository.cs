namespace Devly.Database.Repositories.Abstract;

public interface IFavoriteVacanciesRepository
{
    Task<IReadOnlyList<int>>? GetAllVacanciesUserLiked(string userLogin);
    Task<IReadOnlyList<string>>? GetAllUsersLikedVacancy(int vacancyId);
    Task InsertLikePair(string userLogin, int vacancyId);
    Task Unlike(string userLogin, int vacancyId);
}