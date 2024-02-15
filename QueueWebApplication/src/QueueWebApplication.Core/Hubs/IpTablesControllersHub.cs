using System.Net;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;
using QueueWebApplication.Core.Entities;
using QueueWebApplication.Core.Messages;
using Action = QueueWebApplication.Core.Messages.Action;

namespace QueueWebApplication.Core.Hubs;

public sealed class IpTablesControllersHub : Hub
{
	private static readonly ConnectionMapping<IPAddress> _connections =
		new ConnectionMapping<IPAddress>();

	public override Task OnConnectedAsync()
	{
		var feature = Context.Features.Get<IHttpConnectionFeature>();
		if (feature is null || feature.RemoteIpAddress is null)
		{
			return base.OnConnectedAsync();
		}
		_connections.Add(feature.RemoteIpAddress, Context.ConnectionId);

		return base.OnConnectedAsync();
	}

	public override Task OnDisconnectedAsync(Exception? exception)
	{
		var feature = Context.Features.Get<IHttpConnectionFeature>();
		if (feature is null || feature.RemoteIpAddress is null)
		{
			return base.OnDisconnectedAsync(exception);
		}
		_connections.Remove(feature.RemoteIpAddress, Context.ConnectionId);
		return base.OnDisconnectedAsync(exception);
	}

	public void AddPassToPort(IPAddress serverIp, IPAddress whom, int port)
	{
		foreach (var connection in _connections.GetConnections(serverIp))
		{
			Clients.Client(connection).SendAsync("event", new EventMessage(Action.Allow, whom, port));
		}
	}

	public void RemovePassToPort(IPAddress serverIp, IPAddress whom, int port)
	{
		foreach (var connection in _connections.GetConnections(serverIp))
		{
			Clients.Client(connection).SendAsync("event", new EventMessage(Action.Revoke, whom, port));
		}
	}
}


