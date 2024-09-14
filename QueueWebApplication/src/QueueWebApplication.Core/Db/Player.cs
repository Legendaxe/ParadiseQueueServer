namespace QueueWebApplication.Core.Db;

public partial class Player
{
    public int Id { get; set; }

    public string Ckey { get; set; } = null!;

    public DateTime Firstseen { get; set; }

    public DateTime Lastseen { get; set; }

    public string Ip { get; set; } = null!;

    public string Computerid { get; set; } = null!;

    public string Lastadminrank { get; set; } = null!;

    public string? Ooccolor { get; set; }

    public string? UiStyle { get; set; }

    public string? UiStyleColor { get; set; }

    public short? UiStyleAlpha { get; set; }

    public string? BeRole { get; set; }

    public short? DefaultSlot { get; set; }

    public int? Toggles { get; set; }

    public int? Toggles2 { get; set; }

    public int? Sound { get; set; }

    public string? VolumeMixer { get; set; }

    public string Lastchangelog { get; set; } = null!;

    public string? Exp { get; set; }

    public short? Clientfps { get; set; }

    public short? Atklog { get; set; }

    public long? Fuid { get; set; }

    public short? Fupdate { get; set; }

    public bool? Parallax { get; set; }

    public DateOnly? ByondDate { get; set; }

    public string? DiscordId { get; set; }

    public string? DiscordName { get; set; }

    public string? Keybindings { get; set; }

    public string Viewrange { get; set; } = null!;

    public bool? ScreentipMode { get; set; }

    public string ScreentipColor { get; set; } = null!;
}
