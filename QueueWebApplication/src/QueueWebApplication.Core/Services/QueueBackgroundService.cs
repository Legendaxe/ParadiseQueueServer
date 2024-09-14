using System.Timers;
using Microsoft.Extensions.Hosting;
using QueueWebApplication.Core.Entities;
using QueueWebApplication.Core.Interfaces.Services;
using Timer = System.Timers.Timer;

namespace QueueWebApplication.Core.Services;

public sealed class QueueBackgroundService(IQueueService queueService) : BackgroundService
{
	private IQueueService QueueService { get; } = queueService;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
	        await QueueService.ProcessAllQueues();
            await Task.Delay(5000, stoppingToken);
        }
    }
}
