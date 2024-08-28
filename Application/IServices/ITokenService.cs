using System.Security.Claims;

namespace Application.IServices;
public interface ITokenService
{
    string GenerateToken(Claim[] claims);
}
