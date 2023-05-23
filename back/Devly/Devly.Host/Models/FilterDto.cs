namespace Devly.Models;

public class FilterDto
{
    public string[] Grades { get; set; }
    public string[] Languages { get; set; }
    public int SalaryFrom { get; set; }
    public int SalaryTo { get; set; }
}