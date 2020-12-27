using CTB.Server.Logic;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CTB.Server.Services
{
    public class GameEngineBackgroundService : BackgroundService
    {
        private readonly ILogger<GameEngineBackgroundService> _logger;
        private readonly IGameEngineServer _gameEngine;

        public GameEngineBackgroundService(ILogger<GameEngineBackgroundService> logger, IGameEngineServer gameEngine)
        {
            _logger = logger;
            _gameEngine = gameEngine;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var stopWatch = Stopwatch.StartNew();
            var update = stopWatch.Elapsed.Ticks;

            while (!stoppingToken.IsCancellationRequested)
            {
                var timestamp = stopWatch.Elapsed.Ticks;
                var delta = (timestamp - update) / (double)TimeSpan.TicksPerSecond;

                _logger.LogDebug(LoggingEvents.GameEngineBackgroundServiceDebug, $"Delta: {delta}");

                var updates = _gameEngine.Update(delta);
                update = timestamp;

                if (updates == 0)
                {
                    // Wait for players to join
                    await Task.Delay(100, stoppingToken);
                }
                else
                {
                    // Run game engine all the time.
                    await Task.CompletedTask;
                }
            }
        }
    }
}
