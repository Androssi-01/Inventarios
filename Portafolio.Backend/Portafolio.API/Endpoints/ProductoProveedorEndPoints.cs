using Microsoft.EntityFrameworkCore;
using Portafolio.API.Data;
using Portafolio.API.Dtos;
using Portafolio.API.Filters;
using Portafolio.API.Commons;
using Portafolio.API.Models;

namespace Portafolio.API.Endpoints;

public static class ProductosProveedoresEndpoints
{
    public static void MapProductosProveedoresEndpoints(this WebApplication app)
    {
        // 1. Ruta corregida (con guion)
        var group = app.MapGroup("/api/productos-proveedores")
                       .RequireAuthorization()
                       .WithTags("Productos-Proveedores");

        // GET
        group.MapGet("/", async ([AsParameters] ProductoProveedorQueryParams query, PortafolioDbContext db) =>
        {
            var consulta = db.ProductosProveedores.AsQueryable();
            var totalRegistros = await consulta.CountAsync();

            var items = await consulta
                .Include(pp => pp.Producto)
                .Include(pp => pp.Proveedor)
                .Skip((query.PaginaEfectiva - 1) * query.TamanoEfectivo)
                .Take(query.TamanoEfectivo)
                .Select(pp => new ProductoProveedorResponseDto(
                    pp.ProductoId,
                    pp.Producto.Nombre,
                    pp.ProveedorId,
                    pp.Proveedor.NombreEmpresa,
                    pp.PrecioCompra,
                    pp.FechaAsociacion
                )).ToListAsync();

            var resultado = PagedResult<ProductoProveedorResponseDto>.Create(
                items,
                query.PaginaEfectiva,
                query.TamanoEfectivo,
                totalRegistros);

            return Results.Ok(resultado);
        });

        // POST
        group.MapPost("/", async (ProductoProveedorCreateDto dto, PortafolioDbContext db) =>
        {
            var productoExiste = await db.Productos.AnyAsync(p => p.Id == dto.ProductoId);
            var proveedorExiste = await db.Proveedores.AnyAsync(p => p.Id == dto.ProveedorId);

            if (!productoExiste || !proveedorExiste)
                return Results.BadRequest("El producto o el proveedor no existen.");

            var yaExiste = await db.ProductosProveedores
                .AnyAsync(pp => pp.ProductoId == dto.ProductoId && pp.ProveedorId == dto.ProveedorId);

            if (yaExiste)
                return Results.BadRequest("Este producto ya esta asociado a este proveedor");

            var relacion = new ProductoProveedor
            {
                ProductoId = dto.ProductoId,
                ProveedorId = dto.ProveedorId,
                PrecioCompra = dto.PrecioCompra,
            };

            db.ProductosProveedores.Add(relacion);
            await db.SaveChangesAsync();

            return Results.Created($"/api/productos-proveedores/{relacion.ProductoId}/{relacion.ProveedorId}", dto);
        }).RequireAuthorization("SoloAdmin");

        // PUT
        group.MapPut("/{productoId:int}/{proveedorId:int}", async (int productoId, int proveedorId, ProductoProveedorUpdateDto dto, PortafolioDbContext db) =>
        {
            var relacion = await db.ProductosProveedores.FindAsync(productoId, proveedorId);
            if (relacion is null) return Results.NotFound();

            relacion.PrecioCompra = dto.PrecioCompra;
            await db.SaveChangesAsync();

            return Results.NoContent();
        }).RequireAuthorization("SoloAdmin");

        // DELETE (Corregido: usa group y la ruta relativa)
        group.MapDelete("/{productoId:int}/{proveedorId:int}", async (int productoId, int proveedorId, PortafolioDbContext db) =>
        {
            var relacion = await db.ProductosProveedores.FindAsync(productoId, proveedorId);
            if (relacion is null) return Results.NotFound();

            db.ProductosProveedores.Remove(relacion);
            await db.SaveChangesAsync();

            return Results.NoContent();
        }).RequireAuthorization("SoloAdmin");
    } // Cierra el método
} // Cierra la clase