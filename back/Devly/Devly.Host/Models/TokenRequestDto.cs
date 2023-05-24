using System.ComponentModel.DataAnnotations;

namespace Devly.Models;

public class TokenRequestDto
{ 
    public string Email { get; init; }
    public Dictionary<string, object> CustomClaims { get; init; }
}