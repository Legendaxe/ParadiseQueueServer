using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using QueueWebApplication.Core.Dtos;
using QueueWebApplication.Core.Helpers;
using QueueWebApplication.Core.Interfaces.Services;

namespace QueueWebApplication.Core.Hubs;

[Authorize]
public class QueueHub(
	IServerManagerService serverManagerService,
	ILogger<QueueHub> logger) : Hub<IQueueHub>
{
	private IServerManagerService ServerManagerService { get; } = serverManagerService;
	private ILogger<QueueHub> Logger { get; } = logger;

	public override async Task OnConnectedAsync()
	{
		var claims = Context.User?.Claims;
		if (claims is null)
		{
			Logger.LogWarning("Client connected without Authorization");
			await base.OnConnectedAsync();
			return;
		}
		var client = JwtHelpers.ParseWaitingClientFromClaims(claims);
		await Groups.AddToGroupAsync(Context.ConnectionId, client.Ckey);
		await Clients.Caller.PendingServersInitData(ServerManagerService.GetServersInitDtos());
		await base.OnConnectedAsync();
	}

	public override async Task OnDisconnectedAsync(Exception? exception)
	{
		var claims = Context.User?.Claims;
		if (claims is null)
		{
			Logger.LogWarning("Client disconnected without Authorization");
			await base.OnConnectedAsync();
			return;
		}
		var client = JwtHelpers.ParseWaitingClientFromClaims(claims);
		await Groups.RemoveFromGroupAsync(Context.ConnectionId,client.Ckey);
		await base.OnDisconnectedAsync(exception);
	}
}

public interface IQueueHub
{
	Task PendingQueuePosition(QueuePositionDto queuePosition);
	Task PendingServersStatusData(IEnumerable<ServerStatusDto> servers);
	Task PendingServersInitData(IEnumerable<ServerInitDto> servers);
}
