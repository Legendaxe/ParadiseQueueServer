using QueueWebApplication.Core.Interfaces.Services;

namespace QueueWebApplication.Core.Services;

public sealed class PlayersDictionariesService : IPlayersDictionariesService
{
	private readonly Dictionary<string, Dictionary<string, DateTime>> _playersDictionaries = [];
	private IServerManagerService ServerManagerService { get; }
	private IIpPassService IpPassService { get; }

	public PlayersDictionariesService(IServerManagerService serverManagerService, IIpPassService ipPassService)
	{
		ServerManagerService = serverManagerService;
		IpPassService = ipPassService;
		foreach (var serverName in ServerManagerService.GetServersNames())
		{
			_playersDictionaries.Add(serverName, []);
		}
	}

	public IEnumerable<KeyValuePair<string, DateTime>> GetPlayersDictionary(string serverName)
	{
		if (!_playersDictionaries.TryGetValue(serverName, out var playersDictionary))
		{
			throw new KeyNotFoundException();
		}
		return playersDictionary;
	}

	public void AddPlayerFromQueue(string ckey, string serverName)
	{
		if (!_playersDictionaries.TryGetValue(serverName, out var playersDictionary))
		{
			throw new KeyNotFoundException();
		}
		playersDictionary[ckey] = DateTime.Now;
	}

	public Task UpdateAllPlayersDictionaries()
		=> Task.WhenAll(_playersDictionaries.Keys.Select(UpdatePlayersDictionary));


	private async Task UpdatePlayersDictionary(string serverName)
	{
		var presentPlayers = await ServerManagerService.GetPlayersList(serverName);
		if (!_playersDictionaries.TryGetValue(serverName, out var playersDictionary))
		{
			throw new KeyNotFoundException();
		}

		foreach (var player in presentPlayers)
		{
			playersDictionary[player] = DateTime.Now;
		}

		var timeToKick = DateTime.Now - TimeSpan.FromMinutes(10);
		var server = ServerManagerService.GetServer(serverName);

		foreach (var player in playersDictionary.Where(player => player.Value <= timeToKick))
		{
			playersDictionary.Remove(player.Key);
			await IpPassService.RemovePassFromServer(player.Key, server);
		}
	}
}
