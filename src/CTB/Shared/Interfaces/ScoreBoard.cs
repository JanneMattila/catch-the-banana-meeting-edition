using System.Text.Json.Serialization;

namespace CTB.Shared.Interfaces;

public class ScoreBoard
{
    [JsonPropertyName("id")]
    public string ID { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("score")]
    public int Score { get; set; }

    public static ScoreBoard FromMonkey(Monkey monkey)
    {
        return new ScoreBoard()
        {
            ID = monkey.ID,
            Name = monkey.Name,
            Score = monkey.Score
        };
    }
}
