using Devly.Database.Models;

namespace Devly.Database.Repositories.Abstract;

public interface ICompaniesRepository
{
    Task<Company> InsertAsync(Company company);
    Task<Company?> GetCompanyByName(string companyName);
    Task<Company?> GetCompanyByEmail(string companyEmail);
}