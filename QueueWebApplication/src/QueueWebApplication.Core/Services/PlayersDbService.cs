using System.Collections;
using Microsoft.EntityFrameworkCore;
using QueueWebApplication.Core.Db;
using QueueWebApplication.Core.Dtos;
using QueueWebApplication.Core.Interfaces.Services;

namespace QueueWebApplication.Core.Services;

public sealed class PlayersDbService (IDbContextFactory<ParadiseDbContext> paradiseDbContextFactory) : IPlayersDbService
{
	private IDbContextFactory<ParadiseDbContext> ParadiseDbContextFactory { get; } = paradiseDbContextFactory;
	private static readonly string[] BanTypes  = ["ADMIN_TEMPBAN", "TEMPBAN", "ADMIN_PERMABAN", "PERMABAN"];
	private const string FakeRole = "Removed";


	public async Task<bool> IsBanned(string ckey)
	{
		await using var db = await ParadiseDbContextFactory.CreateDbContextAsync();
		return await IsBanned(ckey, db);
	}

	public async Task<int> GetDonateTier(string ckey)
	{
		await using var db = await ParadiseDbContextFactory.CreateDbContextAsync();
		return AmountToTier(await GetDonateTier(ckey, db));
	}

	public async Task<IEnumerable<int>> GetWhitelistPasses(string ckey)
	{
		await using var db = await ParadiseDbContextFactory.CreateDbContextAsync();
		return await GetWhitelistPasses(ckey, db);
	}

	public async Task<string?> GetRole(string ckey)
	{
		await using var db = await ParadiseDbContextFactory.CreateDbContextAsync();
		return await GetRole(ckey, db);
	}

	public async Task<PlayerDto> GetPlayerInfo(string ckey)
	{
		await using var db = await ParadiseDbContextFactory.CreateDbContextAsync();
		return new PlayerDto(
			ckey,
			await GetRole(ckey, db) ?? String.Empty,
			AmountToTier(await GetDonateTier(ckey, db)),
			await IsBanned(ckey, db),
			(await GetWhitelistPasses(ckey, db)).ToArray()
		);
	}
	private static Task<bool> IsBanned(string ckey, ParadiseDbContext db)
		=> db.Bans.AnyAsync(ban =>
			ban.Ckey == ckey &&
			BanTypes.Contains(ban.Bantype) &&
			ban.ExpirationTime > DateTime.Now &&
			ban.Unbanned == null);
	private static Task<uint> GetDonateTier(string ckey, ParadiseDbContext db)
		=> db.Budgets
			.Where(budget => budget.Ckey == ckey && budget.IsValid == true)
			.Select(budget => budget.Amount)
			.FirstOrDefaultAsync();
	private static Task<IEnumerable<int>> GetWhitelistPasses(string ckey, ParadiseDbContext db)
		=> db.CkeyWhitelists
			.Where(whitelist =>
				whitelist.Ckey == ckey &&
				whitelist.DateStart <= DateTime.Now &&
				(whitelist.DateEnd >= DateTime.Now || whitelist.DateEnd == null) &&
				whitelist.IsValid == true)
			.Select(whitelist => (int) whitelist.Port)
			.Distinct()
			.ToListAsync()
			.ContinueWith( x => x.Result as IEnumerable<int>, TaskContinuationOptions.OnlyOnRanToCompletion);

	private static Task<string?> GetRole(string ckey, ParadiseDbContext db)
		=> db.Admins.Where(admin =>
			admin.Ckey == ckey && !admin.Rank.Contains(FakeRole))
			.Select(admin => admin.Rank)
			.FirstOrDefaultAsync();

	private static int AmountToTier(uint amount)
		=> amount switch
		{
			0 => 0,
			<= 220 => 1,
			<= 440 => 2,
			<= 1000 => 3,
			<= 2200 => 4,
			_ => 5
		};

}
