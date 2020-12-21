using CTB.Shared;
using CTB.Shared.Interfaces;
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
    public class IndexBase : ComponentBase, IDisposable
    {
        [Inject] 
        private HttpClient Http { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject] 
        private IJSRuntime JSRuntime { get; set; }

        public string WelcomeVisibility { get => _welcomeVisibility; set => _welcomeVisibility = value; }

        protected ElementReference _canvas;
        protected GameEngine _gameEngine = new();
        private string _welcomeVisibility = ElementVisibility.None;

        private DotNetObjectReference<IndexBase> _selfRef;
        private HubConnection _hubConnection;

        protected override async Task OnInitializedAsync()
        {
            _gameEngine.SetExecuteDraw(async (game) => await JSRuntime.InvokeAsync<string>("CTB.draw", game));
            _gameEngine.SetExecutePlayerUpdated(async (position) => await _hubConnection.InvokeAsync(HubConstants.MoveEventMethod, position));

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/GameHub"))
                .Build();

            _hubConnection.On(HubConstants.PlayerNameEventMethod, (string name) =>
            {
                NamePlayerEventReceived(name);
                StateHasChanged();
            });

            _hubConnection.On(HubConstants.MoveEventMethod, (Monkey monkey) =>
            {
                MoveEventReceived(monkey);
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
                _selfRef = DotNetObjectReference.Create(this);
                var playerID = await JSRuntime.InvokeAsync<string>("CTB.initialize", _canvas, _selfRef);
                _gameEngine.SetPlayerID(playerID);
                await _hubConnection.InvokeAsync(HubConstants.PlayerIDEventMethod, playerID);
            }
        }

        private void NamePlayerEventReceived(string name)
        {
            Console.WriteLine($"-> NamePlayerEventReceived: {name}");
            _gameEngine.SetPlayerName(name);

            WelcomeVisibility = ElementVisibility.Inline;
        }

        private void MoveEventReceived(Monkey monkey)
        {
            Console.WriteLine("-> MoveEventReceived");
            Console.WriteLine(monkey);
        }

        protected async Task CanvasOnClick(MouseEventArgs eventArgs)
        {
            Console.WriteLine("-> CanvasOnClick");
            await Task.CompletedTask;
        }

        protected void LetsPlayOnClick()
        {
            Console.WriteLine("-> LetsPlayOnClick");
            WelcomeVisibility = ElementVisibility.None;
        }


        [JSInvokable]
        public async void CanvasKeyDown(int keyCode)
        {
            Console.WriteLine($"-> CanvasKeyDown: {keyCode}");
            _gameEngine.OnKeyDown(keyCode);
            await Task.CompletedTask;
        }

        [JSInvokable]
        public async void CanvasKeyUp(int keyCode)
        {
            Console.WriteLine($"-> CanvasKeyUp: {keyCode}");
            _gameEngine.OnKeyUp(keyCode);
            await Task.CompletedTask;
        }


        [JSInvokable]
        public async void GameUpdate(double delta)
        {
            _gameEngine.Update(delta);
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            _selfRef?.Dispose();
        }
    }
}
