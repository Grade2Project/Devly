using Devly.Database.Models;
using Devly.Models;

namespace Devly.Extensions;

public static class ResumeDtoExtensions
{
    public static User MapToUser(this ResumeDto resumeDto, Grade grade)
    {
        return new User
        {
            Login = resumeDto.Login,
            BirthDate = resumeDto.BirthDate,
            City = resumeDto.City,
            Contact = new Contact { Email = resumeDto.Email, Phone = resumeDto.Phone },
            GradeId = grade.Id,
            ImagePath = resumeDto.ImagePath,
            Info = resumeDto.Info,
            Name = resumeDto.Name
        };
    }
}