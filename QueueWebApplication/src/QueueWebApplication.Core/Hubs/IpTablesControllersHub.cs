using System.Net;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using QueueWebApplication.Core.Collections;
using QueueWebApplication.Core.Dtos.Messages;
using QueueWebApplication.Core.Services;
using Action = QueueWebApplication.Core.Dtos.Messages.Action;

namespace QueueWebApplication.Core.Hubs;

public sealed class IpTablesControllersHub(ILogger<IpTablesControllersHub> logger) : Hub<IIptablesControllersHub>
{
	private static readonly ConnectionMapping<IPAddress> Connections = new();
	public static IEnumerable<string> GetConnections(IPAddress serverIp) => Connections.GetConnections(serverIp);

	public override async Task OnConnectedAsync()
	{
		var ipAddress = Context.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;

		if (ipAddress is null)
		{
			logger.LogWarning("Connected without RemoteIpAddress");
			await base.OnConnectedAsync();
			return;
		}

		await Groups.AddToGroupAsync(Context.ConnectionId, ipAddress.ToString());
		await base.OnConnectedAsync();
	}

	public override async Task OnDisconnectedAsync(Exception? exception)
	{
		var ipAddress = Context.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;

		if (ipAddress is null)
		{
			logger.LogWarning("Disconnected without RemoteIpAddress");
			await base.OnConnectedAsync();
			return;
		}

		await Groups.RemoveFromGroupAsync(Context.ConnectionId, ipAddress.ToString());
		await base.OnDisconnectedAsync(exception);
	}
}

public interface IIptablesControllersHub
{
	Task Event(EventMessage message);
}


