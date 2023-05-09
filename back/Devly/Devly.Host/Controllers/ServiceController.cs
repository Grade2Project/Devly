using Devly.Database.Repositories;
using Devly.Database.Repositories.Abstract;
using Devly.Extensions;
using Devly.Models;
using Microsoft.AspNetCore.Mvc;

namespace Devly.Controllers;

public class ServiceController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IVacancyRepository _vacancyRepository;
    private readonly IUsersFavoriteLanguagesRepository _usersFavoriteLanguagesRepository;

    public ServiceController(IUserRepository userRepository,
        IVacancyRepository vacancyRepository,
        IUsersFavoriteLanguagesRepository usersFavoriteLanguagesRepository)
    {
        _userRepository = userRepository;
        _vacancyRepository = vacancyRepository;
        _usersFavoriteLanguagesRepository = usersFavoriteLanguagesRepository;
    }
    
    [HttpPost, Route("next/user")]
    public async Task<ResumeDto> GetNextUser()
    {
        var user = await _userRepository.GetRandomUser();
        var languages = await _usersFavoriteLanguagesRepository.GetUserFavoriteLanguages(user.Login);
        return user.MapToResumeDto(languages.Select(x => x.ProgrammingLanguage.LanguageName));
    }
    
    [HttpPost, Route("next/vacancy")]
    public async Task<VacancyDto?> GetNextVacancy()
    {
        var vacancy = await _vacancyRepository.GetRandomVacancy();
        return vacancy?.MapToVacancyDto();
    }
}