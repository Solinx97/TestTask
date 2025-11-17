namespace UserAPI.Models;

public class JournalInfoModel
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public DateTimeOffset CreateAt { get; set; }
}