using System.ComponentModel.DataAnnotations.Schema;

namespace Devly.Database.Models;

public class UserPassword
{
    public string UserLogin { get; set; }
    [ForeignKey(nameof(UserLogin))]
    public User User { get; set; }
    public string HashedPass { get; set; }
}