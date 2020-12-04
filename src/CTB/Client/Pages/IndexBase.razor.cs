using CTB.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
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

        [Inject] 
        private IJSRuntime JSRuntime { get; set; }

        protected string PlayerName { get => _playerName; set => _playerName = value; }
        public string WelcomeVisibility { get => _welcomeVisibility; set => _welcomeVisibility = value; }

        protected ElementReference _canvas;
        private string _playerName;
        private string _welcomeVisibility = "none";

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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("CTB.initialize", _canvas);
            }
        }

        private void NamePlayerEventReceived(string name)
        {
            Console.WriteLine($"-> NamePlayerEventReceived: {name}");
            PlayerName = name;
            WelcomeVisibility = "inline";
        }

        private void MoveEventReceived()
        {
            Console.WriteLine("-> MoveEventReceived");
        }

        protected async Task CanvasOnClick(MouseEventArgs eventArgs)
        {
            Console.WriteLine("-> CanvasOnClick");

            await JSRuntime.InvokeVoidAsync("CTB.draw", eventArgs /* not real game object */);
        }

        protected void LetsPlayOnClick()
        {
            Console.WriteLine("-> LetsPlayOnClick");
            WelcomeVisibility = "none";
            WelcomeVisibility = "none";
        }
    }
}
