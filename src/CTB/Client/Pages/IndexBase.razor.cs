﻿using CTB.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CTB.Client.Pages
{
    public class IndexBase : ComponentBase
    {
        [Inject] 
        private HttpClient Http { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private HubConnection _hubConnection;

        protected override async Task OnInitializedAsync()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/GameHub"))
                .Build();

            _hubConnection.On(HubConstants.NamePlayerEventMethod, (string name) =>
            {
                NamePlayerEventReceived(name);
                StateHasChanged();
            });
            _hubConnection.On(HubConstants.MoveEventMethod, () =>
            {
                MoveEventReceived();
                StateHasChanged();
            });

            await _hubConnection.StartAsync();

            await base.OnInitializedAsync();
        }

        private void NamePlayerEventReceived(string name)
        {
            Console.WriteLine($"-> NamePlayerEventReceived: {name}");
        }

        private void MoveEventReceived()
        {
            Console.WriteLine("-> MoveEventReceived");
        }
    }
}
