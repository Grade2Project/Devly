namespace Devly.Models;

public class ResumeDto
{
    public string? Login { get; set; }
    public string Name { get; init; }
    public DateTime BirthDate { get; init; }
    public int? Age { get; init; }
    public string City { get; init; }
    public int Experience { get; init; }
    public string Info { get; init; }
    public string Grade { get; init; }
    public byte[]? Photo { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string[] FavoriteLanguages { get; init; }
}

// Info - json.
// Photo
// Nginx
// Grades с сервера 