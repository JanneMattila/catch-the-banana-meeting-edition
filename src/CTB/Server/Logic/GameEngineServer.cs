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

namespace CTB.Server.Logic;

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

        var trackedMonkey = new List<string>();
        foreach (var shark in sharks)
        {
            Monkey closestMonkey = null;
            foreach (var monkey in monkeys)
            {
                var collision = CheckCollision(monkey.Position, WorldConstants.Monkey, shark.Position, WorldConstants.Shark);
                if (collision)
                {
                    // Monkey has been eaten by the shark!
                    CreateRandomPosition(monkey.Position);

                    await _gameHub.Clients.All.SendAsync(HubConstants.MoveMonkeyEventMethod, monkey);
                }
                else
                {
                    if (trackedMonkey.Contains(monkey.ID))
                    {
                        // Some other shark is already tracking this monkey.
                        continue;
                    }

                    closestMonkey = monkey;
                }
            }

            if (closestMonkey != null)
            {
                trackedMonkey.Add(closestMonkey.ID);

                shark.Position.Rotation = CalculateAngle(shark.Position, closestMonkey.Position);

                _logger.LogTrace($"Closest monkey: {closestMonkey.ID}, {shark.Position.Rotation}, {closestMonkey.Position.Rotation}");

                MoveObject(shark.Position, delta);

                if (closestMonkey.ID != shark.Follows)
                {
                    shark.Follows = closestMonkey.ID;
                    await _gameHub.Clients.All.SendAsync(HubConstants.MoveSharkEventMethod, shark);
                }
            }
        }

        const int monkeyToSharkRatio = 1;
        if (sharks.Count * monkeyToSharkRatio < monkeys.Count)
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
        else if (sharks.Count * monkeyToSharkRatio > monkeys.Count)
        {
            var shark = sharks.First();
            _repository.DeleteShark(shark.ID);
            await _gameHub.Clients.All.SendAsync(HubConstants.DeleteSharkEventMethod, shark.ID, shark);
        }

        return monkeys.Any();
    }
}
