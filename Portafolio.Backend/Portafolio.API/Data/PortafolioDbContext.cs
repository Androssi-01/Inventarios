
using Microsoft.EntityFrameworkCore;
using Portafolio.API.Models;

namespace Portafolio.API.Data;

public class PortafolioDbContext : DbContext
{

    public PortafolioDbContext(DbContextOptions<PortafolioDbContext> options)
        : base(options)
    {
    }


    public DbSet<Cliente> Clientes { get; set; } = null!;
    public DbSet<Producto> Productos { get; set; } = null!;
    public DbSet<Proveedor> Proveedores { get; set; } = null!;
    public DbSet<ProductoProveedor> ProductosProveedores { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("Clientes");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.FechaAlta)
                .HasDefaultValueSql("GETDATE()");

        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.ToTable("Productos");
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Sku).IsUnique();

            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("GETDATE()");

            entity.Property(p => p.Precio)
            .HasPrecision(18, 2);

        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.ToTable("Proveedores");
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.NombreEmpresa).IsUnique();

            entity.Property(e => e.FechaAlta)
                .HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<ProductoProveedor>(entity =>
        {
            entity.ToTable("ProductosProveedores");

            entity.HasKey(e => new { e.ProductoId, e.ProveedorId });

            entity.HasOne(e => e.Producto)
                .WithMany()
                .HasForeignKey(e => e.ProductoId);

            entity.HasOne(e => e.Proveedor)
                .WithMany()
                .HasForeignKey(e => e.ProveedorId);

            entity.Property(e => e.FechaAsociacion)
                .HasDefaultValueSql("GETDATE()");

            entity.Property(p => p.PrecioCompra)
                .HasPrecision(18, 2);
        });
    }
}