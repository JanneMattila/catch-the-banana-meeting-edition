using CTB.Client.Logic;
using CTB.Shared;
using CTB.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
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

        public string WelcomeVisibility { get; set; } = ElementVisibility.None;

        protected ElementReference _canvas;
        protected GameEngineClient _gameEngine = new();

        private DotNetObjectReference<IndexBase> _selfRef;
        private HubConnection _hubConnection;

        protected override async Task OnInitializedAsync()
        {
            _gameEngine.SetExecuteDraw(async (game) => await JSRuntime.InvokeAsync<string>("CTB.draw", game));
            _gameEngine.SetExecutePlayerUpdated(async (position) =>
            {
                if (_hubConnection.State == HubConnectionState.Connected)
                {
                    Console.WriteLine($"Sending position {position}");
                    await _hubConnection.InvokeAsync(HubConstants.MoveMonkeyEventMethod, position);
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

            _hubConnection.On(HubConstants.MoveMonkeyEventMethod, (Monkey monkey) =>
            {
                _gameEngine.MonkeyUpdate(monkey);
                StateHasChanged();
            });

            _hubConnection.On(HubConstants.MonkeyConnectedEventMethod, (Monkey monkey) =>
            {
                _gameEngine.MonkeyUpdate(monkey);
                StateHasChanged();
            });

            _hubConnection.On(HubConstants.MonkeyDisconnectedEventMethod, (Monkey monkey) =>
            {
                MonkeyDisconnected(monkey);
                StateHasChanged();
            });

            _hubConnection.On(HubConstants.MoveBananaEventMethod, (Banana banana) =>
            {
                _gameEngine.BananaUpdate(banana);
                StateHasChanged();
            });
            _hubConnection.On(HubConstants.DeleteBananaEventMethod, (string id, List<string> monkeyPoints) =>
            {
                _gameEngine.BananaDelete(id, monkeyPoints);
                StateHasChanged();
            });

            _hubConnection.On(HubConstants.MoveSharkEventMethod, (Shark shark) =>
            {
                _gameEngine.SharkUpdate(shark);
                StateHasChanged();
            });
            _hubConnection.On(HubConstants.DeleteSharkEventMethod, (string id) =>
            {
                _gameEngine.SharkDelete(id);
                StateHasChanged();
            });

            await _hubConnection.StartAsync();

            await base.OnInitializedAsync();
        }

        private void MonkeyDisconnected(Monkey monkey)
        {
            _gameEngine.MonkeyDelete(monkey);
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

        protected async Task CanvasOnClick(MouseEventArgs eventArgs)
        {
            await Task.CompletedTask;
        }

        protected void LetsPlayOnClick()
        {
            WelcomeVisibility = ElementVisibility.None;
        }

        [JSInvokable]
        public async void ClearInput()
        {
            _gameEngine.OnClearInput();
            await Task.CompletedTask;
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
