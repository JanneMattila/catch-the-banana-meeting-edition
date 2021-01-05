using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using CTB.Server.Data;
using CTB.Server.Logic;
using CTB.Shared;
using CTB.Shared.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace CTB.Server.Hubs
{
    public class GameHub : Hub
    {
        private readonly IGameEngineServer _gameEngineServer;
        private readonly IRepository _repository;
        private readonly ILogger<GameHub> _logger;

        public GameHub(IGameEngineServer gameEngineServer, IRepository repository, ILogger<GameHub> logger)
        {
            _gameEngineServer = gameEngineServer;
            _repository = repository;
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            foreach (var monkey in _repository.GetMonkeys())
            {
                await Clients.Caller.SendAsync(HubConstants.MonkeyConnectedEventMethod, monkey);
            }

            foreach (var shark in _repository.GetSharks())
            {
                await Clients.Caller.SendAsync(HubConstants.MoveSharkEventMethod, shark);
            }

            foreach (var banana in _repository.GetBananas())
            {
                await Clients.Caller.SendAsync(HubConstants.MoveBananaEventMethod, banana);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var monkey = _repository.DeleteByConnectionID(Context.ConnectionId);
            await Clients.Others.SendAsync(HubConstants.MonkeyDisconnectedEventMethod, monkey);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task PlayerIDEvent(string playerID)
        {
            var monkey = _repository.MapConnectionIDToPlayer(Context.ConnectionId, playerID);

            await Clients.Caller.SendAsync(HubConstants.PlayerRegisteredEventMethod, monkey);
            await Clients.Others.SendAsync(HubConstants.MonkeyConnectedEventMethod, monkey);
        }

        public async Task MoveMonkeyEvent(Position position)
        {
            var monkey = _repository.GetByConnectionID(Context.ConnectionId);
            monkey.Position = _gameEngineServer.MoveMonkey(monkey, position);
            await Clients.Others.SendAsync(HubConstants.MoveMonkeyEventMethod, monkey);
        }
    }
}
