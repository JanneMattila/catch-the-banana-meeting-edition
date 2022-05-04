using CTB.Shared.Interfaces;
using System.Collections.Generic;

namespace CTB.Shared;

public class Game
{
    public Monkey Me { get; init; } = new();

    public List<Monkey> Monkeys { get; init; } = new();
    public List<Banana> Bananas { get; init; } = new();
    public List<Shark> Sharks { get; init; } = new();

    public List<ScoreBoard> ScoreBoard { get; init; } = new();
}
