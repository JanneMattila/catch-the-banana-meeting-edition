using System.Text.Json.Serialization;

namespace CTB.Shared.Interfaces
{
    public class Shark
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("position")]
        public Position Position { get; set; } = new();

        [JsonPropertyName("follows")]
        public string Follows { get; set; }
    }
}
