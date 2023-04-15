using Devly.Database.Models;

namespace Devly.Database.Repositories;

public interface IGradesRepository
{
    public Task<IReadOnlyList<User>> FindAllUsersByGrade(string grade);
}