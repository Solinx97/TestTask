using UserAPI.Enums;

namespace UserAPI.Models;

public class JournalModel
{
    public int Id { get; set; }

    public ExceptionType Type { get; set; }

    public string Data { get; set; }

    public DateTimeOffset CreateAt { get; set; }
}
