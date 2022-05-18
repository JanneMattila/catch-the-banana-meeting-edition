using System.Text.Json.Serialization;

namespace CTB.Shared.Interfaces;

public class Shark : CharacterBase
{
    [JsonPropertyName("follows")]
    public string Follows { get; set; }
}
