using Devly.Database.Repositories;
using Devly.Database.Repositories.Abstract;
using Devly.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Devly.Controllers;

[Route("lang")]
public class LanguageController : Controller
{
    private readonly IProgrammingLanguagesRepository _repository;

    public LanguageController(IProgrammingLanguagesRepository repository)
    {
        _repository = repository;
    }
    
    [Authorize]
    [HttpGet, Route("get")]
    public Task<ArrayDto<string>> GetAllLanguages()
    {
        var languagesDto = new ArrayDto<string>(_repository.GetAllLanguages().Result
            .Select(x => x.LanguageName).ToList());
        return Task.FromResult(languagesDto);
    }
}