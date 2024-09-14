using Microsoft.EntityFrameworkCore;

namespace QueueWebApplication.Core.Db;

public partial class ParadiseDbContext : DbContext
{
    public ParadiseDbContext()
    {
    }

    public ParadiseDbContext(DbContextOptions<ParadiseDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; init; }

    public virtual DbSet<Ban> Bans { get; init; }

    public virtual DbSet<Player> Players { get; init; }

    public virtual DbSet<Budget> Budgets { get; init; }

    public virtual DbSet<CkeyWhitelist> CkeyWhitelists { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("admin")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.Ckey, "ckey");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Ckey)
                .HasMaxLength(32)
                .HasColumnName("ckey")
                .UseCollation("utf8mb4_unicode_ci");
            entity.Property(e => e.Flags)
                .HasColumnType("int(16)")
                .HasColumnName("flags");
            entity.Property(e => e.Level)
                .HasColumnType("int(2)")
                .HasColumnName("level");
            entity.Property(e => e.Rank)
                .HasMaxLength(32)
                .HasDefaultValueSql("'Administrator'")
                .HasColumnName("admin_rank")
                .UseCollation("utf8mb4_unicode_ci");
        });

        modelBuilder.Entity<Ban>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("ban")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.Ckey, "ckey");

            entity.HasIndex(e => e.Computerid, "computerid");

            entity.HasIndex(e => e.Ip, "ip");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.ACkey)
                .HasMaxLength(32)
                .HasColumnName("a_ckey");
            entity.Property(e => e.AComputerid)
                .HasMaxLength(32)
                .HasColumnName("a_computerid");
            entity.Property(e => e.AIp)
                .HasMaxLength(32)
                .HasColumnName("a_ip");
            entity.Property(e => e.Adminwho)
                .HasColumnType("mediumtext")
                .HasColumnName("adminwho");
            entity.Property(e => e.Bantime)
                .HasColumnType("datetime")
                .HasColumnName("bantime");
            entity.Property(e => e.Bantype)
                .HasMaxLength(32)
                .HasColumnName("bantype");
            entity.Property(e => e.Ckey)
                .HasMaxLength(32)
                .HasColumnName("ckey");
            entity.Property(e => e.Computerid)
                .HasMaxLength(32)
                .HasColumnName("computerid");
            entity.Property(e => e.Duration)
                .HasColumnType("int(11)")
                .HasColumnName("duration");
            entity.Property(e => e.Edits)
                .HasColumnType("mediumtext")
                .HasColumnName("edits");
            entity.Property(e => e.ExpirationTime)
                .HasColumnType("datetime")
                .HasColumnName("expiration_time");
            entity.Property(e => e.Ip)
                .HasMaxLength(32)
                .HasColumnName("ip");
            entity.Property(e => e.Job)
                .HasMaxLength(32)
                .HasColumnName("job");
            entity.Property(e => e.Reason)
                .HasColumnType("mediumtext")
                .HasColumnName("reason");
            entity.Property(e => e.Rounds)
                .HasColumnType("int(11)")
                .HasColumnName("rounds");
            entity.Property(e => e.Serverip)
                .HasMaxLength(32)
                .HasColumnName("serverip");
            entity.Property(e => e.Unbanned).HasColumnName("unbanned");
            entity.Property(e => e.UnbannedCkey)
                .HasMaxLength(32)
                .HasColumnName("unbanned_ckey");
            entity.Property(e => e.UnbannedComputerid)
                .HasMaxLength(32)
                .HasColumnName("unbanned_computerid");
            entity.Property(e => e.UnbannedDatetime)
                .HasColumnType("datetime")
                .HasColumnName("unbanned_datetime");
            entity.Property(e => e.UnbannedIp)
                .HasMaxLength(32)
                .HasColumnName("unbanned_ip");
            entity.Property(e => e.Who)
                .HasColumnType("mediumtext")
                .HasColumnName("who");
        });

        modelBuilder.Entity<Budget>(entity =>
        {
	        entity.HasKey(e => e.Id).HasName("PRIMARY");

	        entity.ToTable("budget");

	        entity.Property(e => e.Id)
		        .HasColumnType("int(11)")
		        .HasColumnName("id");
	        entity.Property(e => e.Amount)
		        .HasColumnType("int(10) unsigned")
		        .HasColumnName("amount");
	        entity.Property(e => e.Ckey)
		        .HasMaxLength(32)
		        .HasColumnName("ckey")
		        .UseCollation("utf8mb4_unicode_ci");
	        entity.Property(e => e.Date)
		        .HasDefaultValueSql("current_timestamp()")
		        .HasColumnType("datetime")
		        .HasColumnName("date");
	        entity.Property(e => e.DateEnd)
		        .HasDefaultValueSql("(current_timestamp() + interval 1 month)")
		        .HasColumnType("datetime")
		        .HasColumnName("date_end");
	        entity.Property(e => e.DateStart)
		        .HasDefaultValueSql("current_timestamp()")
		        .HasColumnType("datetime")
		        .HasColumnName("date_start");
	        entity.Property(e => e.DiscordId)
		        .HasColumnType("bigint(20)")
		        .HasColumnName("discord_id");
	        entity.Property(e => e.IsValid)
		        .IsRequired()
		        .HasDefaultValueSql("'1'")
		        .HasColumnName("is_valid");
	        entity.Property(e => e.Source)
		        .HasMaxLength(32)
		        .HasColumnName("source");
        });

        modelBuilder.Entity<CkeyWhitelist>(entity =>
        {
	        entity.HasKey(e => e.Id).HasName("PRIMARY");

	        entity.ToTable("ckey_whitelist");

	        entity.Property(e => e.Id)
		        .HasColumnType("int(11)")
		        .HasColumnName("id");
	        entity.Property(e => e.Adminwho)
		        .HasMaxLength(32)
		        .HasColumnName("adminwho");
	        entity.Property(e => e.Ckey)
		        .HasMaxLength(32)
		        .HasColumnName("ckey")
		        .UseCollation("utf8mb4_unicode_ci");
	        entity.Property(e => e.Date)
		        .HasDefaultValueSql("current_timestamp()")
		        .HasColumnType("datetime")
		        .HasColumnName("date");
	        entity.Property(e => e.DateEnd)
		        .HasColumnType("datetime")
		        .HasColumnName("date_end");
	        entity.Property(e => e.DateStart)
		        .HasDefaultValueSql("current_timestamp()")
		        .HasColumnType("datetime")
		        .HasColumnName("date_start");
	        entity.Property(e => e.IsValid)
		        .IsRequired()
		        .HasDefaultValueSql("'1'")
		        .HasColumnName("is_valid");
	        entity.Property(e => e.Port)
		        .HasColumnType("int(5) unsigned")
		        .HasColumnName("port");
        });


        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("player")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.Ckey, "ckey").IsUnique();

            entity.HasIndex(e => e.Computerid, "computerid");

            entity.HasIndex(e => e.Fuid, "fuid");

            entity.HasIndex(e => e.Fupdate, "fupdate");

            entity.HasIndex(e => e.Ip, "ip");

            entity.HasIndex(e => e.Lastseen, "lastseen");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Atklog)
                .HasDefaultValueSql("'0'")
                .HasColumnType("smallint(4)")
                .HasColumnName("atklog");
            entity.Property(e => e.BeRole).HasColumnName("be_role");
            entity.Property(e => e.ByondDate).HasColumnName("byond_date");
            entity.Property(e => e.Ckey)
                .HasMaxLength(32)
                .HasColumnName("ckey");
            entity.Property(e => e.Clientfps)
                .HasDefaultValueSql("'0'")
                .HasColumnType("smallint(4)")
                .HasColumnName("clientfps");
            entity.Property(e => e.Computerid)
                .HasMaxLength(32)
                .HasColumnName("computerid");
            entity.Property(e => e.DefaultSlot)
                .HasDefaultValueSql("'1'")
                .HasColumnType("smallint(4)")
                .HasColumnName("default_slot");
            entity.Property(e => e.DiscordId)
                .HasMaxLength(32)
                .HasColumnName("discord_id");
            entity.Property(e => e.DiscordName)
                .HasMaxLength(32)
                .HasColumnName("discord_name");
            entity.Property(e => e.Exp).HasColumnName("exp");
            entity.Property(e => e.Firstseen)
                .HasColumnType("datetime")
                .HasColumnName("firstseen");
            entity.Property(e => e.Fuid)
                .HasColumnType("bigint(20)")
                .HasColumnName("fuid");
            entity.Property(e => e.Fupdate)
                .HasDefaultValueSql("'0'")
                .HasColumnType("smallint(4)")
                .HasColumnName("fupdate");
            entity.Property(e => e.Ip)
                .HasMaxLength(18)
                .HasColumnName("ip");
            entity.Property(e => e.Keybindings).HasColumnName("keybindings");
            entity.Property(e => e.Lastadminrank)
                .HasMaxLength(32)
                .HasDefaultValueSql("'Player'")
                .HasColumnName("lastadminrank");
            entity.Property(e => e.Lastchangelog)
                .HasMaxLength(32)
                .HasDefaultValueSql("'0'")
                .HasColumnName("lastchangelog");
            entity.Property(e => e.Lastseen)
                .HasColumnType("datetime")
                .HasColumnName("lastseen");
            entity.Property(e => e.Ooccolor)
                .HasMaxLength(7)
                .HasDefaultValueSql("'#b82e00'")
                .HasColumnName("ooccolor");
            entity.Property(e => e.Parallax)
                .HasDefaultValueSql("'8'")
                .HasColumnName("parallax");
            entity.Property(e => e.ScreentipColor)
                .HasMaxLength(7)
                .HasDefaultValueSql("'#ffd391'")
                .HasColumnName("screentip_color");
            entity.Property(e => e.ScreentipMode)
                .HasDefaultValueSql("'8'")
                .HasColumnName("screentip_mode");
            entity.Property(e => e.Sound)
                .HasDefaultValueSql("'31'")
                .HasColumnType("mediumint(8)")
                .HasColumnName("sound");
            entity.Property(e => e.Toggles)
                .HasColumnType("int(11)")
                .HasColumnName("toggles");
            entity.Property(e => e.Toggles2)
                .HasColumnType("int(11)")
                .HasColumnName("toggles_2");
            entity.Property(e => e.UiStyle)
                .HasMaxLength(10)
                .HasDefaultValueSql("'Midnight'")
                .HasColumnName("UI_style");
            entity.Property(e => e.UiStyleAlpha)
                .HasDefaultValueSql("'255'")
                .HasColumnType("smallint(4)")
                .HasColumnName("UI_style_alpha");
            entity.Property(e => e.UiStyleColor)
                .HasMaxLength(7)
                .HasDefaultValueSql("'#ffffff'")
                .HasColumnName("UI_style_color");
            entity.Property(e => e.Viewrange)
                .HasMaxLength(5)
                .HasDefaultValueSql("'19x15'")
                .HasColumnName("viewrange");
            entity.Property(e => e.VolumeMixer).HasColumnName("volume_mixer");
        });
    }
}
