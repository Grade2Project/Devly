using Devly.Database.Models;

namespace Devly.Database.Repositories.Abstract;

public interface ICitiesRepository
{
    public Task<IReadOnlyList<City>> GetAllCities();
    public Task<IReadOnlyList<City>> GetCitiesByPattern(string pattern);
    public Task<City?> GetCityByName(string cityName);
}