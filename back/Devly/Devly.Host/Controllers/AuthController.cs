using System.Security.Claims;
using Devly.Database.Repositories.Abstract;
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
    private readonly IIdentityService _identityService;
    private readonly ICompaniesPasswordsRepository _companiesPasswordsRepository;
    private readonly ICompaniesRepository _companiesRepository;

    public AuthController(IUserPasswordRepository userPasswordRepository,
        IUserRepository userRepository,
        IPasswordHasher hasher,
        IIdentityService identityService,
        ICompaniesPasswordsRepository companiesPasswordsRepository,
        ICompaniesRepository companiesRepository)
    {
        _userPasswordRepository = userPasswordRepository;
        _userRepository = userRepository;
        _hasher = hasher;
        _identityService = identityService;
        _companiesPasswordsRepository = companiesPasswordsRepository;
        _companiesRepository = companiesRepository;
    }

    [HttpPost]
    [Route("user")]
    public async Task<IActionResult> AuthUser([FromBody] LoginDto dto)
    {
        if (await AuthUserInternal(dto))
        {
            var token = await _identityService.GenerateToken(new TokenRequestDto
            {
                Email = dto.Login,
                CustomClaims = new Dictionary<string, object>
                {
                    [ClaimTypes.Role] = "User"
                }
            });
            
            return Ok(token);
        }
        return StatusCode(401);
    }

    [HttpPost]
    [Route("company")]
    public async Task<IActionResult> AuthCompany([FromBody] LoginDto dto)
    {
        if (await AuthCompanyInternal(dto))
        {
            var token = await _identityService.GenerateToken(new TokenRequestDto
            {
                Email = dto.Login,
                CustomClaims = new Dictionary<string, object>
                {
                    [ClaimTypes.Role] = "Company"
                }
            });
            
            return Ok(token);
        }

        return StatusCode(401);
    }

    private async Task<bool> AuthUserInternal(LoginDto dto)
    {
        var hashed = _hasher.HashPassword(dto.Password);
        var dbUserPassword = await _userPasswordRepository.FindByUserLoginAsync(dto.Login).ConfigureAwait(false);
        return dbUserPassword != null && dbUserPassword.HashedPass == hashed;
    }

    private async Task<bool> AuthCompanyInternal(LoginDto dto)
    {
        var hashed = _hasher.HashPassword(dto.Password);
        var company = await _companiesRepository.GetCompanyByEmail(dto.Login);
        if (company is null) return false;
        
        var dbCompanyPassword = await _companiesPasswordsRepository
            .GetPasswordById(company.Id);
        return hashed == dbCompanyPassword;
    }
}