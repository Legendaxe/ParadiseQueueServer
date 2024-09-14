namespace QueueWebApplication.Core.Db;

public partial class Budget
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public string? Ckey { get; set; }

    public uint Amount { get; set; }

    public string Source { get; set; } = null!;

    public DateTime DateStart { get; set; }

    public DateTime? DateEnd { get; set; }

    public bool? IsValid { get; set; }

    public long? DiscordId { get; set; }
}
