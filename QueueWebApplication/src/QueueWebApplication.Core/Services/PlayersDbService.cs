using Microsoft.EntityFrameworkCore;
using QueueWebApplication.Core.Db;
using QueueWebApplication.Core.DTOs;
using QueueWebApplication.Core.Interfaces.Services;

namespace QueueWebApplication.Core.Services;

public sealed class PlayersDbService (IDbContextFactory<ParadiseDbContext> paradiseDbContextFactory) : IPlayersDbService
{
	private IDbContextFactory<ParadiseDbContext> ParadiseDbContextFactory { get; } = paradiseDbContextFactory;
	private static readonly string[] BanTypes  = ["ADMIN_TEMPBAN", "TEMPBAN", "ADMIN_PERMABAN", "PERMABAN"];
	private const string FakeRole = "Removed";

	private static int AmountToTier(uint amount)
	{
		return amount switch
		{
			0 => 0,
			<= 220 => 1,
			<= 440 => 2,
			<= 1000 => 3,
			<= 2200 => 4,
			_ => 5
		};
	}

	public async Task<bool> IsBanned(string ckey)
	{
		await using var db = await ParadiseDbContextFactory.CreateDbContextAsync();
		return await db.Bans
			.AnyAsync(ban =>
			ban.Ckey == ckey &&
			BanTypes.Contains(ban.Bantype) &&
			ban.ExpirationTime > DateTime.Now &&
			ban.Unbanned == null);
	}

	public async Task<int> GetDonateTier(string ckey)
	{
		await using var db = await ParadiseDbContextFactory.CreateDbContextAsync();
		var amount = await db.Budgets
			.Where(budget => budget.Ckey == ckey && budget.IsValid == true)
			.Select(budget => budget.Amount)
			.FirstOrDefaultAsync();
		return AmountToTier(amount);
	}
	public async Task<IEnumerable<int>> GetWhitelistPasses(string ckey)
	{
		await using var db = await ParadiseDbContextFactory.CreateDbContextAsync();
		return await db.CkeyWhitelists
			.Where(whitelist =>
			whitelist.Ckey == ckey &&
			whitelist.DateStart <= DateTime.Now &&
			(whitelist.DateEnd >= DateTime.Now || whitelist.DateEnd == null) &&
			whitelist.IsValid == true).Select(whitelist => (int) whitelist.Port).Distinct().ToListAsync();
	}

	public async Task<string?> GetRole(string ckey)
	{
		await using var db = await ParadiseDbContextFactory.CreateDbContextAsync();
		return await db.Admins.Where(admin =>
			admin.Ckey == ckey &&
			!admin.Rank.Contains(FakeRole)).Select(admin => admin.Rank).FirstOrDefaultAsync();
	}

	public async Task<PlayerDto> GetPlayerInfo(string ckey)
	{
		var donateTierTask = GetDonateTier(ckey);
		var isBannedTask = IsBanned(ckey);
		var whitelistPassesTask = GetWhitelistPasses(ckey);
		var roleTask = GetRole(ckey);

		await Task.WhenAll(donateTierTask, roleTask, whitelistPassesTask, roleTask);

		return new PlayerDto(Ckey: ckey, Role: await roleTask ?? string.Empty, DonateTier: await donateTierTask,
			Ban: await isBannedTask, WhitelistPasses: (await whitelistPassesTask).ToArray());
	}
}
