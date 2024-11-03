using QueueWebApplication.Core.Dtos;
using QueueWebApplication.Core.Entities;
using QueueWebApplication.Core.Helpers;

namespace QueueWebApplication.Core.Interfaces.Services;

public interface IQueueService
{
    public Task ProcessAllQueues();
    public ValueTask<QueueApiResult> AddPlayerToQueue(PlayerDto playerToAdd, string serverName);
    public ValueTask RemovePlayerFromQueue(PlayerDto playerToRemove, string serverName);
    public Task AddPassToServer(string ckey, string serverName);
}
