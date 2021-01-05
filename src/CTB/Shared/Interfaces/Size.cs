using System.Text.Json.Serialization;

namespace CTB.Shared.Interfaces
{
    public class Size
    {
        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
