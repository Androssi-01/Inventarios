namespace Portafolio.API.Dtos;

public class ProductoProveedorQueryParams
{
    private const int TamanoMaximo = 50;
    private const int TamanoPredeterminado = 10;
    public int? Pagina { get; set; }
    public int? Tamano { get; set; }
    public int PaginaEfectiva => Pagina is null or < 1 ? 1 : Pagina.Value;
    public int TamanoEfectivo => Tamano switch
    {
        null or < 1 => TamanoPredeterminado,
        > TamanoMaximo => TamanoMaximo,
        _ => Tamano.Value
    };
}