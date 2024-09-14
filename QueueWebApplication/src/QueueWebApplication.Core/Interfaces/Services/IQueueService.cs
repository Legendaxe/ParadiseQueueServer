using QueueWebApplication.Core.DTOs;
using QueueWebApplication.Core.Entities;

namespace QueueWebApplication.Core.Interfaces.Services;

public interface IQueueService
{
    public Task ProcessAllQueues();
    public void AddClientToQueue(WaitingClientDto clientDtoToAdd, string serverName);
    public void RemoveClientFromQueue(WaitingClientDto clientDtoToRemove, string serverName);
}
