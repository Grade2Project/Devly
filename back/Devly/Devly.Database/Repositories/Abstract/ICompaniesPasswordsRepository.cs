namespace Devly.Database.Repositories.Abstract;

public interface ICompaniesPasswordsRepository
{
    public Task InsertAsync(int companyId, string hashedPass);
    public Task<string> GetPasswordById(int companyId);
}