namespace Portafolio.API.Dtos;

public record ProductoCreateDto(

    string Sku,
    string Nombre,
    string? Descripcion,
    decimal Precio,
    int Stock
);

public record ProductoUpdateDto(

    string Nombre,
    string? Descripcion,
    decimal Precio,
    int Stock,
    bool Activo

);

public record ProductoResponseDto(

    int Id,
    string Sku,
    string Nombre,
    string? Descripcion,
    decimal Precio,
    int Stock,
    bool Activo,
    DateTime FechaCreacion

);

