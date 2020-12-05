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

        private Game _game = new Game();
        private Action<Game> _executeDraw;

        public void SetExecuteDraw(Action<Game> executeDraw)
        {
            _executeDraw = executeDraw;
        }

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
            _executeDraw(_game);
        }
    }
}
