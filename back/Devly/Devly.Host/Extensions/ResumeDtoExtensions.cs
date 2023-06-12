using Devly.Database.Models;
using Devly.Models;

namespace Devly.Extensions;

public static class ResumeDtoExtensions
{
    public static User MapToUser(this ResumeDto resumeDto, Grade grade, int cityId)
    {
        return new User
        {
            Login = resumeDto.Login,
            Experience = resumeDto.Experience,
            BirthDate = resumeDto.BirthDate,
            CityId = cityId,
            Contact = new Contact { Email = resumeDto.Email, Phone = resumeDto.Phone },
            GradeId = grade.Id,
            Info = resumeDto.Info,
            Name = resumeDto.Name
        };
    }
}