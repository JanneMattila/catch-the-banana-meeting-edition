using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using CTB.Server.Data;
using CTB.Server.Logic;
using CTB.Shared;
using CTB.Shared.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace CTB.Server.Hubs
{
    public class GameHub : Hub
    {
        private readonly IRepository _repository;

        public GameHub(IRepository repository)
        {
            _repository = repository;
        }

        public override async Task OnConnectedAsync()
        {
            foreach (var monkey in _repository.GetMonkeys())
            {
                await Clients.Caller.SendAsync(HubConstants.MonkeyConnectedEventMethod, monkey);
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

        public async Task MoveEvent(Position position)
        {
            var monkey = _repository.GetByConnectionID(Context.ConnectionId);
            monkey.Position = position;
            await Clients.Others.SendAsync(HubConstants.MoveEventMethod, monkey);
        }
    }
}
