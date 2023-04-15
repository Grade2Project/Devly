using Devly.Configs;
using Devly.Database.Repositories;
using Devly.Models;
using Devly.Services;
using Microsoft.AspNetCore.Mvc;

namespace Devly.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IUserPasswordRepository _userPasswordRepository;
    private readonly IPasswordHasher _hasher;

    public AuthController(IUserPasswordRepository userPasswordRepository, 
        IUserRepository userRepository,
        IPasswordHasher hasher)
    {
        _userPasswordRepository = userPasswordRepository;
        _userRepository = userRepository;
        _hasher = hasher;
    }

    [HttpPost, Route("user")]
    public async Task<IActionResult> AuthUser([FromBody] LoginDto dto)
    {
        if (await AuthInternal(dto))
        {
            var user = await _userRepository.FindUserByLoginAsync(dto.Login);
            return Ok(user);
        }

        return StatusCode(401);
    }

    private async Task<bool> AuthInternal(LoginDto dto)
    {
        var hashed = _hasher.HashPassword(dto.Password);
        var dbUserPassword = await _userPasswordRepository.FindByUserLoginAsync(dto.Login).ConfigureAwait(false);
        return dbUserPassword != null && dbUserPassword.HashedPass == hashed;
    }
}