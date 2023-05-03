using Devly.Database.Repositories;
using Devly.Models;
using Microsoft.AspNetCore.Mvc;

namespace Devly.Controllers;

[Route("grades")]
public class GradeController : Controller
{
    private readonly IGradesRepository _repository;
    private readonly IReadOnlyList<string> grades;

    public GradeController(IGradesRepository repository)
    {
        _repository = repository;
        grades = _repository.GetAllGrades().Result.Select(x => x.Value).ToList();
    }

    [HttpGet, Route("get")]
    public async Task<ArrayDto<string>> GetGrades()
    {
        return new ArrayDto<string>(grades);
    }
}