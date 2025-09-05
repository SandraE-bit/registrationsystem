using System.Text.Json.Serialization;

namespace backend.Models;

public class Visitor
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime Timestamp { get; set; }
}
