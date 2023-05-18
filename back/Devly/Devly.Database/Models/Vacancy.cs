using System.ComponentModel.DataAnnotations.Schema;

namespace Devly.Database.Models;

public class Vacancy
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    
    [ForeignKey(nameof(CompanyId))]
    public Company Company { get; set; }
    public int ProgrammingLanguageId { get; set; }
    
    [ForeignKey(nameof(ProgrammingLanguageId))]
    public ProgrammingLanguage ProgrammingLanguage { get; set; }
    
    public int GradeId { get; set; }
    [ForeignKey(nameof(GradeId))]
    public Grade Grade { get; set; }
    public int Salary { get; set; }
    public string Info { get; set; }
}