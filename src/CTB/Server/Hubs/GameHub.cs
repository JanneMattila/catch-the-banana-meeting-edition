using System;
using System.Threading.Tasks;
using CTB.Server.Logic;
using CTB.Shared;
using Microsoft.AspNetCore.SignalR;

namespace CTB.Server.Hubs
{
    public class GameHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            var name = PlayerNameGenerator.CreateName();
            await Clients.Caller.SendAsync(HubConstants.NamePlayerEventMethod, name);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task MoveEvent(string move)
        {
            await Clients.All.SendAsync(HubConstants.MoveEventMethod, move);
        }
    }
}
