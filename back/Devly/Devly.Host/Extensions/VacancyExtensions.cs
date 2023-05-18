using Devly.Database.Models;
using Devly.Models;

namespace Devly.Extensions;

public static class VacancyExtensions
{
    public static VacancyDto? MapToVacancyDto(this Vacancy vacancy)
    {
        return new VacancyDto
        {
            CompanyName = vacancy.Company.CompanyName,
            Info = vacancy.Info,
            ProgrammingLanguage = vacancy.ProgrammingLanguage.LanguageName,
            Salary = vacancy.Salary,
            Grade = vacancy.Grade.Value
        };
    }
}