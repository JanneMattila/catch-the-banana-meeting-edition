using CTB.Server.Logic;
using System.Collections.Generic;

namespace CTB.Server.Data
{
    public class Repository : IRepository
    {
        private static Dictionary<string, string> s_players = new Dictionary<string, string>();

        public string GetName(string playerID)
        {
            if (s_players.ContainsKey(playerID))
            {
                return s_players[playerID];
            }

            var name = PlayerNameGenerator.CreateName();
            s_players[playerID] = name;
            return name;
        }
    }
}
