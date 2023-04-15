using System.Security.Cryptography;
using System.Text;
using Devly.Configs;
using Devly.Database.Repositories;
using Devly.Models;
using Microsoft.AspNetCore.Mvc;

namespace Devly.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IUserPasswordRepository _userPasswordRepository;
    private readonly AuthConfig _config; 

    public AuthController(IUserPasswordRepository userPasswordRepository,
        AuthConfig config, 
        IUserRepository userRepository)
    {
        _userPasswordRepository = userPasswordRepository;
        _config = config;
        _userRepository = userRepository;
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
        var hashed = HashPassword(dto.Password);
        var dbUserPassword = await _userPasswordRepository.FindByUserLoginAsync(dto.Login).ConfigureAwait(false);
        return dbUserPassword != null && dbUserPassword.HashedPass == hashed;
    }

    private string HashPassword(string password)
    {
        password += _config.Salt;
        using var sha256 = new SHA256Managed();
        return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", "");
    }
}