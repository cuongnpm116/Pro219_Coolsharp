using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace WebAppIntegrated.Utilities;

public class TokenUtility
{
    public static ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = System.Text.Encoding.ASCII.GetBytes(signingKey);

        try
        {
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken? validatedToken);

            return principal;
        }
        catch
        {
            return null;
        }
    }

    public static ClaimsIdentity GetClaimsIdentityFromToken(string token, string signingKey)
    {
        ClaimsPrincipal principal = GetPrincipalFromToken(token, signingKey);
        return principal?.Identity as ClaimsIdentity;
    }
}
