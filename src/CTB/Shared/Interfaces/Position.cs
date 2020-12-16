using System.Text.Json.Serialization;

namespace CTB.Shared.Interfaces
{
    public class Position
    {
        [JsonPropertyName("x")]
        public int X { get; set; }

        [JsonPropertyName("y")]
        public int Y { get; set; }
    }
}
