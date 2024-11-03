using QueueWebApplication.Core.Dtos;

namespace QueueWebApplication.Core.Interfaces.Services;

public interface IQueueStorageService
{
	public ValueTask<int> AddPlayerToQueue(PlayerDto playerToAdd, string serverName);
	public ValueTask<IEnumerable<PlayerDto>> GetPlayersInQueue(string serverName);
	public ValueTask<PlayerDto> PopPlayerFromQueue(string serverName);
	public ValueTask RemovePlayerFromQueue(PlayerDto playerToRemove, string serverName);
	public ValueTask<bool> Contains(PlayerDto playerDto, string serverName);
	public ValueTask<int> Count(string serverName);
	public ValueTask<bool> IsQueueEmpty(string serverName);
}
