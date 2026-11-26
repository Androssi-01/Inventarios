using Portafolio.API.Dtos;
using Portafolio.API.Validators;

namespace Portafolio.API.Tests;

public class ProductoValidatorTests
{
    private static ProductoCreateDto Crear(
        string sku = "PROD-001", string nombre = "Mouse",
        string? descripcion = "Inalámbrico", decimal precio = 100m, int stock = 10)
        => new(Sku: sku, Nombre: nombre, Descripcion: descripcion, Precio: precio, Stock: stock);

    [Fact]
    public void Create_Pasa_Con_Datos_Validos()
    {
        var resultado = new ProductoCreateValidator().Validate(Crear());
        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void Create_Acepta_Stock_Cero()
    {
        // Falla hasta que quites NotEmpty() de Stock
        var resultado = new ProductoCreateValidator().Validate(Crear(stock: 0));
        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void Create_Acepta_Precio_Cero()
    {
        var resultado = new ProductoCreateValidator().Validate(Crear(precio: 0m));
        Assert.True(resultado.IsValid);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Create_Falla_Con_Stock_Negativo(int stock)
    {
        var resultado = new ProductoCreateValidator().Validate(Crear(stock: stock));
        Assert.Contains(resultado.Errors, e => e.PropertyName == "Stock");
    }

    [Fact]
    public void Create_Falla_Con_Precio_Negativo()
    {
        var resultado = new ProductoCreateValidator().Validate(Crear(precio: -5m));
        Assert.Contains(resultado.Errors, e => e.PropertyName == "Precio");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_Falla_Con_Sku_Vacio(string sku)
    {
        var resultado = new ProductoCreateValidator().Validate(Crear(sku: sku));
        Assert.Contains(resultado.Errors, e => e.PropertyName == "Sku");
    }

    [Fact]
    public void Create_Falla_Con_Descripcion_Mayor_A_255()
    {
        var resultado = new ProductoCreateValidator().Validate(Crear(descripcion: new string('x', 256)));
        Assert.Contains(resultado.Errors, e => e.PropertyName == "Descripcion");
    }
}