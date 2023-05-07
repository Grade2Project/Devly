using Devly.Database.Models;

namespace Devly.Database.Repositories;

public interface ICompaniesPasswordsRepository
{
    public Task InsertAsync(int companyId, string hashedPass);
    public Task<string> GetPasswordById(int companyId);
}