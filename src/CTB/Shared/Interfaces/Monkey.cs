using System.Text.Json.Serialization;

namespace CTB.Shared.Interfaces;

public class Monkey : CharacterBase
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("score")]
    public int Score { get; set; }

    [JsonPropertyName("ui")]
    public int UI { get; set; }
}
