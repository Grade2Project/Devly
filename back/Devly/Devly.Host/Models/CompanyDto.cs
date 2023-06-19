namespace Devly.Models;

public class CompanyDto
{
    public string CompanyName { get; init; }
    public string CompanyEmail { get; init; }
    public string CompanyInfo { get; init; }
    public string Password { get; set; }
    public byte[] Photo { get; set; }
}