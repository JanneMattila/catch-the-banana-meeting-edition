using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTB.Shared
{
    public class GameEngine
    {
        public string PlayerID { get; private set; }
        public string PlayerName { get; private set; }

        public void SetPlayerID(string playerID)
        {
            PlayerID = playerID;
        }

        public void SetPlayerName(string playerName)
        {
            PlayerName = playerName;
        }

        public void Update(double delta)
        {
        }
    }
}
