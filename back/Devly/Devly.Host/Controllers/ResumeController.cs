using Devly.Database.Repositories;
using Devly.Extensions;
using Devly.Models;
using Microsoft.AspNetCore.Mvc;

namespace Devly.Controllers;

public class ResumeController : Controller
{
    private readonly IUserRepository _userRepository;

    public ResumeController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    [HttpPost, Route("resume/update")]
    public async Task<IActionResult> UpdateResume([FromBody] ResumeDto resumeDto)
    {
        try
        {
            var resumeToUser = resumeDto.MapToUser();
            if (await _userRepository.FindUserByLoginAsync(resumeDto.Login) != null)
                await _userRepository.UpdateAsync(resumeToUser);
            else
                await _userRepository.InsertAsync(resumeToUser);
        }
        catch (Exception e)
        {
            return StatusCode(400);
        }

        return Ok();
    }

}