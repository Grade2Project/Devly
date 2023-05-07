using Devly.Database.Models;
using Devly.Database.Repositories;
using Devly.Extensions;
using Devly.Models;
using Microsoft.AspNetCore.Mvc;

namespace Devly.Controllers;

public class ResumeController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IGradesRepository _gradesRepository;
    private readonly IProgrammingLanguagesRepository _programmingLanguagesRepository;
    private readonly IUsersFavoriteLanguagesRepository _usersFavoriteLanguagesRepository;

    public ResumeController(IUserRepository userRepository,
        IGradesRepository gradesRepository,
        IProgrammingLanguagesRepository programmingLanguagesRepository,
        IUsersFavoriteLanguagesRepository usersFavoriteLanguagesRepository)
    {
        _userRepository = userRepository;
        _gradesRepository = gradesRepository;
        _programmingLanguagesRepository = programmingLanguagesRepository;
        _usersFavoriteLanguagesRepository = usersFavoriteLanguagesRepository;
    }
    
    [HttpPost, Route("resume/update")]
    public async Task<IActionResult> UpdateResume([FromBody] ResumeDto resumeDto)
    {
        try
        {
            var resumeToUser = await ResumeToUser(resumeDto);
            if (resumeToUser is null)
                return StatusCode(400, "Bad Grade");
            var usersFavoriteLanguages = await GetUsersFavoriteLanguages(resumeDto.FavoriteLanguages)!;
            if (usersFavoriteLanguages is null)
                return StatusCode(400, "Bad Languages");
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

    private async Task<ProgrammingLanguage[]>? GetUsersFavoriteLanguages(string[] languages)
    {
        var favoriteLanguages = new List<ProgrammingLanguage>();
        foreach (var languageName in languages)
        {
            var dbLanguage = await _programmingLanguagesRepository.FindLanguageAsync(languageName);
            if (dbLanguage is null)
                return null;
            favoriteLanguages.Add(dbLanguage);
        }

        return favoriteLanguages.ToArray();
    }
}