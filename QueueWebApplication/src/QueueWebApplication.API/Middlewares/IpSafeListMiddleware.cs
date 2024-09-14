using System.Net;
using System.Security.Claims;

namespace QueueWebApplication.API.Middlewares;

public sealed class IpSafeListMiddleware(
	RequestDelegate next,
	ILogger<IpSafeListMiddleware> logger)
{
	public async Task Invoke(HttpContext context)
	{
		var remoteIp = context.Connection.RemoteIpAddress;
		if (remoteIp is null)
		{
			logger.LogWarning(
				"Missing Remote IP address: {RemoteIp}", remoteIp);
			context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
			return;
		}

		logger.LogDebug("Request from Remote IP address: {RemoteIp}", remoteIp);

		if (context.User.Identity is not null)
		{
			context.User.AddIdentity(new ClaimsIdentity(new List<Claim>
			{
				new("ipaddress", remoteIp.ToString())
			}));
		}

		await next.Invoke(context);
	}
}
