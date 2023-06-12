using Devly.Database.Models;
using Devly.Models;

namespace Devly.Extensions;

public static class VacancyExtensions
{
    public static VacancyDto? MapToVacancyDto(this Vacancy vacancy, byte[]? photo = null)
    {
        return new VacancyDto
        {
            Id = vacancy.Id,
            CompanyName = vacancy.Company.CompanyName,
            Info = vacancy.Info,
            Photo = photo,
            City = vacancy.City.Name,
            ProgrammingLanguage = vacancy.ProgrammingLanguage.LanguageName,
            Salary = vacancy.Salary,
            Grade = vacancy.Grade.Value
        };
    }
}