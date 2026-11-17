namespace Portafolio.API.Models;


public class ProductoProveedor
{
    public int ProductoId { get; set; }
    public int ProveedorId { get; set; }
    public decimal PrecioCompra { get; set; }
    public DateTime FechaAsociacion { get; set; }
    public Producto Producto { get; set; } = null!;
    public Proveedor Proveedor { get; set; } = null!;
}