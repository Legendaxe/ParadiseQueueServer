namespace QueueWebApplication.Core.Db;

public partial class Admin
{
    public int Id { get; set; }

    public string Ckey { get; set; } = null!;

    public string Rank { get; set; } = null!;

    public int Level { get; set; }

    public int Flags { get; set; }
}
