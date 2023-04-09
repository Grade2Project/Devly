using System.ComponentModel.DataAnnotations.Schema;

namespace Devly.Database.Models;

public class UserPassword
{
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
    public string HashedPass { get; set; }
}