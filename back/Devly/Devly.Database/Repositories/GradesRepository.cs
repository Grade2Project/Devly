using Devly.Database.Basics.Repository;
using Devly.Database.Context;
using Devly.Database.Models;

namespace Devly.Database.Repositories;

internal class GradesRepository : IGradesRepository
{
    private readonly IDbRepository<DevlyDbContext> _repository;

    public GradesRepository(IDbRepository<DevlyDbContext> repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyList<User>> FindAllUsersByGrade(string grade)
    {
        return _repository.FindAllAsync<User>(user => user.Grade.Value == grade, default, 
            u => u.Grade);
    }
}