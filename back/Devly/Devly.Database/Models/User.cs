using System.ComponentModel.DataAnnotations.Schema;

namespace Devly.Database.Models;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; }
    public DateTime BirthDate { get; set; }
    public int Age { get; set; }
    public int GradeId { get; set; }
    [ForeignKey("GradeId")]
    public Grade Grade { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Info { get; set; }
    public string ImagePath { get; set; }
}