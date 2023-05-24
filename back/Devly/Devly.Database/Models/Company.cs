namespace Devly.Database.Models;

public class Company
{
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public string CompanyEmail { get; set; }
    public string Info { get; set; }

    public IReadOnlyList<Vacancy> Vacancies { get; set; }
}