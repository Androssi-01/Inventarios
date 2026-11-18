using Microsoft.EntityFrameworkCore;
using Portafolio.API.Data;
using Portafolio.API.Dtos;
using Portafolio.API.Filters;
using Portafolio.API.Commons;
using Portafolio.API.Models;

namespace Portafolio.API.Endpoints;

public static class ClientesEndpoints
{
    public static void MapClientesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/clientes")
                       .RequireAuthorization() // Protegemos todo el grupo de una vez
                       .WithTags("Clientes");  // Para que Swagger los agrupe bonito

        // GET por ID
        group.MapGet("/{id:int}", async (int id, PortafolioDbContext db) =>
        {
            var cliente = await db.Clientes.FindAsync(id);
            if (cliente is null) return Results.NotFound();

            var response = new ClienteResponseDto(
                cliente.Id, cliente.Nombre, cliente.Email, cliente.Telefono,
                cliente.Direccion, cliente.FechaAlta, cliente.Activo
            );
            return Results.Ok(response);
        });

        // GET con paginación
        group.MapGet("/", async ([AsParameters] ClienteQueryParams query, PortafolioDbContext db) =>
        {
            var consulta = db.Clientes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Buscar))
            {
                var busqueda = query.Buscar.Trim().ToLower();
                consulta = consulta.Where(c => c.Nombre.ToLower().Contains(busqueda) || c.Email.ToLower().Contains(busqueda));
            }

            consulta = (query.OrdenarPor?.ToLower(), query.Dir?.ToLower()) switch
            {
                ("nombre", "desc") => consulta.OrderByDescending(c => c.Nombre),
                ("email", "desc") => consulta.OrderByDescending(c => c.Email),
                ("fechaalta", "desc") => consulta.OrderByDescending(c => c.FechaAlta),
                ("email", _) => consulta.OrderBy(c => c.Email),
                ("fechaalta", _) => consulta.OrderBy(c => c.FechaAlta),
                _ => consulta.OrderBy(c => c.Nombre)
            };

            var totalRegistros = await consulta.CountAsync();
            var items = await consulta
                .Skip((query.PaginaEfectiva - 1) * query.TamanoEfectivo)
                .Take(query.TamanoEfectivo)
                .Select(c => new ClienteResponseDto(
                    c.Id, c.Nombre, c.Email, c.Telefono, c.Direccion, c.FechaAlta, c.Activo))
                .ToListAsync();

            var resultado = PagedResult<ClienteResponseDto>.Create(items, query.PaginaEfectiva, query.TamanoEfectivo, totalRegistros);
            return Results.Ok(resultado);
        });

        // POST
        group.MapPost("/", async (ClienteCreateDto dto, PortafolioDbContext db) =>
        {
            var cliente = new Cliente { Nombre = dto.Nombre, Email = dto.Email, Telefono = dto.Telefono, Direccion = dto.Direccion };
            db.Clientes.Add(cliente);
            await db.SaveChangesAsync();

            var response = new ClienteResponseDto(cliente.Id, cliente.Nombre, cliente.Email, cliente.Telefono, cliente.Direccion, cliente.FechaAlta, cliente.Activo);
            return Results.Created($"/api/clientes/{cliente.Id}", response);
        }).AddEndpointFilter<ValidationFilter<ClienteCreateDto>>();

        // PUT
        group.MapPut("/{id:int}", async (int id, ClienteUpdateDto dto, PortafolioDbContext db) =>
        {
            var cliente = await db.Clientes.FindAsync(id);
            if (cliente is null) return Results.NotFound();

            cliente.Nombre = dto.Nombre;
            cliente.Telefono = dto.Telefono;
            cliente.Direccion = dto.Direccion;
            cliente.Activo = dto.Activo;
            await db.SaveChangesAsync();

            var response = new ClienteResponseDto(cliente.Id, cliente.Nombre, cliente.Email, cliente.Telefono, cliente.Direccion, cliente.FechaAlta, cliente.Activo);
            return Results.Ok(response);
        }).AddEndpointFilter<ValidationFilter<ClienteUpdateDto>>();

        // DELETE
        group.MapDelete("/{id:int}", async (int id, PortafolioDbContext db) =>
        {
            var cliente = await db.Clientes.FindAsync(id);
            if (cliente is null) return Results.NotFound();

            db.Clientes.Remove(cliente);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}