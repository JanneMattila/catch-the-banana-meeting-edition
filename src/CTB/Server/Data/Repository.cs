using CTB.Server.Logic;
using CTB.Shared.Interfaces;
using System;
using System.Collections.Generic;

namespace CTB.Server.Data
{
    public class Repository : IRepository
    {
        private static Dictionary<string, Monkey> s_players = new Dictionary<string, Monkey>();

        private Random _random = new Random();

        public Monkey Get(string playerID)
        {
            if (s_players.ContainsKey(playerID))
            {
                return s_players[playerID];
            }

            var name = PlayerNameGenerator.CreateName();
            s_players[playerID] = new Monkey()
            {
                ID = playerID,
                Name = name,
                Score = 0,
                UI = _random.Next(1, 3),
                Position = new Position()
                {
                    X = _random.Next(10, 500),
                    Y = _random.Next(10, 500)
                }
            };
            return s_players[playerID];
        }
    }
}
