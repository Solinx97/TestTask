using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserAPI.Consts;
using UserAPI.Interfaces;

namespace UserAPI.Services;

internal class AuthService(IOptions<JWT> jwtOptions) : IAuthService
{
    private readonly JWT _jwt = jwtOptions.Value;

    public string GenerateJwtToken(string code)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, "ApiUser"),
            new Claim("code", code),
            new Claim("scope", "api")
        };

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: null,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        var jwtSecurityToken = new JwtSecurityTokenHandler().WriteToken(token);

        return jwtSecurityToken;
    }
}
