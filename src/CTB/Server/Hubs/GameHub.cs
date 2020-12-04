using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace CTB.Server.Hubs
{
    public class GameHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
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
