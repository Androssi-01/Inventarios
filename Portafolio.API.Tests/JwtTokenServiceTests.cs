using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Portafolio.API.Services;

namespace Portafolio.API.Tests;

public class JwtTokenServiceTests
{
    private const string Key = "clave-de-pruebas-con-mas-de-32-caracteres-123";
    private const string Issuer = "TestIssuer";
    private const string Audience = "TestAudience";

    private static IConfiguration Config(string? key = Key, int? minutos = null)
    {
        var datos = new Dictionary<string, string?>
        {
            ["Jwt:Key"] = key,
            ["Jwt:Issuer"] = Issuer,
            ["Jwt:Audience"] = Audience
        };
        if (minutos.HasValue) datos["Jwt:ExpiresMinutes"] = minutos.Value.ToString();

        return new ConfigurationBuilder().AddInMemoryCollection(datos).Build();
    }

    private static TokenValidationParameters Parametros(string key = Key) => new()
    {
        ValidateIssuer = true,
        ValidIssuer = Issuer,
        ValidateAudience = true,
        ValidAudience = Audience,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ClockSkew = TimeSpan.Zero
    };

    private static JwtSecurityToken Leer(string token) =>
        new JwtSecurityTokenHandler().ReadJwtToken(token);

    [Fact]
    public void Token_Incluye_Issuer_Y_Audience()
    {
        var token = new JwtTokenService(Config()).GenerateToken("ana@test.com", "Admin");
        var jwt = Leer(token);

        Assert.Equal(Issuer, jwt.Issuer);
        Assert.Contains(Audience, jwt.Audiences);
    }

    [Fact]
    public void Token_Incluye_Email_Y_Rol()
    {
        var token = new JwtTokenService(Config()).GenerateToken("ana@test.com", "Admin");
        var jwt = Leer(token);

        // Según el handler, el tipo puede aparecer como nombre corto o largo.
        var email = jwt.Claims.FirstOrDefault(c => c.Type is "email" or ClaimTypes.Email)?.Value;
        var rol = jwt.Claims.FirstOrDefault(c => c.Type is "role" or ClaimTypes.Role)?.Value;

        Assert.Equal("ana@test.com", jwt.Subject);
        Assert.Equal("ana@test.com", email);
        Assert.Equal("Admin", rol);
    }

    [Fact]
    public void Cada_Token_Tiene_Jti_Distinto()
    {
        var servicio = new JwtTokenService(Config());

        var jti1 = Leer(servicio.GenerateToken("ana@test.com", "Admin")).Id;
        var jti2 = Leer(servicio.GenerateToken("ana@test.com", "Admin")).Id;

        Assert.NotEqual(jti1, jti2);
    }

    [Fact]
    public void Expira_Segun_La_Configuracion()
    {
        var token = new JwtTokenService(Config(minutos: 30)).GenerateToken("ana@test.com", "Admin");
        var vence = Leer(token).ValidTo;

        var esperado = DateTime.UtcNow.AddMinutes(30);
        Assert.InRange(vence, esperado.AddMinutes(-1), esperado.AddMinutes(1));
    }

    [Fact]
    public void Expira_Por_Defecto_En_60_Minutos()
    {
        var token = new JwtTokenService(Config()).GenerateToken("ana@test.com", "Admin");
        var vence = Leer(token).ValidTo;

        var esperado = DateTime.UtcNow.AddMinutes(60);
        Assert.InRange(vence, esperado.AddMinutes(-1), esperado.AddMinutes(1));
    }

    [Fact]
    public void Token_Se_Valida_Con_La_Clave_Correcta()
    {
        var token = new JwtTokenService(Config()).GenerateToken("ana@test.com", "Admin");

        var principal = new JwtSecurityTokenHandler().ValidateToken(token, Parametros(), out _);

        Assert.NotNull(principal);
    }

    [Fact]
    public void Token_Se_Rechaza_Con_Otra_Clave()
    {
        var token = new JwtTokenService(Config()).GenerateToken("ana@test.com", "Admin");
        var otraClave = "otra-clave-completamente-distinta-de-32-caracteres";

        Assert.ThrowsAny<SecurityTokenException>(() =>
            new JwtSecurityTokenHandler().ValidateToken(token, Parametros(otraClave), out _));
    }

    [Fact]
    public void Token_Vencido_Se_Rechaza()
    {
        // -5 minutos: el servicio genera un token que ya venció
        var token = new JwtTokenService(Config(minutos: -5)).GenerateToken("ana@test.com", "Admin");

        Assert.ThrowsAny<SecurityTokenException>(() =>
            new JwtSecurityTokenHandler().ValidateToken(token, Parametros(), out _));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("clave-corta")]
    public void Constructor_Falla_Con_Clave_Ausente_O_Corta(string? key)
    {
        Assert.Throws<InvalidOperationException>(() => new JwtTokenService(Config(key)));
    }
}