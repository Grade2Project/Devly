using Devly.Database.Filters;
using Devly.Database.Models;

namespace Devly.Database.Repositories.Abstract;

public interface IVacancyRepository
{
    public Task<IReadOnlyList<Vacancy>> GetAllCompanyVacancies(string companyEmail);
    public Task<Vacancy?> FindVacancyAsync(Vacancy vacancy);
    public Task<Vacancy?> FindVacancyByIdAsync(int id);
    public Task<IReadOnlyList<Vacancy>>? GetAllLanguageVacancies(string languageName);
    public Task<IReadOnlyList<Vacancy>>? GetAllGradeVacancies(int gradeId);
    public Task<IReadOnlyList<Vacancy>>? GetAllVacanciesFilter(VacancyFilter vacancyFilter, IEnumerable<int>? except = null);

    public Task InsertAsync(Vacancy vacancy);
    public Task DeleteAsync(Vacancy vacancy);
    public Task UpdateAsync(Vacancy vacancy);
    public Task<Vacancy> GetRandomVacancy();
}