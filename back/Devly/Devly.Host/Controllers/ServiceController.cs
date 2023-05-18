using Devly.Database.Models;
using Devly.Database.Repositories;
using Devly.Database.Repositories.Abstract;
using Devly.Extensions;
using Devly.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Devly.Controllers;

public class ServiceController : Controller
{
    private readonly ICompaniesRepository _companiesRepository;
    private readonly IMemoryCache _memoryCache;
    private readonly IUserRepository _userRepository;
    private readonly IVacancyRepository _vacancyRepository;
    private readonly IUsersFavoriteLanguagesRepository _usersFavoriteLanguagesRepository;

    public ServiceController(IUserRepository userRepository,
        IVacancyRepository vacancyRepository,
        IUsersFavoriteLanguagesRepository usersFavoriteLanguagesRepository,
        ICompaniesRepository companiesRepository, IMemoryCache memoryCache)
    {
        _userRepository = userRepository;
        _vacancyRepository = vacancyRepository;
        _usersFavoriteLanguagesRepository = usersFavoriteLanguagesRepository;
        _companiesRepository = companiesRepository;
        _memoryCache = memoryCache;
    }
    
    [HttpPost, Route("next/user/random")]
    public async Task<ResumeDto> GetNextUserRandom()
    {
        var user = await _userRepository.GetRandomUser();
        var languages = await _usersFavoriteLanguagesRepository.GetUserFavoriteLanguages(user.Login);
        return user.MapToResumeDto(languages.Select(x => x.ProgrammingLanguage.LanguageName));
    }

    [HttpPost, Route("next/user")]
    public async Task<ResumeDto>? GetNextUser([FromBody] string companyEmail)
    {
        var vacancies = await _vacancyRepository.GetAllCompanyVacancies(companyEmail);
        foreach (var vacancy in vacancies)
        {
        }

        return null;
    }

    [HttpPost, Route("next/vacancy/random")]
    public async Task<VacancyDto?> GetNextVacancyRandom()
    {
        var vacancy = await _vacancyRepository.GetRandomVacancy();
        return vacancy?.MapToVacancyDto();
    }
    
    [HttpPost, Route("next/vacancy")]
    public async Task<VacancyDto?> GetNextVacancy(string userLogin)
    {
        var vacancies = _memoryCache.Get<IReadOnlyList<Vacancy>>(userLogin);
        if (vacancies == null || vacancies.Count == 0)
        {
            vacancies = new List<Vacancy>();
            var listVacancies = vacancies as List<Vacancy>;
            var user = await _userRepository.FindUserByLoginAsync(userLogin);
            var userLanguages = await _usersFavoriteLanguagesRepository
                .GetUserFavoriteLanguages(userLogin);
            foreach (var language in userLanguages)
            {
                var vacanciesOfLanguage =
                    await _vacancyRepository.GetAllLanguageVacancies(language.ProgrammingLanguage.LanguageName)!;
                if (vacanciesOfLanguage == null)
                    continue;
                listVacancies?.AddRange(vacanciesOfLanguage.Where(vac => vac.Grade.Id <= user.GradeId));
            }
        }
        
        var vacancyToReturn = vacancies.First();
        _memoryCache.Set(userLogin, vacancies.Skip(1).ToList(),
            new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1))); 

        return vacancyToReturn.MapToVacancyDto();
    }
}