using System.Text.Json.Serialization;

namespace CTB.Shared.Interfaces;

public class Position
{
    [JsonPropertyName("x")]
    public double X { get; set; }

    [JsonPropertyName("y")]
    public double Y { get; set; }

    [JsonPropertyName("rotation")]
    public double Rotation { get; set; }

    [JsonPropertyName("speed")]
    public double Speed { get; set; }

    public override string ToString()
    {
        return $"X: {X}, Y: {Y}, R: {Rotation}, S: {Speed}";
    }
}
