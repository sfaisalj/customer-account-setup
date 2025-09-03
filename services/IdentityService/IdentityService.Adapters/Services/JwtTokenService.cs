using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using IdentityService.Core.Models;
using IdentityService.Core.Ports.Outgoing;

namespace IdentityService.Adapters.Services;

public class JwtTokenService : ITokenService
{
    private readonly string _secretKey;
    private readonly int _expiryMinutes;

    public JwtTokenService(string secretKey = "your-super-secret-key-that-should-be-in-config-at-least-256-bits-long", int expiryMinutes = 60)
    {
        _secretKey = secretKey;
        _expiryMinutes = expiryMinutes;
    }

    public string GenerateToken(CustomerAccount user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new(ClaimTypes.Email, user.EmailAddress),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var authClaims = Claims.GetClaimsForRole(user.Role);
        claims.AddRange(authClaims.Select(claim => new Claim("permission", claim)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_expiryMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string? ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            return userId;
        }
        catch
        {
            return null;
        }
    }
}