using QueueWebApplication.Core.DTOs;
using QueueWebApplication.Core.Entities;

namespace QueueWebApplication.Core.Interfaces.Services;

public interface IByondApiService
{
    public ValueTask<bool> IsClientAllowedToServer(WaitingClientDto clientDto, Server server, CancellationToken cancellationToken = default);
    public ValueTask<int> AvailableSlotsOnServer(Server server, CancellationToken cancellationToken = default);
    public ValueTask<IEnumerable<string>> GetPlayersList(Server server, CancellationToken cancellationToken = default);
}
