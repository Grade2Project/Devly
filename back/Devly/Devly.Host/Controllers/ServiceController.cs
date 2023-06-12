using Devly.Database.Filters;
using Devly.Database.Models;
using Devly.Database.Repositories.Abstract;
using Devly.Extensions;
using Devly.Helpers;
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
    private readonly IVacancyRepository _vacancyRepository;
    private readonly ICompaniesRepository _companies;
    private readonly IPhotoHelper _photoHelper;

    public ServiceController(IUserRepository userRepository,
        IVacancyRepository vacancyRepository,
        IMemoryCache memoryCache,
        IGradesRepository gradesRepository,
        IProgrammingLanguagesRepository programmingLanguagesRepository,
        IPhotoHelper photoHelper,
        ICompaniesRepository companies)
    {
        _userRepository = userRepository;
        _vacancyRepository = vacancyRepository;
        _memoryCache = memoryCache;
        _gradesRepository = gradesRepository;
        _programmingLanguagesRepository = programmingLanguagesRepository;
        _photoHelper = photoHelper;
        _companies = companies;
    }
    
    [Authorize(Policy = "CompanyPolicy")]
    [HttpGet, Route("user/random")]
    public async Task<ResumeDto?> GetNextUserRandom()
    {
        var user = await _userRepository.GetRandomUser();
        return await ToResumeDto(user);
    }
    
    
    [Authorize(Policy = "UserPolicy")]
    [HttpGet, Route("vacancy/random")]
    public async Task<VacancyDto?> GetNextVacancyRandom()
    {
        var vacancy = await _vacancyRepository.GetRandomVacancy();
        return await ToVacancyDto(vacancy);
    }

    [HttpGet]
    [Route("user/find")]
    public async Task<ResumeDto?> GetResumeByLogin(string login)
    {
        var user = await _userRepository.FindUserByLoginAsync(login);
        return await ToResumeDto(user);
    }
    
    [HttpGet]
    [Route("vacancy/find")]
    public async Task<VacancyDto?> GetVacancyById(int id)
    {
        var vacancy = await _vacancyRepository.FindVacancyByIdAsync(id);
        return await ToVacancyDto(vacancy);
    }

    [HttpPost]
    [Authorize(Policy = "CompanyPolicy")]
    [Route("user")]
    public async Task<ResumeDto?> GetNextUserFilter([FromBody] UserFilterDto? userFilterDto)
    {
        var companyEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email")!.Value;
        if (userFilterDto is null)
            return await GetNextUser(companyEmail);
        var key = $"company_{companyEmail}{FilterString}";
        lock (_memoryCache)
        {
            var cachedUsers = _memoryCache.Get<MemoryCacheEntry<User>>(key);
            if (cachedUsers == null || cachedUsers.IsEnded || cachedUsers.Entries.Count == 0 ||
                cachedUsers.FilterHash != userFilterDto.GetHashCode())
            {
                var grades = userFilterDto.Grades is null
                    ? _gradesRepository.GetAllGrades().Result
                    : userFilterDto.Grades.Select(x => _gradesRepository.FindGrade(x).Result);
                var languages = userFilterDto.Languages is null
                    ? _programmingLanguagesRepository.GetAllLanguages().Result
                    : _programmingLanguagesRepository.FindLanguagesAsync(userFilterDto.Languages).Result;


                var filteredUsers = _userRepository.GetAllUsersFilter(new UserFilter
                {
                    LanguageIds = languages.Select(x => x.Id).ToArray(),
                    GradeIds = grades.Select(x => x.Id).ToArray(),
                    ExperienceFrom = userFilterDto.ExperienceFrom,
                    City = userFilterDto.City,
                    UserName = userFilterDto.UserName
                })
                    .Result;

                cachedUsers = new MemoryCacheEntry<User>(filteredUsers, userFilterDto.GetHashCode());
            }

            if (cachedUsers.Entries.Count == 0)
                return null;
            var answer = cachedUsers.Next();
            _memoryCache.Set(key, cachedUsers,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            return ToResumeDto(answer).Result;
        }
    }

    [HttpPost]
    [Authorize(Policy = "UserPolicy")]
    [Route("vacancy")]
    public async Task<VacancyDto?> GetNextVacancyFilter([FromBody] VacancyFilterDto? vacancyFilterDto)
    {
        var userLogin = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email")!.Value;
        if (vacancyFilterDto is null)
            return await GetNextVacancy(userLogin);
        var key = $"user_{userLogin}{FilterString}";
        
        lock (_memoryCache)
        {
            var cachedVacancies = _memoryCache.Get<MemoryCacheEntry<Vacancy>>(key);

            if (cachedVacancies == null || cachedVacancies.Entries.Count == 0 ||
                cachedVacancies.FilterHash != vacancyFilterDto.GetHashCode() || cachedVacancies.IsEnded)
            {
                var grades = vacancyFilterDto.Grades is null
                    ? _gradesRepository.GetAllGrades().Result.Select(x => x.Id).ToArray()
                    : vacancyFilterDto.Grades.Select(x => _gradesRepository.FindGrade(x).Result.Id).ToArray();
                var languages = vacancyFilterDto.Languages is null
                    ? _programmingLanguagesRepository.GetAllLanguages().Result.Select(x => x.Id).ToArray()
                    : _programmingLanguagesRepository.FindLanguagesAsync
                        (vacancyFilterDto.Languages).Result.Select(x => x.Id).ToArray();
                
                var filteredVacancies = _vacancyRepository.GetAllVacanciesFilter(new VacancyFilter
                    {
                        GradeIds = grades,
                        LanguageIds = languages,
                        SalaryFrom = vacancyFilterDto.SalaryFrom,
                        SalaryTo = vacancyFilterDto.SalaryTo,
                        CompanyName = vacancyFilterDto.CompanyName
                    })
                    ?.Result.ToArray();
                
                cachedVacancies = new MemoryCacheEntry<Vacancy>(filteredVacancies!, vacancyFilterDto.GetHashCode());
            }
            
            var answer = cachedVacancies.Next();
            if (answer is null)
                return null;
            _memoryCache.Set(key, cachedVacancies,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            return ToVacancyDto(answer).Result;
        }
    }
    
    private async Task<ResumeDto?> GetNextUser(string companyEmail)
    {
        lock (_memoryCache)
        {
            var cachedUsers = _memoryCache.Get<MemoryCacheEntry<User>>(companyEmail);
            if (cachedUsers == null || cachedUsers.IsEnded || cachedUsers.Entries.Count == 0)
            {
                var company = _companies.GetCompanyByEmail(companyEmail).Result;
                var companyVacancies = company.Vacancies;
                var alreadyLiked = company.FavoriteUsers.Select(x => x.UserLogin).ToArray();
                var users = _userRepository.GetAllUsersFilter(new UserFilter
                {
                    GradeIds = companyVacancies.Select(x => x.GradeId).ToArray(),
                    LanguageIds = companyVacancies.Select(x => x.ProgrammingLanguageId).ToArray(),
                }, alreadyLiked)?.Result;
                
                if (users == null)
                    return GetNextUserRandom().Result;
                cachedUsers = new MemoryCacheEntry<User>(users);
            }

            var userToReturn = cachedUsers.Next();
            if (userToReturn is null)
                return GetNextUserRandom().Result;
            _memoryCache.Set(companyEmail, cachedUsers,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));
            return ToResumeDto(userToReturn).Result;
        }
    }
    
    private async Task<VacancyDto?> GetNextVacancy(string userLogin)
    {
        lock (_memoryCache)
        {
            var cachedVacancies = _memoryCache.Get<MemoryCacheEntry<Vacancy>>(userLogin);
            if (cachedVacancies == null || cachedVacancies.IsEnded || cachedVacancies.Entries.Count == 0)
            {
                var user = _userRepository.FindUserByLoginAsync(userLogin).Result!;
                var alreadyLiked = user.FavoriteVacancies.Select(x => x.VacancyId).ToArray();
                var userLanguages = user.FavoriteLanguages.Select(x => x.ProgrammingLanguageId);
                var vacancies = _vacancyRepository.GetAllVacanciesFilter(new VacancyFilter
                {
                    GradeIds = new[] { user.GradeId },
                    LanguageIds = userLanguages.ToArray()
                }, alreadyLiked)
                    ?.Result;
                if (vacancies is null)
                    return GetNextVacancyRandom().Result;
                cachedVacancies = new MemoryCacheEntry<Vacancy>(vacancies);
            }

            var vacancyToReturn = cachedVacancies.Next();
            if (vacancyToReturn is null)
                return GetNextVacancyRandom().Result;
            _memoryCache.Set(userLogin, cachedVacancies,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));

            return ToVacancyDto(vacancyToReturn).Result;
        }
    }

    private async Task<ResumeDto?> ToResumeDto(User? user)
    {
        if (user is null)
            return null;
        var photo = await _photoHelper.LoadFrom(user.ImagePath ?? "../photos/users/default.txt");
        //Временно, чтобы не менять тестовые данные
        return user.MapToResumeDto(photo);
    }

    private async Task<VacancyDto?> ToVacancyDto(Vacancy? vacancy)
    {
        if (vacancy is null)
            return null;
        var path = vacancy.Company.ImagePath ?? "../photos/companies/default.txt";
        var photo = await _photoHelper.LoadFrom(path);
        return vacancy.MapToVacancyDto(photo);
    }
}