using Devly.Database.Models;
using Devly.Models;

namespace Devly.Extensions;

public static class UserExtensions
{
    public static ResumeDto MapToResumeDto(this User user, byte[]? photo = null)
    {
        return new ResumeDto
        {
            Login = user.Login,
            Experience = user.Experience,
            Age = user.BirthDate.ToAge(),
            BirthDate = user.BirthDate,
            City = user.City.Name,
            Photo = photo,
            Info = user.Info,
            Name = user.Name,
            Grade = user.Grade.Value,
            FavoriteLanguages = user.FavoriteLanguages
                .Select(x => x.ProgrammingLanguage.LanguageName).ToArray()
        };
    }
}