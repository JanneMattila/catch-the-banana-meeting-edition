using System.Text.Json.Serialization;

namespace CTB.Shared.Interfaces
{
    public class Position
    {
        [JsonPropertyName("x")]
        public int X { get; set; }

        [JsonPropertyName("y")]
        public int Y { get; set; }

        [JsonPropertyName("rotation")]
        public double Rotation { get; set; }

        [JsonPropertyName("speed")]
        public int Speed { get; set; }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}, R: {Rotation}, S: {Speed}";
        }
    }
}
