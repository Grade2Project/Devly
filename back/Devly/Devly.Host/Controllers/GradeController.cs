using Devly.Database.Repositories.Abstract;
using Devly.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Devly.Controllers;

[Route("grades")]
public class GradeController : Controller
{
    private readonly IReadOnlyList<string> _grades;
    private readonly IGradesRepository _repository;

    public GradeController(IGradesRepository repository)
    {
        _repository = repository;
        _grades = _repository.GetAllGrades().Result.Select(x => x.Value).ToList();
    }
    
    [Authorize]
    [HttpGet, Route("get")]
    public Task<ArrayDto<string>> GetGrades()
    {
        var a = HttpContext.User.Claims.ToList();
        return Task.FromResult(new ArrayDto<string>(_grades));
    }
}