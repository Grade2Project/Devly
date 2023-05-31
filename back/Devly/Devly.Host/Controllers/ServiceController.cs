using Devly.Database.Models;
using Devly.Database.Repositories.Abstract;
using Devly.Extensions;
using Devly.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Devly.Controllers;

[Route("next")]
public class ServiceController : Controller
{
    private const string FilterString = "filter";
    private readonly IGradesRepository _gradesRepository;
    private readonly IMemoryCache _memoryCache;
    private readonly IProgrammingLanguagesRepository _programmingLanguagesRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUsersFavoriteLanguagesRepository _usersFavoriteLanguagesRepository;
    private readonly IVacancyRepository _vacancyRepository;

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
    
    [Authorize(Policy = "CompanyPolicy")]
    [HttpGet, Route("user/random")]
    public async Task<ResumeDto> GetNextUserRandom()
    {
        var user = await _userRepository.GetRandomUser();
        return user.MapToResumeDto();
    }
    
    
    [Authorize(Policy = "UserPolicy")]
    [HttpGet, Route("vacancy/random")]
    public async Task<VacancyDto?> GetNextVacancyRandom()
    {
        var vacancy = await _vacancyRepository.GetRandomVacancy();
        return vacancy?.MapToVacancyDto();
    }

    [HttpPost]
    [Authorize(Policy = "CompanyPolicy")]
    [Route("user")]
    public async Task<ResumeDto?> GetNextUserFilter([FromBody] UserFilterDto? userFilterDto)
    {
        var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email")!.Value;
        if (userFilterDto is null)
            return await GetNextUser(email);
        userFilterDto.CompanyEmail = email;
        var companyEmail = userFilterDto.CompanyEmail;
        var key = $"company_{companyEmail}{FilterString}";
        lock (_memoryCache)
        {
            var cachedUsers = _memoryCache.Get<MemoryCacheEntry<User>>(key);
            if (cachedUsers == null || cachedUsers.IsEnded || cachedUsers.Entries.Count == 0 ||
                cachedUsers.FilterHash != userFilterDto.GetHashCode())
            {
                var grades = userFilterDto.Grades is null
                    ? _gradesRepository.GetAllGrades().Result.ToArray()
                    : userFilterDto.Grades.Select(x => _gradesRepository.FindGrade(x).Result).ToArray();
                var languages = userFilterDto.Languages is null
                    ? _programmingLanguagesRepository.GetAllLanguages().Result.ToArray()
                    : _programmingLanguagesRepository.FindLanguagesAsync(userFilterDto.Languages).Result;
                if (grades.Any(x => x is null) || languages.Any(x => x is null)) return null;
                var languagesIds = languages.Select(x => x.Id).ToHashSet();

                List<User> filteredUsers;
                try
                {
                    filteredUsers = grades.Select(grade => _userRepository.GetUsersEqGrade(grade.Id)?.Result)
                        .SelectMany(x => x)
                        .DistinctBy(x => x.Login)
                        .Where(user =>
                            user.FavoriteLanguages.Any(
                                language => languagesIds.Contains(language.ProgrammingLanguageId)))
                        .ToList();
                    // потом надо сделать Take(n)
                }
                catch (Exception e)
                {
                    return GetNextUserRandom().Result;
                }

                cachedUsers = new MemoryCacheEntry<User>(filteredUsers, userFilterDto.GetHashCode());
            }

            if (cachedUsers.Entries.Count == 0)
                return null;
            var answer = cachedUsers.Next();
            _memoryCache.Set(key, cachedUsers,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            return answer.MapToResumeDto();
        }
    }

    [HttpPost]
    [Authorize(Policy = "UserPolicy")]
    [Route("vacancy")]
    public async Task<VacancyDto?> GetNextVacancyFilter([FromBody] VacancyFilterDto? vacancyFilterDto)
    {
        var login = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email")!.Value;
        if (vacancyFilterDto is null)
            return await GetNextVacancy(login);
        vacancyFilterDto.UserLogin = login;
        var userLogin = login;
        var key = $"user_{userLogin}{FilterString}";
        lock (_memoryCache)
        {
            var cachedVacancies = _memoryCache.Get<MemoryCacheEntry<Vacancy>>(key);

            if (cachedVacancies == null || cachedVacancies.Entries.Count == 0 ||
                cachedVacancies.FilterHash != vacancyFilterDto.GetHashCode())
            {
                var grades = vacancyFilterDto.Grades is null
                    ? _gradesRepository.GetAllGrades().Result.ToArray()
                    : vacancyFilterDto.Grades.Select(x => _gradesRepository.FindGrade(x).Result).ToArray();
                var languages = vacancyFilterDto.Languages is null
                    ? _programmingLanguagesRepository.GetAllLanguages().Result.ToArray()
                    : _programmingLanguagesRepository.FindLanguagesAsync(vacancyFilterDto.Languages).Result;
                var languagesIds = languages.Select(x => x.Id).ToHashSet();

                List<Vacancy> filteredVacancies;
                try
                {
                    filteredVacancies = grades
                        .Select(x => _vacancyRepository.GetAllGradeVacancies(x.Id)?.Result)
                        .SelectMany(x => x)
                        .DistinctBy(x => x.Id)
                        .Where(vacancy => languagesIds.Contains(vacancy.ProgrammingLanguageId) &&
                                          vacancy.Salary >= vacancyFilterDto.SalaryFrom &&
                                          vacancy.Salary <= (vacancyFilterDto.SalaryTo > 0
                                              ? vacancyFilterDto.SalaryTo
                                              : int.MaxValue))
                        .ToList();
                }
                catch (Exception e)
                {
                    return GetNextVacancyRandom().Result;
                }

                cachedVacancies = new MemoryCacheEntry<Vacancy>(filteredVacancies, vacancyFilterDto.GetHashCode());
            }

            if (cachedVacancies.Entries.Count == 0)
                return null;
            var answer = cachedVacancies.Next();
            _memoryCache.Set(key, cachedVacancies,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            return answer.MapToVacancyDto();
        }
    }
    
    private async Task<ResumeDto>? GetNextUser(string companyEmail)
    {
        lock (_memoryCache)
        {
            var cachedUsers = _memoryCache.Get<MemoryCacheEntry<User>>(companyEmail);
            if (cachedUsers == null || cachedUsers.IsEnded || cachedUsers.Entries.Count == 0)
            {
                var users = new List<User>();
                var companyVacancies = _vacancyRepository.GetAllCompanyVacancies(companyEmail).Result;
                foreach (var vacancy in companyVacancies)
                {
                    var usersOfVacancyGrade = _userRepository.GetUsersEqGrade(vacancy.Grade.Id).Result;
                    if (usersOfVacancyGrade == null)
                        continue;
                    users.AddRange(usersOfVacancyGrade.Where(user =>
                    {
                        var userFavoriteLanguages = user.FavoriteLanguages
                            .Select(x => x.ProgrammingLanguage.LanguageName)
                            .ToArray();
                        return userFavoriteLanguages.Contains(vacancy.ProgrammingLanguage.LanguageName);
                    }));
                }

                users = users.DistinctBy(x => x.Login).ToList();
                cachedUsers = new MemoryCacheEntry<User>(users, 0);
            }

            var userToReturn = cachedUsers.Next();
            if (userToReturn is null)
                return GetNextUserRandom().Result;
            _memoryCache.Set(companyEmail, cachedUsers,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));
            return userToReturn.MapToResumeDto();
        }
    }
    
    private async Task<VacancyDto?> GetNextVacancy(string userLogin)
    {
        lock (_memoryCache)
        {
            var cachedVacancies = _memoryCache.Get<MemoryCacheEntry<Vacancy>>(userLogin);
            if (cachedVacancies == null || cachedVacancies.IsEnded || cachedVacancies.Entries.Count == 0)
            {
                var vacancies = new List<Vacancy>();
                var user = _userRepository.FindUserByLoginAsync(userLogin).Result;
                var userLanguages = user.FavoriteLanguages;
                foreach (var language in userLanguages)
                {
                    var vacanciesOfLanguage =
                        _vacancyRepository.GetAllLanguageVacancies(language.ProgrammingLanguage.LanguageName).Result;
                    if (vacanciesOfLanguage == null)
                        continue;
                    vacancies.AddRange(vacanciesOfLanguage.Where(vac => vac.Grade.Id <= user.GradeId));
                }

                vacancies = vacancies.DistinctBy(x => x.Id).ToList();
                cachedVacancies = new MemoryCacheEntry<Vacancy>(vacancies, 0);
            }

            var vacancyToReturn = cachedVacancies.Next();
            if (vacancyToReturn is null)
                return GetNextVacancyRandom().Result;
            _memoryCache.Set(userLogin, cachedVacancies,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));

            return vacancyToReturn.MapToVacancyDto();
        }
    }
}