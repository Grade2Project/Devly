using Devly.Database.Repositories;
using Devly.Extensions;
using Devly.Models;
using Microsoft.AspNetCore.Mvc;

namespace Devly.Controllers;

public class ResumeController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IGradesRepository _gradesRepository;

    public ResumeController(IUserRepository userRepository, IGradesRepository gradesRepository)
    {
        _userRepository = userRepository;
        _gradesRepository = gradesRepository;
    }
    
    [HttpPost, Route("resume/update")]
    public async Task<IActionResult> UpdateResume([FromBody] ResumeDto resumeDto)
    {
        try
        {
            var grade = await _gradesRepository.FindGrade(resumeDto.Grade);
            if (grade == null)
            {
                throw new ArgumentException();
            }
            var resumeToUser = resumeDto.MapToUser(grade);
            if (await _userRepository.FindUserByLoginAsync(resumeDto.Login) != null)
            {
                await _userRepository.UpdateAsync(resumeToUser);
            }
            else
            {
                await _userRepository.InsertAsync(resumeToUser);
            }
        }
        catch (Exception e)
        {
            return StatusCode(400);
        }

        return Ok();
    }

}