namespace Portafolio.API.Dtos;

public record ProveedorCreateDto(

    string NombreEmpresa,
    string? Contacto,
    string? Email,
    string? Telefono
);

public record ProveedorUpdateDto(

    string NombreEmpresa,
    string? Contacto,
    string? Email,
    string? Telefono,
    bool Activo
);

public record ProveedorResponseDto(

    int Id,
    string NombreEmpresa,
    string? Contacto,
    string? Email,
    string? Telefono,
    bool Activo,
    DateTime FechaAlta

);

