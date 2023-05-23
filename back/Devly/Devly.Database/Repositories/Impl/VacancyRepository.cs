using Devly.Database.Basics.Repository;
using Devly.Database.Context;
using Devly.Database.Models;
using Devly.Database.Repositories.Abstract;

namespace Devly.Database.Repositories.Impl;

internal class VacancyRepository : IVacancyRepository
{
    private readonly IDbRepository<DevlyDbContext> _repository;

    public VacancyRepository(IDbRepository<DevlyDbContext> repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<Vacancy>> GetAllCompanyVacancies(string companyEmail)
    {
        return await _repository.FindAllAsync<Vacancy>(x => x.Company.CompanyEmail == companyEmail,
            CancellationToken.None, vacancy => vacancy.Company,
            vacancy => vacancy.ProgrammingLanguage,
            vacancy => vacancy.Grade);
    }

    public async Task<Vacancy> FindVacancyAsync(Vacancy vacancy)
    {
        return await _repository.FindAsync<Vacancy>(x => x.Info == vacancy.Info &&
                                                         x.Salary == vacancy.Salary &&
                                                         x.CompanyId == vacancy.CompanyId &&
                                                         x.ProgrammingLanguageId == vacancy.ProgrammingLanguageId);
    }

    public async Task<Vacancy> FindVacancyByIdAsync(int id)
    {
        return await _repository.FindAsync<Vacancy>(x => x.Id == id);
    }

    public async Task<IReadOnlyList<Vacancy>>? GetAllLanguageVacancies(string languageName)
    {
        return await _repository.FindAllAsync<Vacancy>(v => v.ProgrammingLanguage.LanguageName == languageName,
            CancellationToken.None, vacancy => vacancy.Company,
            vacancy => vacancy.ProgrammingLanguage,
            vacancy => vacancy.Grade);
    }

    public async Task InsertAsync(Vacancy vacancy)
    {
        await _repository.InsertAsync(vacancy);
    }

    public async Task DeleteAsync(Vacancy vacancy)
    {
        await _repository.DeleteAsync(vacancy);
    }

    public async Task UpdateAsync(Vacancy vacancy)
    {
        await _repository.UpdateAsync(vacancy);
    }

    public async Task<Vacancy> GetRandomVacancy()
    {
        return await _repository.GetNextRandom<Vacancy>
            (CancellationToken.None, vacancy => vacancy.Company, vacancy => vacancy.ProgrammingLanguage);
    }
}