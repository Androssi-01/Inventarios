using Portafolio.API.Dtos;
using Portafolio.API.Validators;

namespace Portafolio.API.Tests;

public class ProveedorValidatorTests
{
    private static ProveedorCreateDto Crear(
        string nombreEmpresa = "Hardware Pro", string? contacto = "Luis Torres",
        string? email = "info@hw.com", string? telefono = "555-5566")
        => new(NombreEmpresa: nombreEmpresa, Contacto: contacto, Email: email, Telefono: telefono);

    [Fact]
    public void Create_Pasa_Con_Datos_Validos()
    {
        Assert.True(new ProveedorCreateValidator().Validate(Crear()).IsValid);
    }

    [Fact]
    public void Create_Acepta_Campos_Opcionales_Nulos()
    {
        var dto = Crear(contacto: null, email: null, telefono: null);
        Assert.True(new ProveedorCreateValidator().Validate(dto).IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData("A")]
    public void Create_Falla_Con_NombreEmpresa_Vacio_O_Corto(string nombre)
    {
        var resultado = new ProveedorCreateValidator().Validate(Crear(nombreEmpresa: nombre));
        Assert.Contains(resultado.Errors, e => e.PropertyName == "NombreEmpresa");
    }

    [Theory]
    [InlineData("correo-malo")]
    [InlineData("sin-arroba.com")]
    public void Create_Falla_Con_Email_Invalido(string email)
    {
        var resultado = new ProveedorCreateValidator().Validate(Crear(email: email));
        Assert.Contains(resultado.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public void Update_Falla_Con_NombreEmpresa_Vacio()
    {
        var dto = new ProveedorUpdateDto(NombreEmpresa: "", Contacto: null, Email: null, Telefono: null, Activo: true);
        var resultado = new ProveedorUpdateValidator().Validate(dto);
        Assert.Contains(resultado.Errors, e => e.PropertyName == "NombreEmpresa");
    }
}