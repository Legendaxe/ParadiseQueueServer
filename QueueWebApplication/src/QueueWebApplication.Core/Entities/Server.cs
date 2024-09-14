using System.Net;

namespace QueueWebApplication.Core.Entities;

public class Server
{
    public required string IpAddress { get; init; }
    public required ushort Port { get; init; }
    public int MaximumPlayers { get; set; } = 100;
    public int CurrentPlayers { get; set; } = 0;
}
