namespace Devly.Database.Models;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; }
    public int Age { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Info { get; set; }
    public string ImagePath { get; set; }
}