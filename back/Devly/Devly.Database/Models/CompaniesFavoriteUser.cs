using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Devly.Database.Models;

public class CompaniesFavoriteUser
{
    public int CompanyId { get; set; }
    
    [ForeignKey(nameof(CompanyId))]
    public Company Company { get; set; }
    public string UserLogin { get; set; }
    
    [ForeignKey(nameof(User))]
    public User User { get; set; }
}