using QueueWebApplication.Core.Dtos;
using QueueWebApplication.Core.Interfaces.Services;

namespace QueueWebApplication.Core.Services;

public class QueueStorageService : IQueueStorageService
{

	private readonly Dictionary<string, List<PlayerDto>> _serversQueue = [];

    public QueueStorageService(IServerManagerService serversManagerService)
    {
	    foreach (var serverName in serversManagerService.GetServersNames())
	    {
		    _serversQueue.Add(serverName, []);
	    }
    }

    public ValueTask<int> AddPlayerToQueue(PlayerDto playerToAdd, string serverName)
    {
	    var queue = _serversQueue[serverName];
        for (var i = 0; i < queue.Count; i++)
        {
	        if (queue[i].DonateTier >= playerToAdd.DonateTier) continue;
	        queue.Insert(i, playerToAdd);
	        return ValueTask.FromResult(i);
        }
        queue.Add(playerToAdd);
        return ValueTask.FromResult(queue.Count - 1);
    }

    public ValueTask<IEnumerable<PlayerDto>> GetPlayersInQueue(string serverName)
	    => ValueTask.FromResult<IEnumerable<PlayerDto>>(_serversQueue[serverName]);


    public ValueTask<PlayerDto> PopPlayerFromQueue(string serverName)
    {
	    var result = _serversQueue[serverName][0];
	    if (result is null)
	    {
		    throw new InvalidOperationException("The queue is empty");
	    }
	    _serversQueue[serverName].RemoveAt(0);
	    return ValueTask.FromResult(result);
    }

    public ValueTask RemovePlayerFromQueue(PlayerDto playerToRemove, string serverName)
    {
	    _serversQueue[serverName].Remove(playerToRemove);
	    return default;
    }

    public ValueTask<int> Count(string serverName)
    {
	    return ValueTask.FromResult(_serversQueue[serverName].Count);
    }

    public ValueTask<bool> IsQueueEmpty(string serverName)
    {
	    return ValueTask.FromResult(_serversQueue[serverName].Count == 0);
    }

    public ValueTask<bool> Contains(PlayerDto playerDto, string serverName)
    {
	    var queue = _serversQueue[serverName];

	    return ValueTask.FromResult(queue.Contains(playerDto));
    }
}
