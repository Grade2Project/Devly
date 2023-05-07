using Devly.Database.Repositories;
using Devly.Models;
using Devly.Services;
using Microsoft.AspNetCore.Mvc;

namespace Devly.Controllers;

public class RegController : Controller
{
    private readonly IUserPasswordRepository _passwordRepository;
    private readonly ICompaniesRepository _companiesRepository;
    private readonly IPasswordHasher _hasher;

    public RegController(IUserPasswordRepository passwordRepository,
        IPasswordHasher hasher,
        ICompaniesRepository companiesRepository)
    {
        _passwordRepository = passwordRepository;
        _hasher = hasher;
        _companiesRepository = companiesRepository;
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

    [HttpPost, Route("company/reg")]
    public async Task<IActionResult> RegisterCompany([FromBody] CompanyDto companyDto)
    {
        if (await _companiesRepository.GetCompanyByName(companyDto.CompanyName) != null)
        {
            return StatusCode(400);
        }

        await _companiesRepository.InsertAsync(companyDto.CompanyName, companyDto.CompanyInfo);
        return Ok();
    }
}