using Portafolio.API.Dtos;
using Portafolio.API.Services;

namespace Portafolio.API.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth")
                       .WithTags("Autenticación");

        group.MapPost("/login", (LoginRequestDto dto, JwtTokenService tokenService, IConfiguration config) =>
        {
            // Cuenta demo pública: solo lectura
            if (dto.Email == "demo@portafolio.com" && dto.Password == "Demo123!")
            {
                var token = tokenService.GenerateToken(dto.Email, "Demo");
                return Results.Ok(new LoginResponseDto(token, dto.Email, "Demo"));
            }

            // Cuenta admin: credenciales fuera del repo
            var adminEmail = config["Admin:Email"];
            var adminPassword = config["Admin:Password"];

            if (!string.IsNullOrEmpty(adminEmail) && !string.IsNullOrEmpty(adminPassword)
                && dto.Email == adminEmail && dto.Password == adminPassword)
            {
                var token = tokenService.GenerateToken(dto.Email, "Admin");
                return Results.Ok(new LoginResponseDto(token, dto.Email, "Admin"));
            }

            return Results.Unauthorized();
        }).AllowAnonymous();
    }
}