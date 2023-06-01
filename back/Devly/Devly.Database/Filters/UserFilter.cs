namespace Devly.Database.Filters;

public class UserFilter
{
    public int[] GradeIds { get; init; }
    public int[] LanguageIds { get; init; }
    
    public string UserName { get; init; }
    public string City { get; init; }
    public int ExperienceFrom { get; init; }
}