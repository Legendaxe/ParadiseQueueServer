using System.Net;

namespace QueueWebApplication.Core.DTOs;

public record WaitingClientDto
{
    public required string Ckey { get; init; }
    public required int DonateTier { get; init; }
}
