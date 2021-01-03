using CTB.Server.Data;
using CTB.Server.Hubs;
using CTB.Shared.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTB.Server.Logic
{
    public class GameEngineServer : IGameEngineServer
    {
        private readonly ILogger<GameEngineServer> _logger;
        private readonly IRepository _repository;
        private readonly IHubContext<GameHub> _gameHub;

        public GameEngineServer(IRepository repository, IHubContext<GameHub> gameHub, ILogger<GameEngineServer> logger)
        {
            _repository = repository;
            _gameHub = gameHub;
            _logger = logger;
        }

        public Position MoveMonkey(Monkey monkey, Position position)
        {
            var x1 = monkey.Position.X;
            var y1 = monkey.Position.Y;
            var x2 = position.X;
            var y2 = position.Y;

            var dx = x2 - x1;
            var dy = y2 - y1;
            var distance = Math.Sqrt(dx * dx + dy * dy);
            if (distance > 10)
            {
                _logger.LogWarning($"Distance between positions is {distance}. Old: {monkey.Position} -> New: {position}");
            }

            // Accept new position
            return position;
        }

        public bool Update(double delta)
        {
            var monkeys = _repository.GetMonkeys();
            foreach (var monkey in monkeys)
            {
                if (monkey.Position.Speed > 0)
                {
                    monkey.Position.X += (int)Math.Round(delta * Math.Cos(monkey.Position.Rotation));
                    monkey.Position.Y += (int)Math.Round(delta * Math.Sin(monkey.Position.Rotation));
                }
            }
            return monkeys.Any();
        }
    }
}
