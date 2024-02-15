using System.Net;

namespace QueueWebApplication.API.Middlewares;

public class IpSafeListMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<IpSafeListMiddleware> _logger;
	private readonly byte[][] _safeList;

	public IpSafeListMiddleware(
		RequestDelegate next,
		ILogger<IpSafeListMiddleware> logger,
		string safeList)
	{
		var ips = safeList.Split(';');
		_safeList = new byte[ips.Length][];
		for (var i = 0; i < ips.Length; i++)
		{
			_safeList[i] = IPAddress.Parse(ips[i]).GetAddressBytes();
		}

		_next = next;
		_logger = logger;
	}

	public async Task Invoke(HttpContext context)
	{
		var remoteIp = context.Connection.RemoteIpAddress;

		if (remoteIp is null)
		{
			_logger.LogWarning(
				"Missing Remote IP address: {RemoteIp}", remoteIp);
			context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
			return;
		}

		_logger.LogDebug("Request from Remote IP address: {RemoteIp}", remoteIp);

		var addressBytes = remoteIp.GetAddressBytes();
		
		var badIp = true;
		foreach (var address in _safeList)
		{
			if (address.SequenceEqual(addressBytes))
			{
				badIp = false;
				break;
			}
		}

		if (badIp)
		{
			_logger.LogWarning(
				"Request from uncertified Remote IP address: {RemoteIp}", remoteIp);
			context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
			return;
		}

		await _next.Invoke(context);
	}
}
