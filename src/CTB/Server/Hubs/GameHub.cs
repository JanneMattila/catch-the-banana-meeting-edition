using System;
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
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task PlayerIDEvent(string playerID)
        {
            var name = _repository.GetName(playerID);
            await Clients.Caller.SendAsync(HubConstants.PlayerNameEventMethod, name);
        }

        public async Task MoveEvent(Position position)
        {
            await Clients.Others.SendAsync(HubConstants.MoveEventMethod, position);
        }
    }
}
