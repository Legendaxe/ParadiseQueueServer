using System.Net;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;
using QueueWebApplication.Core.Collections;
using QueueWebApplication.Core.DTOs.Messages;
using QueueWebApplication.Core.Entities;
using Action = QueueWebApplication.Core.DTOs.Messages.Action;

namespace QueueWebApplication.Core.Hubs;

public sealed class IpTablesControllersHub : Hub
{
	private static readonly ConnectionMapping<IPAddress> Connections = new();

	public override Task OnConnectedAsync()
	{
		var feature = Context.Features.Get<IHttpConnectionFeature>();
		if (feature?.RemoteIpAddress is null)
		{
			return base.OnConnectedAsync();
		}
		Connections.Add(feature.RemoteIpAddress, Context.ConnectionId);
		return base.OnConnectedAsync();
	}

	public override Task OnDisconnectedAsync(Exception? exception)
	{
		var feature = Context.Features.Get<IHttpConnectionFeature>();
		if (feature?.RemoteIpAddress is null)
		{
			return base.OnDisconnectedAsync(exception);
		}
		Connections.Remove(feature.RemoteIpAddress, Context.ConnectionId);
		return base.OnDisconnectedAsync(exception);
	}

	public async void AddPassToPort(IPAddress serverIp, IPAddress whom, int port)
	{
		foreach (var connection in Connections.GetConnections(serverIp))
		{
			await Clients.Client(connection).SendAsync("event", new EventMessage(Action.Allow, whom, port));
		}
	}

	public async void RemovePassToPort(IPAddress serverIp, IPAddress whom, int port)
	{
		foreach (var connection in Connections.GetConnections(serverIp))
		{
			await Clients.Client(connection).SendAsync("event", new EventMessage(Action.Revoke, whom, port));
		}
	}
}


