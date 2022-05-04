using System.Text.Json.Serialization;

namespace CTB.Shared.Interfaces;

public class Banana
{
    [JsonPropertyName("id")]
    public string ID { get; set; }

    [JsonPropertyName("position")]
    public Position Position { get; set; } = new();
}
