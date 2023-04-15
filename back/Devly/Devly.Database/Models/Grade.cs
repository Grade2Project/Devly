using System.Text.Json.Serialization;

namespace Devly.Database.Models;

public class Grade
{
    public int Id { get; set; }
    [JsonPropertyName("grade")]
    public string Value { get; set; }
}