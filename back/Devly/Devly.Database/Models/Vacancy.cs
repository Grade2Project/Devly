namespace Devly.Database.Models;

public class Vacancy
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int ProgrammingLanguageId { get; set; }
    public int Salary { get; set; }
    public string Info { get; set; }
}