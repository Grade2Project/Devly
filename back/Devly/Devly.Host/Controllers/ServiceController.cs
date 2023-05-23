using Devly.Database.Models;
using Devly.Database.Repositories.Abstract;
using Devly.Extensions;
using Devly.Models;
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

    [HttpPost]
    [Route("user/random")]
    public async Task<ResumeDto> GetNextUserRandom()
    {
        var user = await _userRepository.GetRandomUser();
        return user.MapToResumeDto();
    }

    [HttpPost]
    [Route("user")]
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
            
            users = users.Take(10).ToList();
        }

        var userToReturn = users.FirstOrDefault();
        if (userToReturn is null)
            return await GetNextUserRandom();
        _memoryCache.Set(companyEmail, users.Skip(1).ToList(),
            new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));
        return userToReturn.MapToResumeDto();
    }

    [HttpPost]
    [Route("vacancy/random")]
    public async Task<VacancyDto?> GetNextVacancyRandom()
    {
        var vacancy = await _vacancyRepository.GetRandomVacancy();
        return vacancy?.MapToVacancyDto();
    }

    [HttpPost]
    [Route("vacancy")]
    public async Task<VacancyDto?> GetNextVacancy([FromBody] string userLogin)
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

            vacancies = vacancies.Take(10).ToList();
        }

        var vacancyToReturn = vacancies.FirstOrDefault();
        if (vacancyToReturn is null)
            return await GetNextVacancyRandom();
        _memoryCache.Set(userLogin, vacancies.Skip(1).ToList(),
            new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));

        return vacancyToReturn.MapToVacancyDto();
    }

    [HttpPost]
    [Route("user/filter")]
    public async Task<ResumeDto?> GetNextUserFilter([FromBody] UserFilterDto userFilterDto)
    {
        var companyEmail = userFilterDto.CompanyEmail;
        var key = $"{companyEmail}{FilterString}";
        var cachedUsers = _memoryCache.Get<MemoryCacheEntry<User>>(key);
        if (cachedUsers == null || cachedUsers.Entries.Count == 0 || cachedUsers.FilterHash != userFilterDto.GetHashCode())
        {
            var grades = userFilterDto.Grades.Select(x => _gradesRepository.FindGrade(x).Result).ToArray();
            var languages = await _programmingLanguagesRepository
                .FindLanguagesAsync(userFilterDto.Languages);
            if (grades.Any(x => x is null) || languages.Any(x => x is null)) return null;
            var languagesIds = languages.Select(x => x.Id).ToHashSet();

            List<User> filteredUsers;
            try
            {
                filteredUsers = grades.Select(grade => _userRepository.GetUsersByGrade(grade.Id)?.Result)
                    .SelectMany(x => x)
                    .Where(user =>
                        user.FavoriteLanguages.Any(language => languagesIds.Contains(language.ProgrammingLanguageId)))
                    .Take(10).ToList();
            }
            catch(Exception e)
            {
                return await GetNextUserRandom();
            }

            cachedUsers = new MemoryCacheEntry<User>
            {
                Entries = filteredUsers,
                FilterHash = userFilterDto.GetHashCode()
            };
        }

        if (cachedUsers.Entries.Count == 0)
            return null;
        var answer = cachedUsers.Entries[0].MapToResumeDto();
        cachedUsers.Entries = cachedUsers.Entries.Skip(1).ToList();
        _memoryCache.Set(key, cachedUsers,
            new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
        return answer;
    }

    [HttpPost]
    [Route("vacancy/filter")]
    public async Task<VacancyDto?> GetNextVacancyFilter([FromBody] VacancyFilterDto vacancyFilterDto)
    {
        var userLogin = vacancyFilterDto.UserLogin;
        var key = $"{userLogin}{FilterString}";
        var cachedVacancies = _memoryCache.Get<MemoryCacheEntry<Vacancy>>(key);

        if (cachedVacancies == null || cachedVacancies.Entries.Count == 0 ||
            cachedVacancies.FilterHash != vacancyFilterDto.GetHashCode())
        {
            var grades = vacancyFilterDto.Grades.Select(x => _gradesRepository.FindGrade(x).Result).ToArray();
            var languages = await _programmingLanguagesRepository
                .FindLanguagesAsync(vacancyFilterDto.Languages);
            if (grades.Any(x => x is null) || languages.Any(x => x is null)) return null;
            var languagesIds = languages.Select(x => x.Id).ToHashSet();

            List<Vacancy> filteredVacancies;
            try
            {
                filteredVacancies = grades
                    .Select(x => _vacancyRepository.GetAllGradeVacancies(x.Id)?.Result)
                    .SelectMany(x => x)
                    .Where(vacancy => languagesIds.Contains(vacancy.ProgrammingLanguageId) && 
                                      vacancy.Salary >= vacancyFilterDto.SalaryFrom &&
                                      vacancy.Salary <= (vacancyFilterDto.SalaryTo > 0 ? vacancyFilterDto.SalaryTo : int.MaxValue))
                    .Take(10).ToList();
            }
            catch (Exception e)
            {
                return await GetNextVacancyRandom();
            }

            cachedVacancies = new MemoryCacheEntry<Vacancy>
            {
                Entries = filteredVacancies,
                FilterHash = vacancyFilterDto.GetHashCode()
            };
        }

        if (cachedVacancies.Entries.Count == 0)
            return null;
        var answer = cachedVacancies.Entries[0].MapToVacancyDto();
        cachedVacancies.Entries = cachedVacancies.Entries.Skip(1).ToList();
        _memoryCache.Set(key, cachedVacancies,
            new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
        return answer;
    }
}