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
            await _userRepository.UpdateAsync(resumeDto.MapToUser());
        }
        catch (Exception e)
        {
            return StatusCode(400);
        }

        return Ok();
    }

}