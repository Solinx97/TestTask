using UserAPI.Models;

namespace UserAPI.Interfaces;

/// <summary>
/// Contracts for creating and retrieving Users
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Create a new User by unique code
    /// </summary>
    /// <param name="code">A unique code identifying the User</param>
    /// <returns>Created User</returns>
    Task<UserModel?> CreateAsync(string code);

    /// <summary>
    /// Get User by unque code
    /// </summary>
    /// <param name="code">>A unique code identifying the User</param>
    /// <returns>Found User. If no any User - create a new User</returns>
    Task<UserModel> GetAsync(string code);
}
