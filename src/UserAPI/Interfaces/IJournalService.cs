using UserAPI.Enums;
using UserAPI.Models;

namespace UserAPI.Interfaces;

/// <summary>
/// Contract for creating and retrieving Journals
/// </summary>
public interface IJournalService
{
    /// <summary>
    /// Create a new Journal
    /// </summary>
    /// <param name="type">Type of exception</param>
    /// <param name="data">Descripe of exception</param>
    /// <returns>Created Journal</returns>
    Task<JournalModel?> CreateAsync(ExceptionType type, string data);

    /// <summary>
    /// Get Journals by range
    /// </summary>
    /// <param name="skip">Count how many elements should be skip before get</param>
    /// <param name="take">How many elements get</param>
    /// <returns>JournalRange where reflected how many elements skip, how many get and Journal collection</returns>
    Task<JournalRangeModel> GetRangeAsync(int skip, int take);

    /// <summary>
    /// Get only one Journal by id
    /// </summary>
    /// <param name="id">Unique ID, reflected which Journal we wanna</param>
    /// <returns>Journal by provided ID</returns>
    Task<JournalModel?> GetSingleAsync(int id);
}
