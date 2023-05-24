using Devly.Database.Basics.Repository;
using Devly.Database.Context;
using Devly.Database.Models;
using Devly.Database.Repositories.Abstract;

namespace Devly.Database.Repositories.Impl;

internal class FavoriteUsersRepository : IFavoriteUsersRepository
{
    private readonly IDbRepository<DevlyDbContext> _repository;

    public FavoriteUsersRepository(IDbRepository<DevlyDbContext> repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<int>> GetAllCompaniesLikedUser(string userLogin)
    {
        var result = await _repository.FindAllAsync<CompaniesFavoriteUser>
            (x => x.UserLogin == userLogin);
        return result.Select(x => x.CompanyId).ToList();
    }

    public async Task<IReadOnlyList<string>> GetAllUsersCompanyLiked(int companyId)
    {
        var result = await _repository.FindAllAsync<CompaniesFavoriteUser>
            (x => x.CompanyId == companyId);
        return result.Select(x => x.UserLogin).ToList();
    }

    public async Task InsertLikePair(int companyId, string userLogin)
    {
        await _repository.InsertAsync(new CompaniesFavoriteUser
        {
            CompanyId = companyId,
            UserLogin = userLogin
        });
    }

    public async Task Unlike(int companyId, string userLogin)
    {
        await _repository.DeleteAsync(_repository.FindAsync<CompaniesFavoriteUser>
            (x => x.CompanyId == companyId && x.UserLogin == userLogin));
    }
}