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
        private static ConcurrentDictionary<string, Monkey> s_monkeys = new();

        public GameHub(IRepository repository)
        {
            _repository = repository;
        }

        public override async Task OnConnectedAsync()
        {
            foreach (var monkey in s_monkeys)
            {
                await Clients.Caller.SendAsync(HubConstants.MonkeyConnectedEventMethod, monkey);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var monkey = s_monkeys[Context.ConnectionId];
            await Clients.Others.SendAsync(HubConstants.MonkeyDisconnectedEventMethod, monkey);

            s_monkeys.Remove(Context.ConnectionId, out var _);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task PlayerIDEvent(string playerID)
        {
            var monkey = _repository.GetByPlayerID(playerID);

            s_monkeys.AddOrUpdate(Context.ConnectionId, monkey, (key, previous) => { return monkey; });
            await Clients.Caller.SendAsync(HubConstants.PlayerRegisteredEventMethod, monkey);
            await Clients.Others.SendAsync(HubConstants.MonkeyConnectedEventMethod, monkey);
        }

        public async Task MoveEvent(Position position)
        {
            var monkey = s_monkeys[Context.ConnectionId];
            monkey.Position = position;
            await Clients.Others.SendAsync(HubConstants.MoveEventMethod, monkey);
        }
    }
}
