namespace UserAPI.Interfaces;

public interface IAuthService
{
    string GenerateJwtToken(string code);
}
