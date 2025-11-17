namespace UserAPI.Models;

public class JournalModel
{
    public int Id { get; set; }

    public string Text { get; set; }

    public int EventId { get; set; }

    public DateTimeOffset CreateAt { get; set; }
}
