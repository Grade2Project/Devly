using Devly.Database.Repositories;
using Devly.Models;
using Microsoft.AspNetCore.Mvc;

namespace Devly.Controllers;

public class ResumeController : Controller
{
    private readonly IUserRepository _userRepository;

    public ResumeController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

}