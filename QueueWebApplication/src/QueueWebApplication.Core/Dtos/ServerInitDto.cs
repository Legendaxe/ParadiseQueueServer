namespace QueueWebApplication.Core.Dtos;

public record ServerInitDto(string Name, string IpAddress, int Port, int CurrentPlayers, int MaximumPlayers, bool Whitelisted);


