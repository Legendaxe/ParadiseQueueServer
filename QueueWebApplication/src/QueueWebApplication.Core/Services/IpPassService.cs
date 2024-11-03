using System.Net;
using Microsoft.AspNetCore.SignalR;
using QueueWebApplication.Core.Entities;
using QueueWebApplication.Core.Hubs;
using QueueWebApplication.Core.Interfaces.Services;
using QueueWebApplication.Core.Dtos.Messages;
using Action = QueueWebApplication.Core.Dtos.Messages.Action;

namespace QueueWebApplication.Core.Services;

public sealed class IpPassService(IHubContext<IpTablesControllersHub, IIptablesControllersHub> iptablesHubContext) : IIpPassService
{
	private readonly Dictionary<string, Dictionary<IPAddress, DateTime>> _ckeyIpLink = [];
	private readonly Dictionary<IPAddress, HashSet<Server>> _passesDictionary = [];
	private IHubContext<IpTablesControllersHub, IIptablesControllersHub> IptablesHubContext { get; } = iptablesHubContext;

	public void LinkIp(string ckey, IPAddress ip)
	{
		if (!_ckeyIpLink.ContainsKey(ckey))
		{
			_ckeyIpLink[ckey] = [];
		}

		_ckeyIpLink[ckey][ip] = DateTime.Now;
	}

	public async Task<IEnumerable<IPAddress>> GetIps(string ckey)
	{
		if (!_ckeyIpLink.TryGetValue(ckey, out var ipData))
		{
			return [];
		}
		await RemoveExpiredIps(ckey);
		return ipData.Select(ip => ip.Key);
	}

	private async Task AddPassToServer(IPAddress clientIpAddress, Server server)
	{
		if (!_passesDictionary.ContainsKey(clientIpAddress))
		{
			_passesDictionary[clientIpAddress] = [];
		}

		_passesDictionary[clientIpAddress].Add(server);

		await IptablesHubContext.Clients.Group(server.IpTablesDaemonIpAddress).Event(new EventMessage(Action.Allow, clientIpAddress, server.Port));
	}

	private async Task RemovePassFromServer(IPAddress clientIpAddress, Server server)
	{
		_passesDictionary[clientIpAddress].Remove(server);
		await IptablesHubContext.Clients.Group(server.IpTablesDaemonIpAddress).Event(new EventMessage(Action.Revoke, clientIpAddress, server.Port));
	}

	public async Task AddPassToServer(string clientCkey, Server server)
	{
		foreach (var ip in await GetIps(clientCkey)) await AddPassToServer(ip, server);
	}

	public async Task RemovePassFromServer(string clientCkey, Server server)
	{
		foreach (var ip in await GetIps(clientCkey)) await RemovePassFromServer(ip, server);
	}
	private async Task RemovePassesForIp(IPAddress ip)
	{
		if (!_passesDictionary.TryGetValue(ip, out var servers))
		{
			return;
		}

		foreach (var server in servers)
		{
			await RemovePassFromServer(ip, server);
		}
	}

	private async Task RemoveExpiredIps(string ckey)
	{
		foreach (var ipToRemove in _ckeyIpLink[ckey]
			         .Where(ipDate => ipDate.Value < DateTime.Now - TimeSpan.FromDays(1)))
		{
			await RemovePassesForIp(ipToRemove.Key);
			_ckeyIpLink[ckey].Remove(ipToRemove.Key);
		}
	}

}
