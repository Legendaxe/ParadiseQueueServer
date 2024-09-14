using System.Timers;
using Microsoft.Extensions.Hosting;
using QueueWebApplication.Core.Entities;
using QueueWebApplication.Core.Interfaces.Services;
using Timer = System.Timers.Timer;

namespace QueueWebApplication.Core.Services;

public sealed class FetchPlayersBackgroundService(IPlayersDictionariesService playersDictionariesService) : BackgroundService
{
	private IPlayersDictionariesService PlayersDictionariesService { get; } = playersDictionariesService;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
	        await PlayersDictionariesService.UpdateAllPlayersDictionaries();
            await Task.Delay(10000, stoppingToken);
        }
    }
}
