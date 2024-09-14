using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using QueueWebApplication.API.Claims;
using QueueWebApplication.Core.DTOs;

using QueueWebApplication.Core.Interfaces.Services;

namespace QueueWebApplication.API.ApiHandlers;

public static class ClientsQueue
{

	[Authorize]
	public static IResult AddClient(string serverName, ClaimsPrincipal user, IQueueService queueService)
	{
		try
		{
			queueService.AddClientToQueue(ParseClientFromClaims(user.Claims), serverName);
			return Results.Ok();
		}
		catch (Exception)
		{
			return Results.BadRequest("Failed");
		}
	}

	[Authorize]
	public static IResult RemoveClient(string serverName, ClaimsPrincipal user, IQueueService queueService)
	{
		try
		{
			queueService.RemoveClientFromQueue(ParseClientFromClaims(user.Claims), serverName);
			return Results.Ok();
		}
		catch (Exception)
		{
			return Results.BadRequest("Failed");
		}
	}

	private static WaitingClientDto ParseClientFromClaims(IEnumerable<Claim> claims)
	{
		var ckey = claims
			.Where(claim => claim.Type == JwtRegisteredClaimNames.Sub)
			.Select(claim => claim.Value).SingleOrDefault();
		var donateTier = claims
			.Where(claim => claim.Type == JwtPlayerClaims.DonateTier)
			.Select(claim => int.Parse(claim.Value)).SingleOrDefault();

		return new WaitingClientDto() { Ckey = ckey ?? throw new HttpRequestException("broken jwt"), DonateTier = donateTier };
	}
}
