using System.ComponentModel.DataAnnotations.Schema;

namespace Devly.Database.Models;

public class UsersFavoriteLanguage
{
    public string UserLogin { get; set; }

    [ForeignKey(nameof(UserLogin))] public User User { get; set; }

    public int ProgrammingLanguageId { get; set; }

    [ForeignKey(nameof(ProgrammingLanguageId))]
    public ProgrammingLanguage ProgrammingLanguage { get; set; }
}