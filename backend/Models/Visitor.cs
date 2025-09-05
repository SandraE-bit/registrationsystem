using Newtonsoft.Json;

namespace backend.Models;

public partial class Article
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonProperty("createdBy")]
    public Guid CreatedBy { get; set; }

    [JsonProperty("createdOn")]
    public DateTime CreatedOn { get; set; }

    [JsonProperty("body")]
    public string Body { get; set; } = "";
}
