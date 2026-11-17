namespace Portafolio.API.Dtos;

public record ProductoProveedorCreateDto(

    int ProductoId,
    int ProveedorId,
    decimal PrecioCompra

);

public record ProductoProveedorUpdateDto(

    decimal PrecioCompra

);

public record ProductoProveedorResponseDto(

    int ProductoId,
    string NombreProducto,
    int ProveedorId,
    string NombreProveedor,
    decimal PrecioCompra,
    DateTime FechaAsociacion



);