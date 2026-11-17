namespace Portafolio.API.Dtos;

public record ClienteCreateDto(
    string Nombre,
    string Email,
    string? Telefono,
    string? Direccion
);

public record ClienteUpdateDto(
    string Nombre,
    string? Telefono,
    string? Direccion,
    bool Activo
);

public record ClienteResponseDto(
    int Id,
    string Nombre,
    string Email,
    string? Telefono,
    string? Direccion,
    DateTime FechaAlta,
    bool Activo
);