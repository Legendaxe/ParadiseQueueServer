using System.Configuration;
using Microsoft.Extensions.Configuration;
using QueueWebApplication.Core.Entities;
using QueueWebApplication.Core.Interfaces.Services;

namespace QueueWebApplication.Core.Services;

public sealed class ServerManagerService(IByondApiService byondApiService, IConfiguration configuration)
	: IServerManagerService
{
	private readonly Dictionary<string, Server> _servers =
		configuration.GetSection("Servers").Get<Dictionary<string, Server>>() ??
		throw new ConfigurationErrorsException("There is no servers data in configuration");
	private IByondApiService ByondApiService { get; } = byondApiService;

	public IEnumerable<KeyValuePair<string, Server>> GetServers() => _servers;

	public Server GetServer(string serverName)
	{
		if (!_servers.TryGetValue(serverName, out var server))
		{
			throw new KeyNotFoundException();
		}

		return server;
	}

	public IEnumerable<string> GetServersNames() => _servers.Keys;

	public ValueTask<int> GetAvailableSlots(string serverName)
	{

		if (!_servers.TryGetValue(serverName, out var server))
		{
			throw new KeyNotFoundException();
		}
		return ByondApiService.AvailableSlotsOnServer(server);
	}

	public ValueTask<IEnumerable<string>> GetPlayersList(string serverName)
	{
		if (!_servers.TryGetValue(serverName, out var server))
		{
			throw new KeyNotFoundException();
		}
		return ByondApiService.GetPlayersList(server);
	}
}

