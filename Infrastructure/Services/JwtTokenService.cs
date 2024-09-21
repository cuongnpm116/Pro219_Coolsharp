using Application.IServices;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services;
internal sealed class JwtTokenService : ITokenService
{
    private const string SecretKey = "FTeJLDt3wY7zTvH4X0i8g5iPANBh6J4x";
    private const string Issuer = "CoolSharp";
    private const string Audience = "CoolSharp";
    private const int Expiration = 1; // hour

    public JwtTokenService()
    {
    }

    public string GenerateToken(Claim[] claims)
    {
        SymmetricSecurityKey key = new(Encoding.ASCII.GetBytes(SecretKey));
        SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(Expiration),
            signingCredentials: creds);

        JwtSecurityTokenHandler tokenHandler = new();
        string finalToken = tokenHandler.WriteToken(token);
        return finalToken;
    }
}
