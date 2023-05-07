using Devly.Database.Basics.Repository;
using Devly.Database.Context;
using Devly.Database.Models;

namespace Devly.Database.Repositories;

internal class CompaniesRepository : ICompaniesRepository
{
    private readonly IDbRepository<DevlyDbContext> _repository;

    public CompaniesRepository(IDbRepository<DevlyDbContext> repository)
    {
        _repository = repository;
    }

    public async Task UpdateAsync(Company company)
    {
        await _repository.UpdateAsync(company);
    }

    public async Task InsertAsync(string companyName, string info)
    {
        await _repository.InsertAsync(new Company()
        {
            CompanyName = companyName,
            Info = info
        });
    }

    public async Task<Company> GetCompanyByName(string companyName)
    {
        return await _repository.FindAsync<Company>(x => x.CompanyName == companyName).ConfigureAwait(false);
    }
}