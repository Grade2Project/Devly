using Devly.Database.Models;
using Devly.Models;

namespace Devly.Extensions;

public static class ResumeDtoExtensions
{
    public static User MapToUser(this ResumeDto resumeDto)
    {
        return new User
        {
            Login = resumeDto.Login,
            BirthDate = resumeDto.BirthDate,
            City = resumeDto.City,
            Contact = new Contact { Email = resumeDto.Email, Phone = resumeDto.Phone },
            Grade = new Grade { Value = resumeDto.Grade },
            ImagePath = resumeDto.ImagePath,
            Info = resumeDto.Info,
            Name = resumeDto.Name
        };
    }
}