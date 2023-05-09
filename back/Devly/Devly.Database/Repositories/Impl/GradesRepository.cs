using Devly.Database.Basics.Repository;
using Devly.Database.Context;
using Devly.Database.Models;
using Devly.Database.Repositories.Abstract;

namespace Devly.Database.Repositories.Impl;

internal class GradesRepository : IGradesRepository
{
    private readonly IDbRepository<DevlyDbContext> _repository;

    public GradesRepository(IDbRepository<DevlyDbContext> repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<User>> FindAllUsersByGrade(string grade)
    {
        return await _repository.FindAllAsync<User>(user => user.Grade.Value == grade, default, 
            u => u.Grade);
    }

    public async Task<Grade> FindGrade(string gradeName)
    {
        return await _repository.FindAsync<Grade>(grade => grade.Value == gradeName);
    }

    public async Task<IReadOnlyList<Grade>> GetAllGrades()
    {
        return await _repository.FindAllAsync<Grade>(grade => grade.Value != null).ConfigureAwait(false);
    }
}