using System.Text.Json.Serialization;

namespace CTB.Shared.Interfaces
{
    public class Monkey
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("ui")]
        public string UI { get; set; }

        [JsonPropertyName("position")]
        public Position Position { get; set; } = new();
    }
}
