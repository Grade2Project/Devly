using Devly.Database.Repositories;
using Devly.Extensions;
using Devly.Models;
using Microsoft.AspNetCore.Mvc;

namespace Devly.Controllers;

public class ServiceController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IVacancyRepository _vacancyRepository;

    public ServiceController(IUserRepository userRepository,
        IVacancyRepository vacancyRepository)
    {
        _userRepository = userRepository;
        _vacancyRepository = vacancyRepository;
    }
    
    [HttpPost, Route("next/user")]
    public async Task<ResumeDto> GetNextUser()
    {
        var user = await _userRepository.GetRandomUser();
        return user.MapToResumeDto();
    }
    
    [HttpPost, Route("next/vacancy")]
    public async Task<VacancyDto?> GetNextVacancy()
    {
        var vacancy = await _vacancyRepository.GetRandomVacancy();
        return vacancy?.MapToVacancyDto();
    }
}