using QueueWebApplication.Core.DTOs;
using QueueWebApplication.Core.Entities;
using QueueWebApplication.Core.Interfaces.Services;

namespace QueueWebApplication.Core.Services;

public sealed class QueueService : IQueueService
{
	private readonly Dictionary<string, List<WaitingClientDto>> _serversQueue = [];
    private IServerManagerService ServersManagerService { get; }
    private IIpPassService IpPassService { get; }

    public QueueService(IServerManagerService serversManagerService, IIpPassService ipPassService)
    {
	    ServersManagerService = serversManagerService;
	    IpPassService = ipPassService;
	    foreach (var serverName in ServersManagerService.GetServersNames())
	    {
		    _serversQueue.Add(serverName, []);
	    }
    }

    private async Task ProcessServerQueue(string serverName, Server server)
    {
	    var queue = _serversQueue[serverName];
	    if (queue.Count == 0)
	    {
		    return;
	    }

	    var availableSlots = await ServersManagerService.GetAvailableSlots(serverName);

	    for (var i = 0; i < availableSlots; i++)
	    {
		    if (queue.Count == 0)
		    {
			    break;
		    }
		    await IpPassService.AddPassToServer(queue[0].Ckey, server);
		    RemoveClientFromQueue(queue[0], serverName);
	    }
    }

    public async Task ProcessAllQueues()
    {
	    await Task.WhenAll(ServersManagerService.GetServers()
		    .Select(server => ProcessServerQueue(server.Key, server.Value)));
    }

    public void AddClientToQueue(WaitingClientDto clientDtoToAdd, string serverName)
    {
	    var queue = _serversQueue[serverName];
        for (var i = 0; i < queue.Count; i++)
        {
	        if (queue[i].DonateTier >= clientDtoToAdd.DonateTier) continue;
	        queue.Insert(i, clientDtoToAdd);
	        return;
        }

        queue.Add(clientDtoToAdd);
    }

    public void RemoveClientFromQueue(WaitingClientDto clientDtoToRemove, string serverName)
		=> _serversQueue[serverName].Remove(clientDtoToRemove);

}
