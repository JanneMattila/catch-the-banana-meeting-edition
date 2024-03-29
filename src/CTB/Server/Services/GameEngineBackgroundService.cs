﻿using CTB.Server.Logic;
using CTB.Server.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CTB.Server.Services;

public class GameEngineBackgroundService : BackgroundService
{
    private readonly ILogger<GameEngineBackgroundService> _logger;
    private readonly IGameEngineServer _gameEngine;
    private readonly IOptions<CTBOptions> _options;

    public GameEngineBackgroundService(ILogger<GameEngineBackgroundService> logger, IGameEngineServer gameEngine, IOptions<CTBOptions> options)
    {
        _logger = logger;
        _gameEngine = gameEngine;
        _options = options;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        const double SPEED_CONSTANT = 100d;

        var stopWatch = Stopwatch.StartNew();
        var update = stopWatch.Elapsed.Ticks;

        while (!stoppingToken.IsCancellationRequested)
        {
            var timestamp = stopWatch.Elapsed.Ticks;
            var delta = SPEED_CONSTANT * (timestamp - update) / TimeSpan.TicksPerSecond;

            _logger.LogTrace(LoggingEvents.GameEngineBackgroundServiceDebug, $"Delta: {delta}");

            var updates = await _gameEngine.UpdateAsync(delta);
            update = timestamp;

            if (updates)
            {
                // Run game engine all the time.
                if (_options.Value.GameEngineDelay.HasValue)
                {
                    if (_options.Value.GameEngineDelay.Value > 0)
                    {
                        await Task.Delay(_options.Value.GameEngineDelay.Value, stoppingToken);
                    }
                }
                else
                {
                    // In reality this delay is ~20ms which is okay for game like this
                    await Task.Delay(1, stoppingToken);
                }
            }
            else
            {
                // Wait for players to join
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
