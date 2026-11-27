using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Portafolio.API.Services;

public class JwtTokenService
{
    private readonly SymmetricSecurityKey _key;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expiresMinutes;

    public JwtTokenService(IConfiguration config)
    {
        var key = config["Jwt:Key"];
        if (string.IsNullOrWhiteSpace(key) || key.Length < 32)
            throw new InvalidOperationException(
                "Jwt:Key no está configurada o tiene menos de 32 caracteres.");

        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        _issuer = config["Jwt:Issuer"]
            ?? throw new InvalidOperationException("Falta Jwt:Issuer.");
        _audience = config["Jwt:Audience"]
            ?? throw new InvalidOperationException("Falta Jwt:Audience.");
        _expiresMinutes = config.GetValue<int?>("Jwt:ExpiresMinutes") ?? 60;
    }

    public string GenerateToken(string email, string rol)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, rol),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_expiresMinutes),
            signingCredentials: new SigningCredentials(_key, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}