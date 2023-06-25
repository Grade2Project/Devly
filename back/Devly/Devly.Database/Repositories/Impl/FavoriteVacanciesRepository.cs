using Devly.Database.Basics.Repository;
using Devly.Database.Context;
using Devly.Database.Models;
using Devly.Database.Repositories.Abstract;

namespace Devly.Database.Repositories.Impl;

internal class FavoriteVacanciesRepository : IFavoriteVacanciesRepository
{
    private readonly IDbRepository<DevlyDbContext> _repository;

    public FavoriteVacanciesRepository(IDbRepository<DevlyDbContext> repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<Vacancy>> GetAllVacanciesUserLiked(string userLogin)
    {
        var result = await _repository.FindAllAsync<UsersFavoriteVacancy>
            (x => x.UserLogin == userLogin, CancellationToken.None, v => v.Vacancy).ConfigureAwait(false);
        return result.Select(x => x.Vacancy).ToList();
    }

    public async Task<IReadOnlyList<string>> GetAllUsersLikedVacancy(int vacancyId)
    {
        var result = await _repository.FindAllAsync<UsersFavoriteVacancy>
            (x => x.VacancyId == vacancyId).ConfigureAwait(false);
        return result.Select(x => x.UserLogin).ToList();
    }

    public async Task InsertLikePair(string userLogin, int vacancyId)
    {
        await _repository.InsertAsync(new UsersFavoriteVacancy
        {
            UserLogin = userLogin,
            VacancyId = vacancyId
        });
    }

    public async Task Unlike(string userLogin, int vacancyId)
    {
        await _repository.DeleteAsync(await _repository.FindAsync<UsersFavoriteVacancy>
            (x => x.UserLogin == userLogin && x.VacancyId == vacancyId));
    }
}