namespace QueueWebApplication.Core.Db;

public partial class CkeyWhitelist
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public string Ckey { get; set; } = null!;

    public string Adminwho { get; set; } = null!;

    public uint Port { get; set; }

    public DateTime DateStart { get; set; }

    public DateTime? DateEnd { get; set; }

    public bool? IsValid { get; set; }
}
