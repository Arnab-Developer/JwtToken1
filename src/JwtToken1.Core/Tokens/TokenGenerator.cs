using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtToken1.Core.Tokens;

public class TokenGenerator(IOptionsMonitor<TokenOptions> optionsMonitor) : ITokenGenerator
{
    private readonly TokenOptions _tokenOptions = optionsMonitor.CurrentValue;

    public string GenerateToken(int id, string name, params IEnumerable<string> roleNames)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.Key));

        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, id.ToString()),
            new(JwtRegisteredClaimNames.Name, name),
            new("Roles", roleNames.Aggregate((first, second) => $"{first},{second}"))
        };

        var descriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_tokenOptions.ExpiresInMinutes),
            Issuer = _tokenOptions.Issuer,
            Audience = _tokenOptions.Audience,
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(descriptor);
        return handler.WriteToken(token);
    }
}