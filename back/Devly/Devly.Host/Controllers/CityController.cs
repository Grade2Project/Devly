using Devly.Database.Repositories.Abstract;
using Devly.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Devly.Controllers;

[Route("cities")]
public class CityController : Controller
{
    private readonly ICitiesRepository _cities;
    private readonly ILogger<CityController> _logger;

    public CityController(ICitiesRepository cities, ILogger<CityController> logger)
    {
        _cities = cities;
        _logger = logger;
    }
    
    [Authorize]
    [Route("similar")]
    [HttpGet]
    public async Task<ArrayDto<string>> CitiesSimilarTo(string pattern)
    {
        _logger.LogInformation(pattern);
        var cities = await _cities.GetCitiesByPattern(pattern);
        return new ArrayDto<string>(cities.Select(x => x.Name).ToList());
    }
    
    [Authorize]
    [HttpGet, Route("all")]
    public async Task<ArrayDto<string>> GetAllCities()
    {
        return new ArrayDto<string>((await _cities.GetAllCities()).Select(c => c.Name).ToList());
    }
}