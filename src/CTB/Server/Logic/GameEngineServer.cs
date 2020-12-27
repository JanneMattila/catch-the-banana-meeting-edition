using CTB.Server.Data;
using CTB.Server.Hubs;
using CTB.Shared.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTB.Server.Logic
{
    public class GameEngineServer : IGameEngineServer
    {
        private readonly IRepository _repository;
        private readonly IHubContext<GameHub> _gameHub;

        public GameEngineServer(IRepository repository, IHubContext<GameHub> gameHub)
        {
            _repository = repository;
            _gameHub = gameHub;
        }

        public int Update(double delta)
        {
            var monkeys = _repository.GetMonkeys();
            foreach (var monkey in monkeys)
            {
                if (monkey.Position.Speed > 0)
                {
                    monkey.Position.X += (int)Math.Round(delta / 10 * Math.Cos(monkey.Position.Rotation));
                    monkey.Position.Y += (int)Math.Round(delta / 10 * Math.Sin(monkey.Position.Rotation));
                }
            }
            return 0;
        }
    }
}
