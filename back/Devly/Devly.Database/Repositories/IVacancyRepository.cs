using Devly.Database.Models;

namespace Devly.Database.Repositories;

public interface IVacancyRepository
{
    public Task<IReadOnlyList<Vacancy>> GetAllCompanyVacancies(int companyId);
    public Task<Vacancy> FindVacancyAsync(Vacancy vacancy); 
    public Task InsertAsync(Vacancy vacancy);
    public Task DeleteAsync(Vacancy vacancy);
    public Task UpdateAsync(Vacancy vacancy);
    public Task<Vacancy> GetRandomVacancy();
}