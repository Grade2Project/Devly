using System.Security.Claims;
using Devly.Database.Models;
using Devly.Database.Repositories.Abstract;
using Devly.Helpers;
using Devly.Models;
using Devly.Services;
using Microsoft.AspNetCore.Mvc;

namespace Devly.Controllers;

public class RegController : Controller
{
    private readonly ICompaniesPasswordsRepository _companiesPasswordsRepository;
    private readonly ICompaniesRepository _companiesRepository;
    private readonly IPasswordHasher _hasher;
    private readonly IIdentityService _identityService;
    private readonly IUserPasswordRepository _passwordRepository;
    private readonly IPhotoHelper _photoHelper;

    public RegController(IUserPasswordRepository passwordRepository,
        IPasswordHasher hasher,
        ICompaniesRepository companiesRepository,
        ICompaniesPasswordsRepository companiesPasswordsRepository,
        IIdentityService identityService,
        IPhotoHelper photoHelper)
    {
        _passwordRepository = passwordRepository;
        _hasher = hasher;
        _companiesRepository = companiesRepository;
        _companiesPasswordsRepository = companiesPasswordsRepository;
        _identityService = identityService;
        _photoHelper = photoHelper;
    }

    [HttpPost]
    [Route("reg")]
    public async Task<IActionResult> Register([FromBody] LoginDto userDto)
    {
        if (await _passwordRepository.FindByUserLoginAsync(userDto.Login) != null) return StatusCode(400);

        await _passwordRepository.InsertAsync(userDto.Login, _hasher.HashPassword(userDto.Password));
        var token = await _identityService.GenerateToken(new TokenRequestDto
        {
            Email = userDto.Login,
            CustomClaims = new Dictionary<string, object>
            {
                [ClaimTypes.Role] = "User"
            }
        });
        return Ok(token);
    }

    [HttpPost]
    [Route("company/reg")]
    public async Task<IActionResult> RegisterCompany([FromBody] CompanyDto companyDto)
    {
        if (await _companiesRepository.GetCompanyByName(companyDto.CompanyName) != null) return StatusCode(400);

        var company = new Company
        {
            CompanyEmail = companyDto.CompanyEmail,
            CompanyName = companyDto.CompanyName,
            Info = companyDto.CompanyInfo,
        };

        string path;
        if (companyDto.Photo is { Length: > 0 })
        {
            path = $"../photos/companies/{Guid.NewGuid()}.txt";
            _photoHelper.Save(companyDto.Photo, path);
        }
        else
        {
            path = "../photos/companies/default.txt";
        }

        company.ImagePath = path;

        await _companiesRepository.InsertAsync(company).ConfigureAwait(false);
        await _companiesPasswordsRepository.InsertAsync(
            company.Id, _hasher.HashPassword(companyDto.Password));
        var token = await _identityService.GenerateToken(new TokenRequestDto
        {
            Email = companyDto.CompanyEmail,
            CustomClaims = new Dictionary<string, object>
            {
                [ClaimTypes.Role] = "Company"
            }
        });
        return Ok(token);
    }
}