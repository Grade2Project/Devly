using Devly.Database.Repositories;
using Devly.Models;
using Devly.Services;
using Microsoft.AspNetCore.Mvc;

namespace Devly.Controllers;

public class RegController : Controller
{
    private readonly IUserPasswordRepository _passwordRepository;
    private readonly IPasswordHasher _hasher;

    public RegController(IUserPasswordRepository passwordRepository,
        IPasswordHasher hasher)
    {
        _passwordRepository = passwordRepository;
        _hasher = hasher;
    }

    [HttpPost, Route("reg")]
    public async Task<IActionResult> Register([FromBody] LoginDto userDto)
    {
        if (await _passwordRepository.FindByUserLoginAsync(userDto.Login) != null)
        {
            return StatusCode(400);
        }

        await _passwordRepository.InsertAsync(userDto.Login, _hasher.HashPassword(userDto.Password));
        
        return Ok();
    }
}