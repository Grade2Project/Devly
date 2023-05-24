using Devly.Services;

namespace Devly.Models;

public class VacancyFilterDto
{
    public string UserLogin { get; set; }
    public string[] Grades { get; set; }
    public string[] Languages { get; set; }
    public int SalaryFrom { get; set; }
    public int SalaryTo { get; set; }

    public override int GetHashCode()
    {
        var comparer = new ArrayComparer<string>();
        return comparer.GetHashCode(Grades) ^ comparer.GetHashCode(Languages) ^ UserLogin.GetHashCode() ^
               SalaryFrom.GetHashCode() ^ SalaryTo.GetHashCode();
    }
}