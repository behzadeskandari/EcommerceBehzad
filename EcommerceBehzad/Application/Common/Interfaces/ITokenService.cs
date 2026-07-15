using System.Security.Claims;
using EcommerceBehzad.Domain.Entities;

namespace EcommerceBehzad.Application.Common.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
