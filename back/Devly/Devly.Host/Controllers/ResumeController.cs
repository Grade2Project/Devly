using Devly.Database.Models;
using Devly.Database.Repositories.Abstract;
using Devly.Extensions;
using Devly.Helpers;
using Devly.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Devly.Controllers;

public class ResumeController : Controller
{
    private readonly ICompaniesRepository _companiesRepository;
    private readonly IGradesRepository _gradesRepository;
    private readonly IProgrammingLanguagesRepository _programmingLanguagesRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUsersFavoriteLanguagesRepository _usersFavoriteLanguagesRepository;
    private readonly IVacancyRepository _vacancyRepository;
    private readonly IPhotoHelper _photo;
    private readonly ICitiesRepository _cities;

    public ResumeController(IUserRepository userRepository,
        IGradesRepository gradesRepository,
        IProgrammingLanguagesRepository programmingLanguagesRepository,
        IUsersFavoriteLanguagesRepository usersFavoriteLanguagesRepository,
        ICompaniesRepository companiesRepository, 
        IVacancyRepository vacancyRepository,
        IPhotoHelper photo,
        ICitiesRepository cities)
    {
        _userRepository = userRepository;
        _gradesRepository = gradesRepository;
        _programmingLanguagesRepository = programmingLanguagesRepository;
        _usersFavoriteLanguagesRepository = usersFavoriteLanguagesRepository;
        _companiesRepository = companiesRepository;
        _vacancyRepository = vacancyRepository;
        _photo = photo;
        _cities = cities;
    }
    
    [Authorize(Policy = "CompanyPolicy")]
    [HttpPost, Route("vacancy/update")]
    public async Task<IActionResult> AddVacancy([FromBody] VacancyDto vacancyDto)
    {
        var companyEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email")!.Value;
        var company = await _companiesRepository.GetCompanyByEmail(companyEmail);
        if (company is null || vacancyDto is null)
            return StatusCode(400, "Bad Vacancy");;
        var language = await _programmingLanguagesRepository.FindLanguagesAsync(vacancyDto.ProgrammingLanguage);
        var grade = await _gradesRepository.FindGrade(vacancyDto.Grade);
        var city = await _cities.GetCityByName(vacancyDto.City);
        if (language is null || language.Count == 0 || grade == null || city is null)
        {
            return StatusCode(400, "Bad Vacancy");
        }

        var vacancy = new Vacancy
        {
            CompanyId = company.Id,
            Info = vacancyDto.Info,
            CityId = city.Id,
            ProgrammingLanguageId = language[0].Id,
            Salary = vacancyDto.Salary,
            GradeId = grade.Id
        };

        await _vacancyRepository.InsertAsync(vacancy);
        return Ok();
    }

    [Authorize(Policy = "CompanyPolicy")]
    [HttpPost, Route("vacancy/delete")]
    public async Task<IActionResult> DeleteVacancy([FromBody] int vacancyId)
    {
        var companyEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email")!.Value;
        var vacancy = await _vacancyRepository.FindVacancyByIdAsync(vacancyId);
        if (vacancy == null || vacancy.Company.CompanyEmail != companyEmail)
            return StatusCode(403);
        _vacancyRepository.DeleteAsync(vacancy);
        return Ok();
    }
    
    [Authorize(Policy = "UserPolicy")]
    [HttpPost, Route("resume/update")]
    public async Task<IActionResult> UpdateResume([FromBody] ResumeDto resumeDto)
    {
        var tokenData = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email");
        resumeDto.Login = tokenData!.Value;
        resumeDto.Email = tokenData!.Value;
        
        try
        {
            var resumeToUser = await ResumeToUser(resumeDto);
            if (resumeToUser is null) return StatusCode(400, "Bad Grade");

            string path;
            if (resumeDto.Photo is { Length: > 0 })
            {
                path = $"../photos/users/{Guid.NewGuid()}.txt";
                _photo.Save(resumeDto.Photo, path);
            }
            else
            {
                path = "../photos/users/default.txt";
            }

            resumeToUser.ImagePath = path;

            var usersFavoriteLanguages = await _programmingLanguagesRepository.FindLanguagesAsync(resumeDto.FavoriteLanguages)!;
            if (usersFavoriteLanguages is null)
            {
                return StatusCode(400, "Bad Languages");
            }

            var user = await _userRepository.FindUserByLoginAsync(resumeDto.Login);
            if (user != null)
            {
                if (user.ImagePath.EndsWith("default.txt"))
                    _photo.Delete(user.ImagePath);
                await _userRepository.UpdateAsync(resumeToUser);
                await _usersFavoriteLanguagesRepository.UpdateAllForUserAsync(usersFavoriteLanguages, resumeDto.Login);
            }
            else
            {
                await _userRepository.InsertAsync(resumeToUser);
                await _usersFavoriteLanguagesRepository.InsertAllAsync(usersFavoriteLanguages, resumeDto.Login);
            }
        }
        catch (Exception e)
        {
            return StatusCode(400);
        }

        return Ok();
    }

    private async Task<User?> ResumeToUser(ResumeDto resumeDto)
    {
        var grade = await _gradesRepository.FindGrade(resumeDto.Grade)!;
        var city = await _cities.GetCityByName(resumeDto.City);
        return grade == null || city == null ? null : resumeDto.MapToUser(grade, city.Id);
    }
}