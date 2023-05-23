using Devly.Database.Models;
using Devly.Models;

namespace Devly.Extensions;

public static class UserExtensions
{
    public static ResumeDto MapToResumeDto(this User user)
    {
        return new ResumeDto
        {
            Login = user.Login,
            BirthDate = user.BirthDate,
            City = user.City,
            Info = user.Info,
            Name = user.Name,
            Email = user.Contact.Email,
            Phone = user.Contact.Phone,
            Grade = user.Grade.Value,
            FavoriteLanguages = user.FavoriteLanguages
                .Select(x => x.ProgrammingLanguage.LanguageName).ToArray()
        };
    }
}