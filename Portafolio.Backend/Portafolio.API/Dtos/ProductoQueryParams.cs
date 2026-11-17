namespace Portafolio.API.Dtos;

public class ProductoQueryParams
{
    private const int TamanoMaximo = 50;
    private const int TamanoPredeterminado = 10;

    // Propiedades que .NET llenará desde la URL. Al ser int?, si no vienen, se quedan en null.

    public decimal? PrecioMin { get; set; }
    public decimal? PrecioMax { get; set; }
    public int? Pagina { get; set; }
    public int? Tamano { get; set; }
    public string? Buscar { get; set; }
    public string? OrdenarPor { get; set; }
    public string? Dir { get; set; }

    // Propiedades calculadas (read-only). Aquí aplicamos la lógica y los defaults.
    public int PaginaEfectiva => Pagina is null or < 1 ? 1 : Pagina.Value;

    public int TamanoEfectivo => Tamano switch
    {
        null or < 1 => TamanoPredeterminado,
        > TamanoMaximo => TamanoMaximo,
        _ => Tamano.Value
    };
}