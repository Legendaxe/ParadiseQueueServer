using System.Timers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using QueueWebApplication.Core.Entities;
using QueueWebApplication.Core.Hubs;
using QueueWebApplication.Core.Interfaces.Services;
using Timer = System.Timers.Timer;

namespace QueueWebApplication.Core.Services;

public sealed class QueueBackgroundService(
	IQueueService queueService,
	IServerManagerService serverManagerService,
	IHubContext<QueueHub, IQueueHub> queueHubContext) : BackgroundService
{
	private IQueueService QueueService { get; } = queueService;
	private IServerManagerService ServerManagerService { get; } = serverManagerService;
	private IHubContext<QueueHub, IQueueHub> QueueHubContext { get; } = queueHubContext;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
	        await ServerManagerService.UpdateAllServers();
	        await QueueHubContext.Clients.All.PendingServersStatusData(ServerManagerService.GetServersStatusDtos());
	        await QueueService.ProcessAllQueues();
            await Task.Delay(20000, stoppingToken);
        }
    }
}
