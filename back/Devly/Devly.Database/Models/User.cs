using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Devly.Database.Models;

public class User
{
    [ForeignKey(nameof(Login))] public UsersPassword UsersPassword { get; set; }

    [Key] public string Login { get; set; }

    public DateTime BirthDate { get; set; }
    public int GradeId { get; set; }

    [ForeignKey(nameof(GradeId))] public Grade Grade { get; set; }

    public string Name { get; set; }
    public int Experience { get; set; }
    public string City { get; set; }
    public string Info { get; set; }
    public string ImagePath { get; set; }
    public int ContactId { get; set; }

    [ForeignKey(nameof(ContactId))] public Contact Contact { get; set; }

    public IReadOnlyList<UsersFavoriteLanguage> FavoriteLanguages { get; set; }
}