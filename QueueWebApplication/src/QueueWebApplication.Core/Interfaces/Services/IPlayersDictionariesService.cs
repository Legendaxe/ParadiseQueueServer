namespace QueueWebApplication.Core.Interfaces.Services;

public interface IPlayersDictionariesService
{
	public IEnumerable<KeyValuePair<string, DateTime>> GetPlayersDictionary(string serverName);
	public void AddPlayerFromQueue(string ckey, string serverName);
	public Task UpdateAllPlayersDictionaries();
}
