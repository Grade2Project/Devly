using Devly.Services;

namespace Devly.Models;

public class UserFilterDto
{
    public string CompanyEmail { get; set; }
    public string[] Grades { get; set; }
    public string[] Languages { get; set; }

    public override int GetHashCode()
    {
        var comparer = new ArrayComparer<string>();
        return comparer.GetHashCode(Grades) ^ comparer.GetHashCode(Languages) ^ CompanyEmail.GetHashCode();
    }
}