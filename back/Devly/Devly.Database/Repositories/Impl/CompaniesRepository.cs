using Devly.Database.Basics.Repository;
using Devly.Database.Context;
using Devly.Database.Models;
using Devly.Database.Repositories.Abstract;

namespace Devly.Database.Repositories.Impl;

internal class CompaniesRepository : ICompaniesRepository
{
    private readonly IDbRepository<DevlyDbContext> _repository;

    public CompaniesRepository(IDbRepository<DevlyDbContext> repository)
    {
        _repository = repository;
    }

    public async Task<Company> InsertAsync(Company company)
    {
        await _repository.InsertAsync(company);
        return company;
    }

    public async Task<Company> GetCompanyByName(string companyName)
    {
        return await _repository.FindAsync<Company>(x => x.CompanyName == companyName).ConfigureAwait(false);
    }

    public async Task<Company> GetCompanyByEmail(string companyEmail)
    {
        return await _repository.FindAsync<Company>(x => x.CompanyEmail == companyEmail,
            CancellationToken.None, company => company.Vacancies).ConfigureAwait(false);
    }

    public async Task UpdateAsync(Company company)
    {
        await _repository.UpdateAsync(company);
    }
}