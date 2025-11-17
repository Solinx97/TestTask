using UserAPI.Enums;
using UserAPI.Models;

namespace UserAPI.Tests.Factory;

internal static class JournalTestData
{
    public static List<JournalModel> CreateJournals()
    {
        var journals = new List<JournalModel>
        {
            new()
            {
                Id = 1,
                CreateAt = DateTime.Now.AddHours(1),
                Data = "{\"message\": \"You have to delete all children nodes\r\nfirst\"}",
                Type = ExceptionType.Secure
            },
            new()
            {
                Id = 2,
                CreateAt = DateTime.Now.AddHours(2),
                Data = "{\"message\": \"You have to delete all children nodes\r\nfirst\"}",
                Type = ExceptionType.Secure
            },
            new()
            {
                Id = 3,
                CreateAt = DateTime.Now.AddHours(3),
                Data = "{\"message\": \"You have to delete all children nodes\r\nfirst\"}",
                Type = ExceptionType.Secure
            },
            new()
            {
                Id = 4,
                CreateAt = DateTime.Now.AddHours(4),
                Data = "{\"message\": \"You have to delete all children nodes\r\nfirst\"}",
                Type = ExceptionType.Secure
            }
        };

        return journals;
    }
}