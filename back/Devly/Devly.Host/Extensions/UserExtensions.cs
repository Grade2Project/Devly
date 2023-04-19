using Devly.Database.Models;
using Devly.Models;

namespace Devly.Extensions;

public static class UserExtensions
{
    public static ResumeDto MapToResumeDto(this User user)
    {
        return new ResumeDto
        {
            BirthDate = user.BirthDate,
            City = user.City,
            Email = user.Contact.Email,
            Info = user.Info,
            Name = user.Name,
            Phone = user.Contact.Phone
        };
    }
}