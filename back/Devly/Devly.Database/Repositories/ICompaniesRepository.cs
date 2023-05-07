using Devly.Database.Models;

namespace Devly.Database.Repositories;

public interface ICompaniesRepository
{
    Task InsertAsync(string companyName, string info);
    Task<Company> GetCompanyByName(string companyName);
}