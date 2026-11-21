using System.Net;
using System.Text.Json;
using Microsoft.Data.SqlClient; // Para atrapar errores de SQL
using Microsoft.EntityFrameworkCore;

namespace Portafolio.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // Intenta ejecutar el endpoint normalmente
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ha ocurrido un error no controlado.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";


        var statusCode = HttpStatusCode.InternalServerError;
        var mensaje = "Ha ocurrido un error interno en el servidor.";


        if (exception is DbUpdateException dbEx && dbEx.InnerException is SqlException sqlEx)
        {

            if (sqlEx.Number == 2601 || sqlEx.Number == 2627)
            {
                statusCode = HttpStatusCode.BadRequest;
                mensaje = "El registro ya existe. Verifica que los datos únicos (como el email o SKU) no estén duplicados.";
            }
        }

        var response = new
        {
            status = (int)statusCode,
            title = "Error en la operación",
            detail = mensaje
        };

        context.Response.StatusCode = (int)statusCode;
        var jsonResponse = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(jsonResponse);
    }
}
