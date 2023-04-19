using Devly.Database.Repositories;
using Devly.Extensions;
using Devly.Models;
using Microsoft.AspNetCore.Mvc;

namespace Devly.Controllers;

public class ServiceController : Controller
{
    private readonly IUserRepository _userRepository;

    public ServiceController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    [HttpPost, Route("next")]
    public async Task<ResumeDto> GetNextUser()
    {
        var user = await _userRepository.GetRandomUser();
        return user.MapToResumeDto();
    }
}