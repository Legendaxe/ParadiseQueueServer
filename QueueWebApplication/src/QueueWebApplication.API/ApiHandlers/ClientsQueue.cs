using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using QueueWebApplication.Core.Helpers;
using QueueWebApplication.Core.Interfaces.Services;

namespace QueueWebApplication.API.ApiHandlers;

public static class ClientsQueue
{

	[Authorize]
	public static async Task<IResult> AddClient(string serverName, ClaimsPrincipal user, IQueueService queueService)
	{
		try
		{
			var result = await queueService.AddPlayerToQueue(JwtHelpers.ParsePlayerFromClaims(user.Claims), serverName);
			return Results.Ok(result);
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
			queueService.RemovePlayerFromQueue(JwtHelpers.ParsePlayerFromClaims(user.Claims), serverName);
			return Results.Ok();
		}
		catch (Exception)
		{
			return Results.BadRequest("Failed");
		}
	}
}
