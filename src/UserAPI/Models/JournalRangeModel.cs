namespace UserAPI.Models;

public class JournalRangeModel
{
    public int Skip { get; set; }

    public int Count { get; set; }

    public JournalModel[] Items { get; set; }
}