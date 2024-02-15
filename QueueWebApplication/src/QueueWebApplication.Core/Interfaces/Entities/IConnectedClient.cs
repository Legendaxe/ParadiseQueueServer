using System.Net;
using Microsoft.AspNetCore.SignalR;

namespace QueueWebApplication.Core.Interfaces.Entities;

public interface IConnectedClient
{
	public IPAddress IpAddress { get; set; }
	public string? Ckey { get; set; }

	public bool IsByondConnected();
}
