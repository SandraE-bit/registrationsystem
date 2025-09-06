using Newtonsoft.Json;

namespace backend.Models;

public class Visitor
{
    [JsonProperty("id")]
    public string Id { get; set; } = default!;

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("timestamp")]
    public DateTime Timestamp { get; set; }
}
