using System.Timers;
using Microsoft.Extensions.Hosting;
using QueueWebApplication.Core.Entities;
using QueueWebApplication.Core.Interfaces.Services;
using Timer = System.Timers.Timer;

namespace QueueWebApplication.Core.Services;

public class QueueService : BackgroundService
{

    private readonly List<WaitingClient> _clientsQueue = new List<WaitingClient>();

    public void AddClientToQueue(WaitingClient clientToAdd)
    {
        for (int i = 0; i < _clientsQueue.Count; i++)
        {
            if (_clientsQueue[i].DonateTier < clientToAdd.DonateTier)
            {
                _clientsQueue.Insert(i, clientToAdd);
                return;
            }
        }
        
        _clientsQueue.Add(clientToAdd);
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
 
            await Task.Delay(5000, stoppingToken);
        }
    }
}