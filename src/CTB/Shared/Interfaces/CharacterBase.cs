using System;
using System.Text.Json.Serialization;

namespace CTB.Shared.Interfaces;

public class CharacterBase
{
    [JsonPropertyName("id")]
    public string ID { get; set; }

    [JsonPropertyName("position")]
    public Position Position { get; set; } = new();

    [JsonPropertyName("animation")]
    public double Animation { get; set; }

    [JsonIgnore]
    public Position ServerPosition { get; set; } = new();

    public void Update(double delta)
    {
        if (Position.Speed > 0)
        {
            Animation += delta;
            var animationThreshold = this switch
            {
                Shark => 80,
                Monkey => 20,
                _ => 10
            };

            if (Animation > animationThreshold)
            {
                Animation -= animationThreshold;
            }

            Position.X += Position.Speed * delta * Math.Cos(Position.Rotation);
            Position.Y += Position.Speed * delta * Math.Sin(Position.Rotation);

            if (Position.X < WorldConstants.BorderRadius)
            {
                Position.X = WorldConstants.BorderRadius;
            }
            else if (Position.X > WorldConstants.Screen.Width - WorldConstants.BorderRadius)
            {
                Position.X = WorldConstants.Screen.Width - WorldConstants.BorderRadius;
            }

            if (Position.Y < WorldConstants.BorderRadius)
            {
                Position.Y = WorldConstants.BorderRadius;
            }
            else if (Position.Y > WorldConstants.Screen.Height - WorldConstants.BorderRadius)
            {
                Position.Y = WorldConstants.Screen.Height - WorldConstants.BorderRadius;
            }
        }
        else
        {
            Animation = 0;
        }
    }
}
