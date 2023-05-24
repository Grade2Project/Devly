using Devly.Database.Repositories.Abstract;
using Devly.Models;
using Devly.Services;
using Microsoft.AspNetCore.Mvc;

namespace Devly.Controllers;

public class RegController : Controller
{
    private readonly ICompaniesPasswordsRepository _companiesPasswordsRepository;
    private readonly ICompaniesRepository _companiesRepository;
    private readonly IPasswordHasher _hasher;
    private readonly IUserPasswordRepository _passwordRepository;

    public RegController(IUserPasswordRepository passwordRepository,
        IPasswordHasher hasher,
        ICompaniesRepository companiesRepository, ICompaniesPasswordsRepository companiesPasswordsRepository)
    {
        _passwordRepository = passwordRepository;
        _hasher = hasher;
        _companiesRepository = companiesRepository;
        _companiesPasswordsRepository = companiesPasswordsRepository;
    }

    [HttpPost]
    [Route("reg")]
    public async Task<IActionResult> Register([FromBody] LoginDto userDto)
    {
        if (await _passwordRepository.FindByUserLoginAsync(userDto.Login) != null) return StatusCode(400);

        await _passwordRepository.InsertAsync(userDto.Login, _hasher.HashPassword(userDto.Password));

        return Ok();
    }

    [HttpPost]
    [Route("company/reg")]
    public async Task<IActionResult> RegisterCompany([FromBody] CompanyDto companyDto)
    {
        if (await _companiesRepository.GetCompanyByName(companyDto.CompanyName) != null) return StatusCode(400);

        var company = await _companiesRepository.InsertAsync
            (companyDto.CompanyName, companyDto.CompanyEmail, companyDto.CompanyInfo).ConfigureAwait(false);
        await _companiesPasswordsRepository.InsertAsync(
            company.Id, _hasher.HashPassword(companyDto.Password));
        return Ok();
    }
}