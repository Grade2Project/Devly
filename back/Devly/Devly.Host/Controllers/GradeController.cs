using Devly.Database.Repositories;
using Devly.Models;
using Microsoft.AspNetCore.Mvc;

namespace Devly.Controllers;

[Route("grades")]
public class GradeController : Controller
{
    private readonly IGradesRepository _repository;
    private readonly IReadOnlyList<string> _grades;

    public GradeController(IGradesRepository repository)
    {
        _repository = repository;
        _grades = _repository.GetAllGrades().Result.Select(x => x.Value).ToList();
    }

    [HttpGet, Route("get")]
    public Task<ArrayDto<string>> GetGrades()
    {
        return Task.FromResult(new ArrayDto<string>(_grades));
    }
}