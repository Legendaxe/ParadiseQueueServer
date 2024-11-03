using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using QueueWebApplication.Core.Claims;
using QueueWebApplication.Core.Dtos;
using QueueWebApplication.Core.Interfaces.Services;

namespace QueueWebApplication.API.ApiHandlers;

public static class AuthHandlers
{
	public static async Task<IResult> LobbyConnect(
		string ckey,
		string ip,
		IQueueService queueService,
		IConfiguration configuration,
		IPlayersDbService playersDbService,
		IIpPassService ipPassService)
	{
		ipPassService.LinkIp(ckey, IPAddress.Parse(ip));
		var player = await playersDbService.GetPlayerInfo(ckey);
		// var player = new PlayerDto("legendaxe", "admin", 4, false, [4002, 5, 20]);
		var issuer = configuration["Jwt:Issuer"];
		var audience = configuration["Jwt:Audience"];
		var key = Encoding.ASCII.GetBytes
			(configuration["Jwt:Key"] ?? throw new ConfigurationErrorsException("JWT key not specified"));

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, player.Ckey),
				new Claim(JwtPlayerClaims.Role, player.Role),
				new Claim(JwtPlayerClaims.DonateTier, player.DonateTier.ToString(), ClaimValueTypes.Integer),
				new Claim(JwtPlayerClaims.Ban, player.Ban.ToString(), ClaimValueTypes.Boolean),
				new Claim(JwtPlayerClaims.WhitelistPasses, JsonSerializer.Serialize(player.WhitelistPasses), JsonClaimValueTypes.JsonArray)
			}),
			Expires = DateTime.UtcNow.AddMinutes(30),
			Issuer = issuer,
			Audience = audience,
			SigningCredentials = new SigningCredentials(
				new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
		};
		var tokenHandler = new JwtSecurityTokenHandler();
		var token = tokenHandler.CreateToken(tokenDescriptor);
		var stringToken = tokenHandler.WriteToken(token);
		return Results.Ok(stringToken);
	}
}
