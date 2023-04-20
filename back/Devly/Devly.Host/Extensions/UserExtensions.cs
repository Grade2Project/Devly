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
            Info = user.Info,
            Name = user.Name,
            Email = user.Contact.Email,
            Phone = user.Contact.Phone,
            Grade = user.Grade.Value
        };
    }
}