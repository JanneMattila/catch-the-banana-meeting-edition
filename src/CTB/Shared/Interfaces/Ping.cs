using System;
using System.Text.Json.Serialization;

namespace CTB.Shared.Interfaces;

public class Ping
{
    [JsonPropertyName("start")]
    public DateTimeOffset Start { get; set; } = DateTimeOffset.UtcNow;
}
