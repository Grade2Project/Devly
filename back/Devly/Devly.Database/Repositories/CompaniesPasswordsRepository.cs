using Devly.Database.Basics.Repository;
using Devly.Database.Context;
using Devly.Database.Models;

namespace Devly.Database.Repositories;

internal class CompaniesPasswordsRepository : ICompaniesPasswordsRepository
{
    private readonly IDbRepository<DevlyDbContext> _repository;
    
    public CompaniesPasswordsRepository(IDbRepository<DevlyDbContext> repository)
    {
        _repository = repository;
    }

    public async Task InsertAsync(int companyId, string hashedPass)
    {
        await _repository.InsertAsync(new CompanyPassword
        {
            CompanyId = companyId,
            HashedPass = hashedPass
        });
    }

    public async Task<string> GetPasswordById(int companyId)
    {
        var company = await _repository.FindAsync<CompanyPassword>(x => x.CompanyId == companyId);
        return company.HashedPass;
    }
}