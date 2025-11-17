using UserAPI.Enums;
using UserAPI.Models;

namespace UserAPI.Interfaces;

public interface IJournalService
{
    Task<JournalModel?> CreateAsync(ExceptionType type, string data);

    Task<JournalRangeModel> GetRangeAsync(int skip, int take);

    Task<JournalModel?> GetSingleAsync(int id);
}
