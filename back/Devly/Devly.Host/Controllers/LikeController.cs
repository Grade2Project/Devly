using Devly.Database.Repositories.Abstract;
using Devly.Models;
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
    [Route("like/vacancy")]
    public async Task<IActionResult> VacancyLike([FromBody] UserLikeDto userLikeDto)
    {
        var (userLogin, vacancyId) = (userLikeDto.UserLogin, userLikeDto.VacancyId);
        if (await _userRepository.FindUserByLoginAsync(userLogin) == null ||
            await _vacancyRepository.FindVacancyByIdAsync(vacancyId) == null)
            return StatusCode(400, "Login or vacancy doesn't exists");

        try
        {
            await _favoriteVacanciesRepository.InsertLikePair(userLogin, vacancyId);
        }
        catch (Exception e)
        {
            return StatusCode(400, "Relation exists");
        }

        return Ok();
    }

    [HttpPost]
    [Route("unlike/vacancy")]
    public async Task<IActionResult> VacancyUnlike([FromBody] UserLikeDto userLikeDto)
    {
        var (userLogin, vacancyId) = (userLikeDto.UserLogin, userLikeDto.VacancyId);
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
    [Route("like/user")]
    public async Task<IActionResult> UserLike([FromBody] CompanyLikeDto companyLikeDto)
    {
        var (companyEmail, userLogin) = (companyLikeDto.CompanyEmail, companyLikeDto.UserLogin);
        var company = await _companiesRepository.GetCompanyByEmail(companyEmail);
        if (await _userRepository.FindUserByLoginAsync(userLogin) == null || company == null)
            return StatusCode(400, "Company or user doesn't exists");

        try
        {
            await _favoriteUsersRepository.InsertLikePair(company.Id, userLogin);
        }
        catch (Exception e)
        {
            return StatusCode(400, "Relation exists");
        }

        return Ok();
    }

    [HttpPost]
    [Route("unlike/user")]
    public async Task<IActionResult> UserUnlike([FromBody] CompanyLikeDto companyLikeDto)
    {
        var (companyEmail, userLogin) = (companyLikeDto.CompanyEmail, companyLikeDto.UserLogin);
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