using Devly.Database.Models;
using Devly.Database.Repositories.Abstract;
using Devly.Extensions;
using Devly.Helpers;
using Microsoft.AspNetCore.Mvc;
using Devly.Models;
using Microsoft.AspNetCore.Authorization;


namespace Devly.Controllers;

[Route("aboutMe")]
public class AboutMeController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly ICompaniesRepository _companiesRepository;
    private readonly IVacancyRepository _vacancyRepository;
    private readonly IPhotoHelper _photoHelper;

    public AboutMeController(ICompaniesRepository companiesRepository, IUserRepository userRepository, IPhotoHelper photoHelper, IVacancyRepository vacancyRepository)
    {
        _companiesRepository = companiesRepository;
        _userRepository = userRepository;
        _photoHelper = photoHelper;
        _vacancyRepository = vacancyRepository;
    }
    
    [Authorize(Policy = "UserPolicy")]
    [HttpGet, Route("user")]
    public async Task<ResumeDto?> GetAboutUser()
    {
        var user = await _userRepository.FindUserByLoginAsync(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email")?.Value!);
        return await ToResumeDto(user);
    }

    [Authorize(Policy = "CompanyPolicy")]
    [HttpGet, Route("company")]
    public async Task<CompanyAboutDto?> GetAboutCompany()
    {
        var company =
            await _companiesRepository.GetCompanyByEmail(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email")
                ?.Value!);
        return await ToCompanyAboutDto(company);
    }

    private async Task<ResumeDto?> ToResumeDto(User? user)
    {
        if (user is null)
            return null;
        var photo = await _photoHelper.LoadFrom(user.ImagePath ?? "../photos/users/default.txt");
        return user.MapToResumeDto(photo);
    }

    private async Task<CompanyAboutDto?> ToCompanyAboutDto(Company? company)
    {
        if (company is null) return null;

        var photo = await _photoHelper.LoadFrom(company.ImagePath ?? "../photos/users/default.txt");
        var vacancies = await _vacancyRepository.GetAllCompanyVacancies(company.CompanyEmail);
        return company.MapToCompanyAboutDto(vacancies, photo);
    }
}