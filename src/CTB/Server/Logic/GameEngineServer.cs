using CTB.Server.Data;
using CTB.Server.Hubs;
using CTB.Shared;
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
        private readonly Random _random = new();

        public GameEngineServer(IRepository repository, IHubContext<GameHub> gameHub, ILogger<GameEngineServer> logger)
        {
            _repository = repository;
            _gameHub = gameHub;
            _logger = logger;
        }

        public Position MoveMonkey(Monkey monkey, Position position)
        {
            double distance = CalculateDistance(monkey.Position, position);
            if (distance > 10)
            {
                _logger.LogWarning($"Distance between positions is {distance}. Old: {monkey.Position} -> New: {position}");
            }

            // Accept new position
            return position;
        }

        private static double CalculateDistance(Position left, Position right)
        {
            var x1 = left.X;
            var y1 = left.Y;
            var x2 = right.X;
            var y2 = right.Y;

            var dx = x2 - x1;
            var dy = y2 - y1;
            var distance = Math.Sqrt(dx * dx + dy * dy);
            return distance;
        }

        public async Task<bool> UpdateAsync(double delta)
        {
            var monkeys = _repository.GetMonkeys();
            var sharks = _repository.GetSharks();
            var bananas = _repository.GetBananas();
            var eatenBananas = new HashSet<string>();

            foreach (var monkey in monkeys)
            {
                if (monkey.Position.Speed > 0)
                {
                    monkey.Position.X += (int)Math.Round(delta * Math.Cos(monkey.Position.Rotation));
                    monkey.Position.Y += (int)Math.Round(delta * Math.Sin(monkey.Position.Rotation));

                    foreach (var banana in bananas)
                    {
                        var distance = CalculateDistance(banana.Position, monkey.Position);
                        if (distance < 10)
                        {
                            monkey.Score++;
                            eatenBananas.Add(banana.ID);
                        }
                    }
                }
            }

            foreach (var eatenBanana in eatenBananas)
            {
                _repository.DeleteBanana(eatenBanana);
                await _gameHub.Clients.All.SendAsync(HubConstants.DeleteBananaEventMethod, eatenBanana);
            }

            if (!bananas.Any())
            {
                var banana = new Banana()
                {
                    ID = Guid.NewGuid().ToString("B"),
                    Position = new Position()
                    {
                        X = _random.Next(WorldConstants.BorderRadius, WorldConstants.Width - WorldConstants.BorderRadius * 2),
                        Y = _random.Next(WorldConstants.BorderRadius, WorldConstants.Height - WorldConstants.BorderRadius * 2)
                    }
                };
                _repository.AddBanana(banana);

                _logger.LogInformation(LoggingEvents.GameEngineAddBanana, $"Add Banana ID: {banana.ID}, {banana.Position}");
                await _gameHub.Clients.All.SendAsync(HubConstants.MoveBananaEventMethod, banana);
            }

            foreach (var shark in sharks)
            {
                if (shark.Position.Speed > 0)
                {
                    shark.Position.X += (int)Math.Round(delta * Math.Cos(shark.Position.Rotation));
                    shark.Position.Y += (int)Math.Round(delta * Math.Sin(shark.Position.Rotation));

                    Monkey closestMonkey = null;
                    var closestDistance = double.MaxValue;
                    foreach (var monkey in monkeys)
                    {
                        var distance = CalculateDistance(shark.Position, monkey.Position);
                        if (distance < closestDistance)
                        {
                            closestMonkey = monkey;
                            closestDistance = distance;
                        }

                        if (distance < 10)
                        {
                            // TODO: Monkey has been eaten by the shark!
                        }
                    }

                    if (closestMonkey != null)
                    {
                        shark.Position.Rotation = CalculateAngle(shark.Position, closestMonkey.Position);
                    }
                }
            }

            if (!sharks.Any())
            {
                var shark = new Shark()
                {
                    ID = Guid.NewGuid().ToString("B"),
                    Position = new Position()
                    {
                        X = _random.Next(WorldConstants.BorderRadius, WorldConstants.Width - WorldConstants.BorderRadius * 2),
                        Y = _random.Next(WorldConstants.BorderRadius, WorldConstants.Height - WorldConstants.BorderRadius * 2)
                    }
                };
                _repository.AddShark(shark);

                _logger.LogInformation(LoggingEvents.GameEngineAddShark, $"Add Shark ID: {shark.ID}, {shark.Position}");
                await _gameHub.Clients.All.SendAsync(HubConstants.MoveSharkEventMethod, shark);
            }

            return monkeys.Any();
        }

        private double CalculateAngle(Position from, Position to)
        {
            var dx = to.X - from.X;
            var dy = from.Y - to.Y;
            return Math.Atan2(dy, dx);
        }
    }
}
