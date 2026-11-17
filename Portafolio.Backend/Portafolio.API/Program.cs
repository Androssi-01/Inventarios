using Microsoft.EntityFrameworkCore;

using Portafolio.API.Data;
using Portafolio.API.Models;
using Portafolio.API.Dtos;
using Portafolio.API.Filters;
using Portafolio.API.Validators;
using FluentValidation;
using Portafolio.API.Commons;
using Microsoft.AspNetCore.Http;




var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader();
    });
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddValidatorsFromAssemblyContaining<ClienteCreateValidator>();
builder.Services.AddDbContext<PortafolioDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/clientes/{id:int}", async (int id, PortafolioDbContext db) =>
{
    var cliente = await db.Clientes.FindAsync(id);
    if (cliente is null)
        return Results.NotFound();

    var response = new ClienteResponseDto(
        cliente.Id,
        cliente.Nombre,
        cliente.Email,
        cliente.Telefono,
        cliente.Direccion,
        cliente.FechaAlta,
        cliente.Activo
    );

    return Results.Ok(response);
});

app.MapGet("/api/clientes", async ([AsParameters] ClienteQueryParams query, PortafolioDbContext db) =>
{
    // 1. Empezar con IQueryable (aún NO ejecuta SQL)
    var consulta = db.Clientes.AsQueryable();

    // 2. Filtro de búsqueda
    if (!string.IsNullOrWhiteSpace(query.Buscar))
    {
        var busqueda = query.Buscar.Trim().ToLower();
        consulta = consulta.Where(c =>
            c.Nombre.ToLower().Contains(busqueda) ||
            c.Email.ToLower().Contains(busqueda));
    }
    // 3. Ordenamiento (whitelist para seguridad)
    consulta = (query.OrdenarPor?.ToLower(), query.Dir?.ToLower()) switch
    {
        ("nombre", "desc") => consulta.OrderByDescending(C => C.Nombre),
        ("email", "desc") => consulta.OrderByDescending(c => c.Email),
        ("fechaalta", "desc") => consulta.OrderByDescending(c => c.FechaAlta),
        ("email", _) => consulta.OrderBy(c => c.Email),
        ("fechaalta", _) => consulta.OrderBy(c => c.FechaAlta),
        _ => consulta.OrderBy(c => c.Nombre)  // default: ordenar por nombre

    };

    // 4. Contar total (necesitamos el conteo ANTES de paginar)

    var totalRegistros = await consulta.CountAsync();


    // 5. Aplicar paginación
    var items = await consulta
        .Skip((query.PaginaEfectiva - 1) * query.TamanoEfectivo)
        .Take(query.TamanoEfectivo)
        .Select(c => new ClienteResponseDto(
            c.Id, c.Nombre, c.Email, c.Telefono, c.Direccion, c.FechaAlta, c.Activo))
        .ToListAsync();

    // 6. Envolver en PagedResult

    var resultado = PagedResult<ClienteResponseDto>.Create(
        items, query.PaginaEfectiva, query.TamanoEfectivo, totalRegistros);

    return Results.Ok(resultado);
});

app.MapPost("/api/clientes", async (ClienteCreateDto dto, PortafolioDbContext db) =>
{
    var cliente = new Cliente
    {
        Nombre = dto.Nombre,
        Email = dto.Email,
        Telefono = dto.Telefono,
        Direccion = dto.Direccion
    };

    db.Clientes.Add(cliente);
    await db.SaveChangesAsync();

    var response = new ClienteResponseDto(
        cliente.Id, cliente.Nombre, cliente.Email,
        cliente.Telefono, cliente.Direccion,
        cliente.FechaAlta, cliente.Activo
    );

    return Results.Created($"/api/clientes/{cliente.Id}", response);
}).AddEndpointFilter<ValidationFilter<ClienteCreateDto>>();

app.MapPut("/api/clientes/{id:int}", async (int id, ClienteUpdateDto dto, PortafolioDbContext db) =>
{
    var cliente = await db.Clientes.FindAsync(id);

    if (cliente is null)
        return Results.NotFound();

    cliente.Nombre = dto.Nombre;
    cliente.Telefono = dto.Telefono;
    cliente.Direccion = dto.Direccion;
    cliente.Activo = dto.Activo;

    await db.SaveChangesAsync();

    var response = new ClienteResponseDto(
        cliente.Id,
        cliente.Nombre,
        cliente.Email,
        cliente.Telefono,
        cliente.Direccion,
        cliente.FechaAlta,
        cliente.Activo
    );

    return Results.Ok(response);

}).AddEndpointFilter<ValidationFilter<ClienteUpdateDto>>();

app.MapDelete("/api/clientes/{id:int}", async (int id, PortafolioDbContext db) =>
{
    var cliente = await db.Clientes.FindAsync(id);
    if (cliente is null)
        return Results.NotFound();

    db.Clientes.Remove(cliente);
    await db.SaveChangesAsync();

    return Results.NoContent();

});



app.MapGet("/api/productos/{id:int}", async (int id, PortafolioDbContext db) =>
{
    var producto = await db.Productos.FindAsync(id);
    if (producto is null)
        return Results.NotFound();

    var response = new ProductoResponseDto(

        producto.Id,
        producto.Sku,
        producto.Nombre,
        producto.Descripcion,
        producto.Precio,
        producto.Stock,
        producto.Activo,
        producto.FechaCreacion
    );

    return Results.Ok(response);
});

