namespace Portafolio.API.Models;

public class Proveedor
{
    public int Id { get; set; }
    public string NombreEmpresa { get; set; } = string.Empty;
    public string? Contacto { get; set; }
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaAlta { get; set; }


}