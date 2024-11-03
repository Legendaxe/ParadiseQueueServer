using System.Security.Claims;
using System.Text.Json;
using Microsoft.IdentityModel.JsonWebTokens;
using QueueWebApplication.Core.Claims;
using QueueWebApplication.Core.Dtos;

namespace QueueWebApplication.Core.Helpers;

public static class JwtHelpers
{
	public static WaitingClientDto ParseWaitingClientFromClaims(IEnumerable<Claim> claims)
	{
		var enumerable = claims as Claim[] ?? claims.ToArray();
		var ckey = enumerable
			.Where(claim => claim.Type == JwtRegisteredClaimNames.Sub)
			.Select(claim => claim.Value).SingleOrDefault();
		var donateTier = enumerable
			.Where(claim => claim.Type == JwtPlayerClaims.DonateTier)
			.Select(claim => int.Parse(claim.Value)).SingleOrDefault();

		return new WaitingClientDto{ Ckey = ckey ?? throw new HttpRequestException("broken jwt"), DonateTier = donateTier };
	}
	public static PlayerDto ParsePlayerFromClaims(IEnumerable<Claim> claims)
	{
		string ckey = "", role = "";
		int donateTier = 0;
		bool ban = false;
		List<int> whitelistPasses = [];
		foreach (var claim in claims)
		{
			switch (claim.Type)
			{
				case JwtRegisteredClaimNames.Sub:
					ckey = claim.Value;
					break;
				case JwtPlayerClaims.Role:
					role = claim.Value;
					break;
				case JwtPlayerClaims.DonateTier:
					donateTier = int.Parse(claim.Value);
					break;
				case JwtPlayerClaims.Ban:
					ban = bool.Parse(claim.Value);
					break;
				case JwtPlayerClaims.WhitelistPasses:
					whitelistPasses.Add(int.Parse(claim.Value));
					break;
			}
		}

		return new PlayerDto(ckey, role, donateTier, ban, whitelistPasses.ToArray());
	}
}
