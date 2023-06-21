using Devly.Database.Models;
using Devly.Models;

namespace Devly.Extensions;

public static class CompanyExtensions
{
    public static CompanyAboutDto MapToCompanyAboutDto(this Company company, byte[]? photo = null, IReadOnlyList<Vacancy>? vacancies = null)
    {
        return new CompanyAboutDto
        {
            CompanyName = company.CompanyName,
            Info = company.Info,
            Photo = photo,
            Vacancies = vacancies is null ? company.Vacancies.Select(x => x.MapToVacancyDto()).ToArray() : Array.Empty<VacancyDto>(),
        };
    }
}