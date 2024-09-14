using System.Net;
using QueueWebApplication.Core.Entities;
using QueueWebApplication.Core.Interfaces.Entities;

namespace QueueWebApplication.Core.Interfaces.Services;

public interface IIpPassService
{
	public Task AddPassToServer(string clientCkey, Server server);
	public Task RemovePassFromServer(string clientCkey, Server server);
	public Task<IEnumerable<IPAddress>> GetIps(string ckey);
	public void LinkIp(string ckey, IPAddress ip);
}
