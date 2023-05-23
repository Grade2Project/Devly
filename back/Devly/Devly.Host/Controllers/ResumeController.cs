using Devly.Database.Models;
using Devly.Database.Repositories;
using Devly.Database.Repositories.Abstract;
using Devly.Extensions;
using Devly.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Devly.Controllers;

public class ResumeController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IGradesRepository _gradesRepository;
    private readonly IProgrammingLanguagesRepository _programmingLanguagesRepository;
    private readonly IUsersFavoriteLanguagesRepository _usersFavoriteLanguagesRepository;
    private readonly ICompaniesRepository _companiesRepository;
    private readonly IVacancyRepository _vacancyRepository;

    public ResumeController(IUserRepository userRepository,
        IGradesRepository gradesRepository,
        IProgrammingLanguagesRepository programmingLanguagesRepository,
        IUsersFavoriteLanguagesRepository usersFavoriteLanguagesRepository,
        ICompaniesRepository companiesRepository, IVacancyRepository vacancyRepository)
    {
        _userRepository = userRepository;
        _gradesRepository = gradesRepository;
        _programmingLanguagesRepository = programmingLanguagesRepository;
        _usersFavoriteLanguagesRepository = usersFavoriteLanguagesRepository;
        _companiesRepository = companiesRepository;
        _vacancyRepository = vacancyRepository;
    }
    
    [Authorize(Policy = "CompanyPolicy")]
    [HttpPost, Route("vacancy/update")]
    public async Task<IActionResult> AddVacancy([FromBody] VacancyDto vacancyDto)
    {
        var vacancy = await DtoToVacancy(vacancyDto);
        if (vacancy is null || await _vacancyRepository.FindVacancyAsync(vacancy) != null)
        {
            return StatusCode(400, "Bad Vacancy");
        }

        await _vacancyRepository.InsertAsync(vacancy);
        return Ok();
    }
    
    [Authorize(Policy = "UserPolicy")]
    [HttpPost, Route("resume/update")]
    public async Task<IActionResult> UpdateResume([FromBody] ResumeDto resumeDto)
    {
        var tokenData = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Email");
        resumeDto.Login = tokenData!.Value;
        
        try
        {
            var resumeToUser = await ResumeToUser(resumeDto);
            if (resumeToUser is null)
            {
                return StatusCode(400, "Bad Grade");
            }

            var usersFavoriteLanguages = await _programmingLanguagesRepository.FindLanguagesAsync(resumeDto.FavoriteLanguages)!;
            if (usersFavoriteLanguages is null)
            {
                return StatusCode(400, "Bad Languages");
            }

            if (await _userRepository.FindUserByLoginAsync(resumeDto.Login) != null)
            {
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
        var grade = await _gradesRepository.FindGrade(resumeDto.Grade);
        return grade == null ? null : resumeDto.MapToUser(grade);
    }

    private async Task<Vacancy?> DtoToVacancy(VacancyDto vacancyDto)
    {
        var company = await _companiesRepository.GetCompanyByName(vacancyDto.CompanyName);
        if (company is null)
            return null;
        var language = await _programmingLanguagesRepository.FindLanguagesAsync(vacancyDto.ProgrammingLanguage);
        var grade = await _gradesRepository.FindGrade(vacancyDto.Grade);
        if (language is null || language.Count == 0 || grade == null)
        {
            return null;
        }

        return new Vacancy
        {
            CompanyId = company.Id,
            Info = vacancyDto.Info,
            ProgrammingLanguageId = language[0].Id,
            Salary = vacancyDto.Salary,
            GradeId = grade.Id
        };
    }
}