using Devly.Database.Models;

namespace Devly.Database.Repositories.Abstract;

public interface IGradesRepository
{
    public Task<IReadOnlyList<User>> FindAllUsersByGrade(string grade);
    public Task<Grade>? FindGrade(string gradeName);
    public Task<IReadOnlyList<Grade>> GetAllGrades();
}