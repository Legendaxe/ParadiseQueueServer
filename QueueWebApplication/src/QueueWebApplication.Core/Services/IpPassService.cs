using System.Net;
using Microsoft.AspNetCore.SignalR;
using QueueWebApplication.Core.Hubs;
using QueueWebApplication.Core.Interfaces.Entities;
using QueueWebApplication.Core.Interfaces.Services;

namespace QueueWebApplication.Core.Services;

public sealed class IpPassService(IHubContext<IpTablesControllersHub> iptablesHubContext) : IIpPassService
{
	private IHubContext<IpTablesControllersHub> _hubContext = iptablesHubContext;

	public void AddPassToServer(IPAddress clientIpAddress, Uri serverUri)
	{
		throw new NotImplementedException();
	}

	public void RemovePassFromServer(IPAddress clientIpAddress, Uri serverUri)
	{
		throw new NotImplementedException();
	}
}
