using Microsoft.EntityFrameworkCore;
using Portafolio.API.Data;
using Portafolio.API.Dtos;
using Portafolio.API.Filters;
using Portafolio.API.Commons;
using Portafolio.API.Models;


namespace Portafolio.API.Endpoints;

public static class ProveedoresEndpoints
{
    public static void MapProveedoresEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/proveedores")
                       .RequireAuthorization() // Protegemos todo el grupo de una vez
                       .WithTags("Proveedores");  // Para que Swagger los agrupe bonito

        group.MapGet("/{id:int}", async (int id, PortafolioDbContext db) =>
        {



            var proveedor = await db.Proveedores.FindAsync(id);
            if (proveedor is null)
                return Results.NotFound();

            var response = new ProveedorResponseDto(

            proveedor.Id,
            proveedor.NombreEmpresa,
            proveedor.Contacto,
            proveedor.Email,
            proveedor.Telefono,
            proveedor.Activo,
            proveedor.FechaAlta
            );

            return Results.Ok(response);
        });

        group.MapGet("/", async ([AsParameters] ProveedorQueryParams query, PortafolioDbContext db) =>
        {
            var consulta = db.Proveedores.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Buscar))
            {

                var busqueda = query.Buscar.Trim().ToLower();

                consulta = consulta.Where(pv =>
                                pv.NombreEmpresa.ToLower().Contains(busqueda) ||
                                (pv.Email != null && pv.Email.ToLower().Contains(busqueda))
                            );
            }

            consulta = (query.OrdenarPor?.ToLower(), query.Dir?.ToLower()) switch
            {

                ("nombreempresa", "desc") => consulta.OrderByDescending(pv => pv.NombreEmpresa),
                ("email", "desc") => consulta.OrderByDescending(pv => pv.Email),
                ("fechaalta", "desc") => consulta.OrderByDescending(pv => pv.FechaAlta),

                ("email", _) => consulta.OrderBy(pv => pv.Email),
                ("fechaalta", _) => consulta.OrderBy(pv => pv.FechaAlta),

                _ => consulta.OrderBy(pv => pv.NombreEmpresa)  // default: ordenar por nombre

            };

            var totalRegistros = await consulta.CountAsync();


            var items = await consulta
                .Skip((query.PaginaEfectiva - 1) * query.TamanoEfectivo)
                .Take(query.TamanoEfectivo)
                .Select(pv => new ProveedorResponseDto(
                    pv.Id,
                    pv.NombreEmpresa,
                    pv.Contacto,
                    pv.Email,
                    pv.Telefono,
                    pv.Activo,
                    pv.FechaAlta))
                .ToListAsync();

            var resultado = PagedResult<ProveedorResponseDto>.Create(
                items, query.PaginaEfectiva, query.TamanoEfectivo, totalRegistros);

            return Results.Ok(resultado);
        });

        group.MapPost("/", async (ProveedorCreateDto dto, PortafolioDbContext db) =>
        {
            var proveedor = new Proveedor
            {
                NombreEmpresa = dto.NombreEmpresa,
                Contacto = dto.Contacto,
                Email = dto.Email,
                Telefono = dto.Telefono
            };

            db.Proveedores.Add(proveedor);
            await db.SaveChangesAsync();

            var response = new ProveedorResponseDto(
            proveedor.Id,
            proveedor.NombreEmpresa,
            proveedor.Contacto,
            proveedor.Email,
            proveedor.Telefono,
            proveedor.Activo,
            proveedor.FechaAlta
            );

            return Results.Created($"/api/proveedores/{proveedor.Id}", response);
        }).AddEndpointFilter<ValidationFilter<ProveedorCreateDto>>();

        group.MapPut("/{id:int}", async (int id, ProveedorUpdateDto dto, PortafolioDbContext db) =>
        {
            var proveedor = await db.Proveedores.FindAsync(id);

            if (proveedor is null)
                return Results.NotFound();

            proveedor.NombreEmpresa = dto.NombreEmpresa;
            proveedor.Contacto = dto.Contacto;
            proveedor.Email = dto.Email;
            proveedor.Telefono = dto.Telefono;
            proveedor.Activo = dto.Activo;

            await db.SaveChangesAsync();

            var response = new ProveedorResponseDto(
            proveedor.Id,
            proveedor.NombreEmpresa,
            proveedor.Contacto,
            proveedor.Email,
            proveedor.Telefono,
            proveedor.Activo,
            proveedor.FechaAlta
            );

            return Results.Ok(response);

        }).AddEndpointFilter<ValidationFilter<ProveedorUpdateDto>>();

        group.MapDelete("/{id:int}", async (int id, PortafolioDbContext db) =>
        {
            var proveedor = await db.Proveedores.FindAsync(id);
            if (proveedor is null)
                return Results.NotFound();

            db.Proveedores.Remove(proveedor);
            await db.SaveChangesAsync();

            return Results.NoContent();

        });
    }
}

