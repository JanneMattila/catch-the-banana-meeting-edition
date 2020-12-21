using System.Text.Json.Serialization;

namespace CTB.Shared
{
    public class CanvasTouch
    {
        [JsonPropertyName("id")]
        public double ID { get; set; }

        [JsonPropertyName("x")]
        public double X { get; set; }

        [JsonPropertyName("y")]
        public double Y { get; set; }
    }
}
