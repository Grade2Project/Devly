using Devly.Services;

namespace Devly.Models;

public class VacancyFilterDto
{
    public string CompanyName { get; set; }
    public string[] Grades { get; set; }
    public string[] Languages { get; set; }
    public int SalaryFrom { get; set; }
    public int SalaryTo { get; set; }

    public override int GetHashCode()
    {
        var comparer = new ArrayComparer<string>();
        var ans = comparer.GetHashCode(Grades) ^ comparer.GetHashCode(Languages) ^
                  SalaryFrom.GetHashCode() ^ SalaryTo.GetHashCode();
        return CompanyName is null ? ans : ans ^ CompanyName.GetHashCode();
    }
}