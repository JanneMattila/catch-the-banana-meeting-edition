using CTB.Shared;
using CTB.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using System;
using System.Net.Http;
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
            _gameEngine.SetExecutePlayerUpdated(async (position) =>
            {
                if (_hubConnection.State == HubConnectionState.Connected)
                {
                    await _hubConnection.InvokeAsync(HubConstants.MoveEventMethod, position);
                }
            });

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/GameHub"))
                .Build();

            _hubConnection.On(HubConstants.PlayerRegisteredEventMethod, (Monkey monkey) =>
            {
                PlayerRegisteredEventMethod(monkey);
                StateHasChanged();
            });

            _hubConnection.On(HubConstants.MoveEventMethod, (Monkey monkey) =>
            {
                MoveEventReceived(monkey);
                StateHasChanged();
            });

            _hubConnection.On(HubConstants.MonkeyConnectedEventMethod, (Monkey monkey) =>
            {
                MonkeyConnected(monkey);
                StateHasChanged();
            });

            _hubConnection.On(HubConstants.MonkeyDisconnectedEventMethod, (Monkey monkey) =>
            {
                MonkeyDisconnected(monkey);
                StateHasChanged();
            });

            await _hubConnection.StartAsync();

            await base.OnInitializedAsync();
        }
        private void MonkeyConnected(Monkey monkey)
        {
            _gameEngine.OtherPlayerConnected(monkey);
        }

        private void MonkeyDisconnected(Monkey monkey)
        {
            _gameEngine.OtherPlayerDisconnected(monkey);
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

        private void PlayerRegisteredEventMethod(Monkey monkey)
        {
            _gameEngine.SetPlayer(monkey);

            WelcomeVisibility = ElementVisibility.Inline;
        }

        private void MoveEventReceived(Monkey monkey)
        {
            _gameEngine.OtherPlayerUpdate(monkey);
        }

        protected async Task CanvasOnClick(MouseEventArgs eventArgs)
        {
            await Task.CompletedTask;
        }

        protected void LetsPlayOnClick()
        {
            WelcomeVisibility = ElementVisibility.None;
        }

        [JSInvokable]
        public async void CanvasKeyDown(int keyCode)
        {
            _gameEngine.OnKeyDown(keyCode);
            await Task.CompletedTask;
        }

        [JSInvokable]
        public async void CanvasKeyUp(int keyCode)
        {
            _gameEngine.OnKeyUp(keyCode);
            await Task.CompletedTask;
        }

        [JSInvokable]
        public async void CanvasTouch(CanvasTouch leftTouchStart, CanvasTouch leftTouchCurrent, CanvasTouch rightTouchCurrent)
        {
            _gameEngine.OnCanvasTouch(leftTouchStart, leftTouchCurrent, rightTouchCurrent);
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
