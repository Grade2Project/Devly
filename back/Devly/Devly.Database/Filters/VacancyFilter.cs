namespace Devly.Database.Filters;

public class VacancyFilter
{
    public int[] GradeIds { get; init; }
    public int[] LanguageIds { get; init; }
    public int SalaryFrom { get; init; }
    public int SalaryTo { get; init; }
    
    public string CompanyName { get; init; }
}