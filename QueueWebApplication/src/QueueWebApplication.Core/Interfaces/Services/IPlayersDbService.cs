using QueueWebApplication.Core.Dtos;

namespace QueueWebApplication.Core.Interfaces.Services;

public interface IPlayersDbService
{
	public Task<bool> IsBanned(string ckey);
	public Task<int> GetDonateTier(string ckey);
	public Task<string?> GetRole(string ckey);
	public Task<IEnumerable<int>> GetWhitelistPasses(string ckey);
	public Task<PlayerDto> GetPlayerInfo(string ckey);
}
