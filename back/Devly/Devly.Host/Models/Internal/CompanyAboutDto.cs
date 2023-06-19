namespace Devly.Models;

public class CompanyAboutDto
{
    public string CompanyName { get; set; }
    public string Info { get; set; }
    public byte[]? Photo { get; set; }
    public VacancyDto?[] Vacancies { get; set; }
}