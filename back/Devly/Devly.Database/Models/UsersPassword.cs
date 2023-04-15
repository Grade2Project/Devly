using System.ComponentModel.DataAnnotations;

namespace Devly.Database.Models;

public class UsersPassword
{
    [Key]
    public string UserLogin { get; set; }
    public string HashedPass { get; set; }
}