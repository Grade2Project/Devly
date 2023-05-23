using System.ComponentModel.DataAnnotations;

namespace Devly.Database.Models;

public class CompanyPassword
{
    [Key] public int CompanyId { get; set; }

    public string HashedPass { get; set; }
}