using Devly.Services;

namespace Devly.Models;

public class UserFilterDto
{
    public string[]? Grades { get; set; }
    public string[]? Languages { get; set; }
    public string UserName { get; set; }

    public override int GetHashCode()
    {
        var comparer = new ArrayComparer<string>();
        var ans = comparer.GetHashCode(Grades) ^ comparer.GetHashCode(Languages);
        return UserName is null ? ans : ans ^ UserName.GetHashCode();
    }
}