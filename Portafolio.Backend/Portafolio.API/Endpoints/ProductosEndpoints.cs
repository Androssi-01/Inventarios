using Microsoft.EntityFrameworkCore;
using Portafolio.API.Data;
using Portafolio.API.Dtos;
using Portafolio.API.Filters;
using Portafolio.API.Commons;
using Portafolio.API.Models;

namespace Portafolio.API.Endpoints;

public static class ProductosEndpoints
{
    public static void MapProductosEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/productos")
                       .RequireAuthorization()
                       .WithTags("Productos");

        // GET por ID
        group.MapGet("/{id:int}", async (int id, PortafolioDbContext db) =>
        {
            var producto = await db.Productos.FindAsync(id);
            if (producto is null) return Results.NotFound();

            var response = new ProductoResponseDto(
                producto.Id, producto.Sku, producto.Nombre, producto.Descripcion, producto.Precio,
                producto.Stock, producto.Activo, producto.FechaCreacion
            );
            return Results.Ok(response);
        });

        // GET con paginación
        group.MapGet("/", async ([AsParameters] ProductoQueryParams query, PortafolioDbContext db) =>
        {
            var consulta = db.Productos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Buscar))
            {
                var busqueda = query.Buscar.Trim().ToLower();
                consulta = consulta.Where(p =>
                    p.Sku.ToLower().Contains(busqueda) ||
                    p.Nombre.ToLower().Contains(busqueda) ||
                    (p.Descripcion != null && p.Descripcion.ToLower().Contains(busqueda)));
            }

            if (query.PrecioMin.HasValue)
                consulta = consulta.Where(p => p.Precio >= query.PrecioMin.Value);

            if (query.PrecioMax.HasValue)
                consulta = consulta.Where(p => p.Precio <= query.PrecioMax.Value);

            consulta = (query.OrdenarPor?.ToLower(), query.Dir?.ToLower()) switch
            {
                ("sku", "desc") => consulta.OrderByDescending(p => p.Sku),
                ("nombre", "desc") => consulta.OrderByDescending(p => p.Nombre),
                ("precio", "desc") => consulta.OrderByDescending(p => p.Precio),
                ("stock", "desc") => consulta.OrderByDescending(p => p.Stock),
                ("fechacreacion", "desc") => consulta.OrderByDescending(p => p.FechaCreacion),
                ("sku", _) => consulta.OrderBy(p => p.Sku),
                ("precio", _) => consulta.OrderBy(p => p.Precio),
                ("stock", _) => consulta.OrderBy(p => p.Stock),
                ("fechacreacion", _) => consulta.OrderBy(p => p.FechaCreacion),
                _ => consulta.OrderBy(p => p.Nombre)
            };

            var totalRegistros = await consulta.CountAsync();
            var items = await consulta
                .Skip((query.PaginaEfectiva - 1) * query.TamanoEfectivo)
                .Take(query.TamanoEfectivo)
                .Select(p => new ProductoResponseDto(
                    p.Id, p.Sku, p.Nombre, p.Descripcion, p.Precio, p.Stock, p.Activo, p.FechaCreacion))
                .ToListAsync();

            var resultado = PagedResult<ProductoResponseDto>.Create(
                items, query.PaginaEfectiva, query.TamanoEfectivo, totalRegistros);

            return Results.Ok(resultado);
        });

        // POST
        group.MapPost("/", async (ProductoCreateDto dto, PortafolioDbContext db) =>
        {
            var producto = new Producto
            {
                Sku = dto.Sku,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                Stock = dto.Stock
            };
            db.Productos.Add(producto);
            await db.SaveChangesAsync();

            var response = new ProductoResponseDto(
                producto.Id, producto.Sku, producto.Nombre, producto.Descripcion,
                producto.Precio, producto.Stock, producto.Activo, producto.FechaCreacion);

            return Results.Created($"/api/productos/{producto.Id}", response);
        })
        .AddEndpointFilter<ValidationFilter<ProductoCreateDto>>()
        .RequireAuthorization("SoloAdmin");

        // PUT
        group.MapPut("/{id:int}", async (int id, ProductoUpdateDto dto, PortafolioDbContext db) =>
        {
            var producto = await db.Productos.FindAsync(id);
            if (producto is null) return Results.NotFound();

            producto.Nombre = dto.Nombre;
            producto.Descripcion = dto.Descripcion;
            producto.Precio = dto.Precio;
            producto.Stock = dto.Stock;
            producto.Activo = dto.Activo;

            await db.SaveChangesAsync();

            var response = new ProductoResponseDto(
                producto.Id, producto.Sku, producto.Nombre, producto.Descripcion,
                producto.Precio, producto.Stock, producto.Activo, producto.FechaCreacion);

            return Results.Ok(response);
        })
        .AddEndpointFilter<ValidationFilter<ProductoUpdateDto>>()
        .RequireAuthorization("SoloAdmin");

        // DELETE
        group.MapDelete("/{id:int}", async (int id, PortafolioDbContext db) =>
        {
            var producto = await db.Productos.FindAsync(id);
            if (producto is null) return Results.NotFound();

            db.Productos.Remove(producto);
            await db.SaveChangesAsync();

            return Results.NoContent();
        }).RequireAuthorization("SoloAdmin");
    }
}