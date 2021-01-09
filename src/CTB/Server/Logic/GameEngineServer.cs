using CTB.Server.Data;
using CTB.Server.Hubs;
using CTB.Shared;
using CTB.Shared.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTB.Server.Logic
{
    public class GameEngineServer : GameEngineBase, IGameEngineServer
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
            double distance = CalculateDistance(monkey.Position, position);
            if (distance > 10)
            {
                _logger.LogWarning($"Distance between positions is {distance}. Old: {monkey.Position} -> New: {position}");
            }

            // Accept new position
            return position;
        }

        public async Task<bool> UpdateAsync(double delta)
        {
            var monkeys = _repository.GetConnectedMonkeys();
            var sharks = _repository.GetSharks();
            var bananas = _repository.GetBananas();
            var eatenBananas = new Dictionary<string, List<string>>();

            foreach (var monkey in monkeys)
            {
                if (monkey.Position.Speed > 0)
                {
                    MoveObject(monkey.Position, delta);
                    foreach (var banana in bananas)
                    {
                        var distance = CalculateDistance(banana.Position, monkey.Position);
                        if (distance <= WorldConstants.Banana.Height)
                        {
                            var collision = CheckCollision(monkey.Position, WorldConstants.Monkey, banana.Position, WorldConstants.Banana);
                            if (collision)
                            {
                                monkey.Score++;
                                if (eatenBananas.ContainsKey(banana.ID))
                                {
                                    eatenBananas[banana.ID].Add(monkey.ID);
                                }
                                else
                                {
                                    eatenBananas[banana.ID] = new List<string>()
                                    {
                                        monkey.ID
                                    };
                                }
                            }
                        }
                    }
                }
            }

            foreach (var eatenBanana in eatenBananas)
            {
                _repository.DeleteBanana(eatenBanana.Key);
                await _gameHub.Clients.All.SendAsync(HubConstants.DeleteBananaEventMethod, eatenBanana.Key, eatenBanana.Value);
            }

            if (!bananas.Any())
            {
                var banana = new Banana()
                {
                    ID = Guid.NewGuid().ToString("B")
                };
                CreateRandomPosition(banana.Position);
                _repository.AddBanana(banana);

                _logger.LogInformation(LoggingEvents.GameEngineAddBanana, $"Add Banana ID: {banana.ID}, {banana.Position}");
                await _gameHub.Clients.All.SendAsync(HubConstants.MoveBananaEventMethod, banana);
            }

            foreach (var shark in sharks)
            {
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
                }

                if (closestMonkey != null)
                {
                    shark.Position.Rotation = CalculateAngle(shark.Position, closestMonkey.Position);

                    _logger.LogTrace($"Closest monkey: {closestMonkey.ID}, {closestDistance}, {shark.Position.Rotation}, {closestMonkey.Position.Rotation}");

                    MoveObject(shark.Position, delta);

                    if (closestMonkey.ID != shark.Follows)
                    {
                        shark.Follows = closestMonkey.ID;
                        await _gameHub.Clients.All.SendAsync(HubConstants.MoveSharkEventMethod, shark);
                    }

                    var collision = CheckCollision(closestMonkey.Position, WorldConstants.Monkey, shark.Position, WorldConstants.Shark);
                    if (collision)
                    {
                        // Monkey has been eaten by the shark!
                        CreateRandomPosition(closestMonkey.Position);

                        await _gameHub.Clients.All.SendAsync(HubConstants.MoveMonkeyEventMethod, closestMonkey);
                    }
                }
            }

            if (!sharks.Any())
            {
                var shark = new Shark()
                {
                    ID = Guid.NewGuid().ToString("B")
                };
                shark.Position.Speed = 0.5;

                CreateRandomPosition(shark.Position);

                _repository.AddShark(shark);

                _logger.LogInformation(LoggingEvents.GameEngineAddShark, $"Add Shark ID: {shark.ID}, {shark.Position}");
                await _gameHub.Clients.All.SendAsync(HubConstants.MoveSharkEventMethod, shark);
            }

            return monkeys.Any();
        }
    }
}
