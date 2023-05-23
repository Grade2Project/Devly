using Devly.Database.Models;
using Devly.Database.Repositories;
using Devly.Database.Repositories.Abstract;
using Devly.Extensions;
using Devly.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Devly.Controllers;

[Route("next")]
public class ServiceController : Controller
{
    private readonly IMemoryCache _memoryCache;
    private readonly IUserRepository _userRepository;
    private readonly IVacancyRepository _vacancyRepository;
    private readonly IUsersFavoriteLanguagesRepository _usersFavoriteLanguagesRepository;
    private readonly IGradesRepository _gradesRepository;
    private readonly IProgrammingLanguagesRepository _programmingLanguagesRepository;

    public ServiceController(IUserRepository userRepository,
        IVacancyRepository vacancyRepository,
        IUsersFavoriteLanguagesRepository usersFavoriteLanguagesRepository,
        IMemoryCache memoryCache,
        IGradesRepository gradesRepository,
        IProgrammingLanguagesRepository programmingLanguagesRepository)
    {
        _userRepository = userRepository;
        _vacancyRepository = vacancyRepository;
        _usersFavoriteLanguagesRepository = usersFavoriteLanguagesRepository;
        _memoryCache = memoryCache;
        _gradesRepository = gradesRepository;
        _programmingLanguagesRepository = programmingLanguagesRepository;
    }
    
    [HttpPost, Route("user/random")]
    public async Task<ResumeDto> GetNextUserRandom()
    {
        var user = await _userRepository.GetRandomUser();
        var languages = await _usersFavoriteLanguagesRepository.GetUserFavoriteLanguages(user.Login);
        return user.MapToResumeDto(languages.Select(x => x.ProgrammingLanguage.LanguageName));
    }

    [HttpPost, Route("user")]
    public async Task<ResumeDto>? GetNextUser([FromBody] string companyEmail)
    {
        var users = _memoryCache.Get<IReadOnlyList<User>>(companyEmail);
        if (users == null || users.Count == 0)
        {
            users = new List<User>();
            var usersList = users as List<User>;
            var companyVacancies = await _vacancyRepository.GetAllCompanyVacancies(companyEmail);
            foreach (var vacancy in companyVacancies)
            {
                var usersOfVacancyGrade = await _userRepository.GetUsersByGrade(vacancy.Grade.Id)!;
                if (usersOfVacancyGrade == null)
                    continue;
                usersList?.AddRange(usersOfVacancyGrade.Where(user =>
                {
                    var userFavoriteLanguages = _usersFavoriteLanguagesRepository
                        .GetUserFavoriteLanguages(user.Login).Result.Select(x => x.ProgrammingLanguage.LanguageName);
                    return userFavoriteLanguages.Contains(vacancy.ProgrammingLanguage.LanguageName);
                }));
            }
        }

        var userToReturn = users.FirstOrDefault();
        if (userToReturn is null)
            return await GetNextUserRandom();
        _memoryCache.Set(companyEmail, users.Skip(1).ToList(),
            new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));
        var languages = await _usersFavoriteLanguagesRepository
            .GetUserFavoriteLanguages(userToReturn.Login);
        return userToReturn.MapToResumeDto(languages.Select(x => x.ProgrammingLanguage.LanguageName));
    }

    [HttpPost, Route("vacancy/random")]
    public async Task<VacancyDto?> GetNextVacancyRandom()
    {
        var vacancy = await _vacancyRepository.GetRandomVacancy();
        return vacancy?.MapToVacancyDto();
    }
    
    [HttpPost, Route("vacancy")]
    public async Task<VacancyDto?> GetNextVacancy([FromBody]string userLogin)
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
        
        var vacancyToReturn = vacancies.FirstOrDefault();
        if (vacancyToReturn is null)
            return await GetNextVacancyRandom();
        _memoryCache.Set(userLogin, vacancies.Skip(1).ToList(),
            new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1))); 

        return vacancyToReturn.MapToVacancyDto();
    }

    [HttpPost, Route("user/filter")]
    public async Task<ResumeDto?> GetNextUserFilter([FromBody] FilterDto filterDto)
    {
        var grades = filterDto.Grades.Select(x => _gradesRepository.FindGrade(x).Result).ToArray();
        var languages = await _programmingLanguagesRepository
            .FindLanguagesAsync(filterDto.Languages);
        if (grades.Any(x => x is null) || languages.Any(x => x is null))
        {
            return null;
        }

        var users = grades.Select(x => _userRepository.GetUsersByGrade(x.Id).Result)
            .SelectMany(x => x)
            .Where(user => _usersFavoriteLanguagesRepository.GetUserFavoriteLanguages(user.Login)
                .Result.IntersectBy(languages.Select
                    (x => x.Id), language => language.ProgrammingLanguageId) != null).Take(10).ToArray();
        return null;
    }
}