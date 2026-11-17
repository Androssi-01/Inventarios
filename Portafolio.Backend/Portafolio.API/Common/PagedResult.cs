namespace Portafolio.API.Commons;

public record PagedResult<T>(

    IEnumerable<T> Items,
    int PaginaActual,
    int TamanoPagina,
    int TotalRegistros,
    int TotalPaginas,
    bool TieneSiguiente,
    bool TieneAnterior
)
{
    public static PagedResult<T> Create(
        IEnumerable<T> items,
        int pagina,
        int tamano,
        int totalRegistros)
    {
        var totalPaginas = (int)Math.Ceiling(totalRegistros / (double)tamano);

        return new PagedResult<T>(
            Items: items,
            PaginaActual: pagina,
            TamanoPagina: tamano,
            TotalRegistros: totalRegistros,
            TotalPaginas: totalPaginas,
            TieneSiguiente: pagina < totalPaginas,
            TieneAnterior: pagina > 1

        );
    }

}