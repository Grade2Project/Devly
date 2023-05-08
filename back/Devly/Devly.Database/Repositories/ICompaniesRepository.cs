using Devly.Database.Models;

namespace Devly.Database.Repositories;

public interface ICompaniesRepository
{
    Task<Company> InsertAsync(string companyName, string companyEmail, string info);
    Task<Company> GetCompanyByName(string companyName);
    Task<Company> GetCompanyByEmail(string companyEmail);
}