using Devly.Database.Repositories.Abstract;
using Devly.Extensions;
using Devly.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Devly.Controllers;

public class LikeController : Controller
{
    private readonly ICompaniesRepository _companiesRepository;
    private readonly IFavoriteUsersRepository _favoriteUsersRepository;
    private readonly IFavoriteVacanciesRepository _favoriteVacanciesRepository;
    private readonly IUserRepository _userRepository;
    private readonly IVacancyRepository _vacancyRepository;

    public LikeController(IUserRepository userRepository,
        IVacancyRepository vacancyRepository,
        IFavoriteVacanciesRepository favoriteVacanciesRepository,
        ICompaniesRepository companiesRepository,
        IFavoriteUsersRepository favoriteUsersRepository)
    {
        _userRepository = userRepository;
        _vacancyRepository = vacancyRepository;
        _favoriteVacanciesRepository = favoriteVacanciesRepository;
        _companiesRepository = companiesRepository;
        _favoriteUsersRepository = favoriteUsersRepository;
    }

    [HttpPost]
    [Authorize(Policy = "UserPolicy")]
    [Route("like/vacancy")]
    public async Task<MutualityLikeDto<CompanyAboutDto>> VacancyLike([FromBody] int vacancyId)
    {
        var userLogin = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email")!.Value;
        var vacancy = await _vacancyRepository.FindVacancyByIdAsync(vacancyId);
        if (await _userRepository.FindUserByLoginAsync(userLogin) == null || vacancy is null)
            return null;

        try
        {
            await _favoriteVacanciesRepository.InsertLikePair(userLogin, vacancyId);
        }
        catch (Exception e)
        {
            return null;
        }

        var allUsersCompanyLiked = await _favoriteUsersRepository.GetAllUsersCompanyLiked(vacancy.CompanyId)!;
        var isMutual = allUsersCompanyLiked.FirstOrDefault(user => user.Login == userLogin) is not null;

        return new MutualityLikeDto<CompanyAboutDto>()
        {
            IsMutual = isMutual,
            Data = vacancy.Company.MapToCompanyAboutDto()
        };
    }

    [HttpPost]
    [Authorize(Policy = "UserPolicy")]
    [Route("unlike/vacancy")]
    public async Task<IActionResult> VacancyUnlike([FromBody] int vacancyId)
    {
        var userLogin = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email")!.Value;
        if (await _userRepository.FindUserByLoginAsync(userLogin) == null ||
            await _vacancyRepository.FindVacancyByIdAsync(vacancyId) == null)
            return StatusCode(400, "User or vacancy doesn't exists");

        try
        {
            await _favoriteVacanciesRepository.Unlike(userLogin, vacancyId);
        }
        catch (Exception e)
        {
            return StatusCode(400, "Relation doesn't exists");
        }

        return Ok();
    }

    [HttpPost]
    [Authorize(Policy = "CompanyPolicy")]
    [Route("like/user")]
    public async Task<MutualityLikeDto<ResumeDto>> UserLike([FromBody] string userLogin)
    {
        var companyEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email")!.Value;
        var company = await _companiesRepository.GetCompanyByEmail(companyEmail);
        var user = await _userRepository.FindUserByLoginAsync(userLogin);
        if (user == null || company == null)
            return null;

        try
        {
            await _favoriteUsersRepository.InsertLikePair(company.Id, userLogin);
        }
        catch (Exception e)
        { }

        var usersFavoriteVacancies = await _favoriteVacanciesRepository.GetAllVacanciesUserLiked(user.Login)!;
        var isMutual = usersFavoriteVacancies?.FirstOrDefault(vac => vac.CompanyId == company.Id) is not null;

        return new MutualityLikeDto<ResumeDto>()
        {
            IsMutual = isMutual,
            Data = user.MapToResumeDto()
        };
    }

    [HttpPost]
    [Authorize(Policy = "CompanyPolicy")]
    [Route("unlike/user")]
    public async Task<IActionResult> UserUnlike([FromBody] string userLogin)
    {
        var companyEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email")!.Value;
        var company = await _companiesRepository.GetCompanyByEmail(companyEmail);
        if (await _userRepository.FindUserByLoginAsync(userLogin) == null || company == null)
            return StatusCode(400, "Company or user doesn't exists");

        try
        {
            await _favoriteUsersRepository.Unlike(company.Id, userLogin);
        }
        catch (Exception e)
        {
            return StatusCode(400, "Relation doesn't exist");
        }

        return Ok();
    }
}