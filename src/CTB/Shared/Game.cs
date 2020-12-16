using CTB.Shared.Interfaces;
using System.Collections.Generic;

namespace CTB.Shared
{
    public class Game
    {
        public Monkey Me { get; init; } = new();

        public List<Monkey> Monkeys { get; init; } = new();
    }
}
