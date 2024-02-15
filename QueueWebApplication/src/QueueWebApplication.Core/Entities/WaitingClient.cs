using System.Net;

namespace QueueWebApplication.Core.Entities;

public record WaitingClient()
{
    public required IPAddress ClientIp { get; init; }
    public required string Ckey { get; init; }
    public required IPAddress TargetIp { get; init; }
    public required int DonateTier{ get; init; }
    // public int OrderInQueue { get; set; }
}
