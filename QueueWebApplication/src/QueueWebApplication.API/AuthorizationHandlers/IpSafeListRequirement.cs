using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace QueueWebApplication.API.AuthorizationHandlers;

public sealed class IpCheckRequirement : IAuthorizationRequirement
{
	public bool IpClaimRequired { get; set; } = true;
}

public sealed class IpCheckHandler : AuthorizationHandler<IpCheckRequirement>
{
	private readonly string[] _safeList;

	public IpCheckHandler(IConfiguration configuration)
	{
		var safeList = configuration["IpSafeList"] ?? "";
		_safeList = safeList.Split(';');
	}

	protected override Task HandleRequirementAsync(
		AuthorizationHandlerContext context, IpCheckRequirement checkRequirement)
	{
		var ipAddressClaim = context.User.FindFirst(
			c => c.Type == "ipaddress");

		if (ipAddressClaim is null)
		{
			return Task.CompletedTask;
		}
		if (_safeList.Contains(ipAddressClaim.Value))
		{
			context.Succeed(checkRequirement);
			return Task.CompletedTask;
		}
		context.Fail();
		return Task.CompletedTask;
	}
}
