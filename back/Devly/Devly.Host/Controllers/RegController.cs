using Devly.Database.Models;
using Devly.Database.Repositories;
using Devly.Models;
using Devly.Services;
using Microsoft.AspNetCore.Mvc;

namespace Devly.Controllers;

public class RegController : Controller
{
    private readonly IUserPasswordRepository _passwordRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _hasher;

    public RegController(IUserPasswordRepository passwordRepository,
        IUserRepository userRepository,
        IPasswordHasher hasher)
    {
        _passwordRepository = passwordRepository;
        _userRepository = userRepository;
        _hasher = hasher;
    }

    [HttpPost, Route("reg")]
    public async Task<IActionResult> Register([FromBody] LoginDto userDto)
    {
        if (await _passwordRepository.FindByUserLoginAsync(userDto.Login) != null)
        {
            return StatusCode(400);
        }

        await RegisterInternalAsync(userDto);
        return Ok();
    }

    private async Task RegisterInternalAsync(LoginDto userDto)
    {
        var user = new User
        {
            Login = userDto.Login
        };
        await _userRepository.InsertAsync(user);
        await _passwordRepository.InsertAsync(user!.Login, _hasher.HashPassword(userDto.Password));
    }
}