app.MapGet("/api/productos", async ([AsParameters] ProductoQueryParams query, PortafolioDbContext db) =>
{
    var consulta = db.Productos.AsQueryable();

    if (!string.IsNullOrWhiteSpace(query.Buscar))
    {

        var busqueda = query.Buscar.Trim().ToLower();

        consulta = consulta.Where(p =>
            p.Sku.ToLower().Contains(busqueda) ||
            p.Nombre.ToLower().Contains(busqueda) ||
            (p.Descripcion != null && p.Descripcion.ToLower().Contains(busqueda))
        );
    }

    if (query.PrecioMin.HasValue)
    {
        consulta = consulta.Where(p => p.Precio >= query.PrecioMin.Value);
    }

    if (query.PrecioMax.HasValue)
    {
        consulta = consulta.Where(p => p.Precio <= query.PrecioMax.Value);
    }

    consulta = (query.OrdenarPor?.ToLower(), query.Dir?.ToLower()) switch
    {
        ("sku", "desc") => consulta.OrderByDescending(p => p.Sku),
        ("nombre", "desc") => consulta.OrderByDescending(p => p.Nombre),
        ("precio", "desc") => consulta.OrderByDescending(p => p.Precio),
        ("stock", "desc") => consulta.OrderByDescending(p => p.Stock),

        ("sku", _) => consulta.OrderBy(p => p.Sku),
        ("precio", _) => consulta.OrderBy(p => p.Precio),
        ("stock", _) => consulta.OrderBy(p => p.Stock),
        ("fechacreacion", _) => consulta.OrderBy(p => p.FechaCreacion),

        _ => consulta.OrderBy(p => p.Nombre)  // default: ordenar por nombre

    };

    var totalRegistros = await consulta.CountAsync();


    var items = await consulta
        .Skip((query.PaginaEfectiva - 1) * query.TamanoEfectivo)
        .Take(query.TamanoEfectivo)
        .Select(p => new ProductoResponseDto(
            p.Id, p.Sku, p.Nombre, p.Descripcion, p.Precio, p.Stock, p.Activo, p.FechaCreacion))
        .ToListAsync();

    // 6. Envolver en PagedResult

    var resultado = PagedResult<ProductoResponseDto>.Create(
        items, query.PaginaEfectiva, query.TamanoEfectivo, totalRegistros);

    return Results.Ok(resultado);
});

app.MapPost("/api/productos", async (ProductoCreateDto dto, PortafolioDbContext db) =>
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
        producto.Precio, producto.Stock, producto.Activo, producto.FechaCreacion
    );

    return Results.Created($"/api/productos/{producto.Id}", response);
}).AddEndpointFilter<ValidationFilter<ProductoCreateDto>>();

app.MapPut("/api/productos/{id:int}", async (int id, ProductoUpdateDto dto, PortafolioDbContext db) =>
{
    var producto = await db.Productos.FindAsync(id);

    if (producto is null)
        return Results.NotFound();

    producto.Nombre = dto.Nombre;
    producto.Descripcion = dto.Descripcion;
    producto.Precio = dto.Precio;
    producto.Stock = dto.Stock;
    producto.Activo = dto.Activo;

    await db.SaveChangesAsync();

    var response = new ProductoResponseDto(
        producto.Id,
        producto.Sku,
        producto.Nombre,
        producto.Descripcion,
        producto.Precio,
        producto.Stock,
        producto.Activo,
        producto.FechaCreacion
    );

    return Results.Ok(response);

}).AddEndpointFilter<ValidationFilter<ProductoUpdateDto>>();

app.MapDelete("/api/productos/{id:int}", async (int id, PortafolioDbContext db) =>
{
    var productos = await db.Productos.FindAsync(id);
    if (productos is null)
        return Results.NotFound();

    db.Productos.Remove(productos);
    await db.SaveChangesAsync();

    return Results.NoContent();

});


app.MapGet("/api/proveedores/{id:int}", async (int id, PortafolioDbContext db) =>
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

app.MapGet("/api/proveedores", async ([AsParameters] ProveedorQueryParams query, PortafolioDbContext db) =>
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

    // 6. Envolver en PagedResult

    var resultado = PagedResult<ProveedorResponseDto>.Create(
        items, query.PaginaEfectiva, query.TamanoEfectivo, totalRegistros);

    return Results.Ok(resultado);
});

app.MapPost("/api/proveedores", async (ProveedorCreateDto dto, PortafolioDbContext db) =>
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

app.MapPut("/api/proveedores/{id:int}", async (int id, ProveedorUpdateDto dto, PortafolioDbContext db) =>
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

app.MapDelete("/api/proveedores/{id:int}", async (int id, PortafolioDbContext db) =>
{
    var proveedor = await db.Proveedores.FindAsync(id);
    if (proveedor is null)
        return Results.NotFound();

    db.Proveedores.Remove(proveedor);
    await db.SaveChangesAsync();

    return Results.NoContent();

});


app.MapGet("/api/productos-proveedores", async ([AsParameters] ProductoProveedorQueryParams query, PortafolioDbContext db) =>
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

app.MapPost("/api/productos-proveedores", async (ProductoProveedorCreateDto dto, PortafolioDbContext db) =>
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


});

app.MapPut("/api/productos-proveedores/{productoId:int}/{proveedorId:int}",
    async (int productoId, int proveedorId, ProductoProveedorUpdateDto dto, PortafolioDbContext db) =>
{
    var relacion = await db.ProductosProveedores.FindAsync(productoId, proveedorId);

    if (relacion is null)
        return Results.NotFound();

    relacion.PrecioCompra = dto.PrecioCompra;

    await db.SaveChangesAsync();
    return Results.NoContent();

});

app.MapDelete("/api/productos-proveedores/{productoId:int}/{proveedorId:int}",
    async (int productoId, int proveedorId, PortafolioDbContext db) =>
{
    var relacion = await db.ProductosProveedores.FindAsync(productoId, proveedorId);

    if (relacion is null)
        return Results.NotFound();

    db.ProductosProveedores.Remove(relacion);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();
