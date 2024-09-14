namespace QueueWebApplication.Core.Db;

public partial class Ban
{
    public int Id { get; set; }

    public DateTime Bantime { get; set; }

    public string Serverip { get; set; } = null!;

    public string Bantype { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public string? Job { get; set; }

    public int Duration { get; set; }

    public int? Rounds { get; set; }

    public DateTime ExpirationTime { get; set; }

    public string Ckey { get; set; } = null!;

    public string Computerid { get; set; } = null!;

    public string Ip { get; set; } = null!;

    public string ACkey { get; set; } = null!;

    public string AComputerid { get; set; } = null!;

    public string AIp { get; set; } = null!;

    public string Who { get; set; } = null!;

    public string Adminwho { get; set; } = null!;

    public string? Edits { get; set; }

    public bool? Unbanned { get; set; }

    public DateTime? UnbannedDatetime { get; set; }

    public string? UnbannedCkey { get; set; }

    public string? UnbannedComputerid { get; set; }

    public string? UnbannedIp { get; set; }
}
