using Devly.Database.Repositories.Abstract;
using Devly.Models;
using Microsoft.AspNetCore.Mvc;

namespace Devly.Controllers;

[Route("cities")]
public class CityController : Controller
{
    private readonly ICitiesRepository _cities;

    public CityController(ICitiesRepository cities)
    {
        _cities = cities;
    }

    [Route("similar")]
    [HttpGet]
    public async Task<ArrayDto<string>> CitiesSimilarTo(string pattern)
    {
        var cities = await _cities.GetCitiesByPattern(pattern);
        return new ArrayDto<string>(cities.Select(x => x.Name).ToArray());
    }
}