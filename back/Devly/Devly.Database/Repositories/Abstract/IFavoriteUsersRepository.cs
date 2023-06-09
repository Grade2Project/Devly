using Devly.Database.Models;

namespace Devly.Database.Repositories.Abstract;

public interface IFavoriteUsersRepository
{
    Task<IReadOnlyList<User>>? GetAllUsersCompanyLiked(int companyId);
    Task<IReadOnlyList<int>> GetAllCompaniesLikedUser(string userLogin);
    Task InsertLikePair(int companyId, string userLogin);
    Task Unlike(int companyId, string userLogin);
}