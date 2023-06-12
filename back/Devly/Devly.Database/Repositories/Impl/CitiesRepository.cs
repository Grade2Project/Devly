using Devly.Database.Basics.Repository;
using Devly.Database.Context;
using Devly.Database.Models;
using Devly.Database.Repositories.Abstract;

namespace Devly.Database.Repositories.Impl;

internal class CitiesRepository : ICitiesRepository
{
    private readonly IDbRepository<DevlyDbContext> _repository;

    public CitiesRepository(IDbRepository<DevlyDbContext> repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<City>> GetAllCities()
    {
        return await _repository.FindAllAsync<City>(c => true);
    }

    public async Task<IReadOnlyList<City>> GetCitiesByPattern(string pattern)
    {
        var cities = await _repository.FindAllAsync<City>(c => c.Name.Contains(pattern));
        return cities.Take(10).ToArray();
    }

    public async Task<City?> GetCityByName(string cityName)
    {
        return await _repository.FindAsync<City>(x => x.Name.Equals(cityName));
    }
}