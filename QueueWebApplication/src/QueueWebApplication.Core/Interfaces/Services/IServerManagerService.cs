using QueueWebApplication.Core.Dtos;
using QueueWebApplication.Core.Entities;

namespace QueueWebApplication.Core.Interfaces.Services;

public interface IServerManagerService
{
	public IEnumerable<KeyValuePair<string, Server>> GetServers();
	public Server GetServer(string serverName);
	public IEnumerable<string> GetServersNames();
	public ValueTask<int> GetAvailableSlots(string serverName);
	public ValueTask<IEnumerable<string>> GetPlayersList(string serverName);
	public IEnumerable<ServerInitDto> GetServersInitDtos();
	public IEnumerable<ServerStatusDto> GetServersStatusDtos();
	public Task UpdateAllServers();
}
