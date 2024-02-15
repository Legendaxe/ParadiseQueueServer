using System.Net;
using QueueWebApplication.Core.Interfaces.Entities;

namespace QueueWebApplication.Core.Interfaces.Services;

public interface IIpPassService
{
	public void AddPassToServer(IPAddress clientIpAddress, Uri serverUri);
	public void RemovePassFromServer(IPAddress clientIpAddress, Uri serverUri);
}
