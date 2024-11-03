using Microsoft.AspNetCore.SignalR;
using QueueWebApplication.Core.Dtos;
using QueueWebApplication.Core.Entities;
using QueueWebApplication.Core.Helpers;
using QueueWebApplication.Core.Hubs;
using QueueWebApplication.Core.Interfaces.Services;
using static System.String;

namespace QueueWebApplication.Core.Services;

public sealed class QueueService(
	IServerManagerService serversManagerService,
	IPlayersDictionariesService playersDictionariesService,
	IIpPassService ipPassService,
	IHubContext<QueueHub, IQueueHub> queueHubContext,
	IQueueStorageService queueStorageService)
	: IQueueService
{
    private IServerManagerService ServersManagerService { get; } = serversManagerService;
    private IPlayersDictionariesService PlayersDictionariesService { get; } = playersDictionariesService;
    private IIpPassService IpPassService { get; } = ipPassService;
    private IHubContext<QueueHub, IQueueHub> QueueHubContext { get; } = queueHubContext;
    private IQueueStorageService QueueStorageService { get; } = queueStorageService;

    public async Task ProcessAllQueues()
    {
	    await Task.WhenAll(ServersManagerService.GetServers()
		    .Select(server => ProcessServerQueue(server.Key, server.Value)));
    }

    public async ValueTask<QueueApiResult> AddPlayerToQueue(PlayerDto playerToAdd, string serverName)
    {
	    var server = ServersManagerService.GetServer(serverName);

	    if (server.MaximumPlayers == -1)
	    {
		    return QueueApiResult.BypassedQueue;
	    }

	    if (await QueueStorageService.Contains(playerToAdd, serverName))
	    {
		    return QueueApiResult.AlreadyInQueue;
	    }

	    if (IsDisallowedToQueue(playerToAdd, server))
	    {
		    return QueueApiResult.Rejected;
	    }

	    if (CanBypassQueue(playerToAdd))
	    {
		    await AddPassToServer(playerToAdd.Ckey, serverName, server);
		    return QueueApiResult.BypassedQueue;
	    }

	    var queuePosition = await QueueStorageService.AddPlayerToQueue(playerToAdd, serverName);
        await QueueHubContext.Clients.Group(playerToAdd.Ckey).PendingQueuePosition(new QueuePositionDto(serverName, queuePosition));
        return QueueApiResult.AddedToQueue;
    }

    public async ValueTask RemovePlayerFromQueue(PlayerDto playerToRemove, string serverName)
    {
	    await QueueStorageService.RemovePlayerFromQueue(playerToRemove, serverName);
    }

    public async Task AddPassToServer(string ckey, string serverName)
    {
	    await AddPassToServer(ckey, serverName, ServersManagerService.GetServer(serverName));
    }
    private async Task AddPassToServer(string ckey, string serverName, Server server)
    {
	    await IpPassService.AddPassToServer(ckey, server);
	    PlayersDictionariesService.AddPlayerFromQueue(ckey, serverName);
	    await QueueHubContext.Clients.Group(ckey).PendingQueuePosition(new QueuePositionDto(serverName, -1));
    }
    private async Task ProcessServerQueue(string serverName, Server server)
    {
	    if (await QueueStorageService.IsQueueEmpty(serverName))
	    {
		    return;
	    }

	    var availableSlots = server.MaximumPlayers - server.CurrentPlayers;

	    for (var i = 0; i < availableSlots - await QueueStorageService.Count(serverName); i++)
	    {
		    if (await QueueStorageService.IsQueueEmpty(serverName))
		    {
			    break;
		    }

		    var player = await QueueStorageService.PopPlayerFromQueue(serverName);
		    await AddPassToServer(player.Ckey, serverName, server);
	    }

	    await SendQueuePosition(serverName);
    }

    private async Task SendQueuePosition(string serverName)
    {
	    var playersInQueue = (await QueueStorageService.GetPlayersInQueue(serverName)).ToArray();
	    for (var i = 0; i < playersInQueue.Length; i++)
	    {
		    await QueueHubContext.Clients.Group(playersInQueue[i].Ckey).PendingQueuePosition(new QueuePositionDto(serverName, i));
	    }
    }

    private static bool IsDisallowedToQueue(PlayerDto player, Server server)
    {
	    if (server.Whitelisted && !player.WhitelistPasses.Contains(server.Port))
	    {
		    return true;
	    }

	    return player.Ban;
    }

    private static bool CanBypassQueue(PlayerDto player)
	    => !IsNullOrEmpty(player.Role);
}
