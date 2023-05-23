using System.ComponentModel.DataAnnotations.Schema;

namespace Devly.Database.Models;

public class CompaniesFavoriteUser
{
    public int CompanyId { get; set; }

    [ForeignKey(nameof(CompanyId))] public Company Company { get; set; }

    public string UserLogin { get; set; }

    [ForeignKey(nameof(UserLogin))] public User User { get; set; }
}