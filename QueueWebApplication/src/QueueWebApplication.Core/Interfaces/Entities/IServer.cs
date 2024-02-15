using System.Net;

namespace QueueWebApplication.Core.Interfaces.Entities;

public interface IServer
{
	public Uri ServerUri { get; set; }
	public int CurrentPlayers { get; set; }
	public int MaximumPlayers { get; set; }
}
