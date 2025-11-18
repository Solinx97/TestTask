namespace UserAPI.Interfaces;

/// <summary>
/// Contract for work with JWT for Authenthication/Authorization in ASP
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Generate new JWT token by unique by User code
    /// </summary>
    /// <param name="code">A unique code identifying the user</param>
    /// <returns>Generated JWT token</returns>
    string GenerateJwtToken(string code);
}
