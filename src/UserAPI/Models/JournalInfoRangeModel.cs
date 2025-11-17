namespace UserAPI.Models;

public class JournalInfoRangeModel
{
    public int Skip { get; set; }

    public int Count { get; set; }

    public JournalInfoModel[] Items { get; set; }
}