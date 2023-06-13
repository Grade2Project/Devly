namespace Devly.Models;

public class VacancyDto
{
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public string City { get; set; }
    public byte[]? Photo { get; set; }
    public string ProgrammingLanguage { get; set; }
    public int Salary { get; set; }
    public string Info { get; set; }
    public string Grade { get; set; }
}