using Xunit;
using Portafolio.API.Dtos;
using Portafolio.API.Validators;

namespace Portafolio.API.Tests;

public class ClienteValidatorTests
{
    [Theory]
    [InlineData("correo-malo")]
    [InlineData("sin-arroba.com")]
    [InlineData("@test.com")]
    [InlineData("")]
    public void Create_Falla_Con_Email_Invalido(string email)
    {
        var dto = new ClienteCreateDto("Ana", email, "555-1234", "Calle 1");
        var resultado = new ClienteCreateValidator().Validate(dto);

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, e => e.PropertyName == "Email");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("A")]
    public void Create_Falla_Con_Nombre_Vacio_O_Muy_Corto(string nombre)
    {
        var dto = new ClienteCreateDto(nombre, "ana@test.com", "555-1234", "Calle 1");
        var resultado = new ClienteCreateValidator().Validate(dto);

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, e => e.PropertyName == "Nombre");
    }

    [Fact]
    public void Create_Falla_Con_Nombre_Mayor_A_150()
    {
        var dto = new ClienteCreateDto(new string('A', 151), "ana@test.com", "555-1234", "Calle 1");
        var resultado = new ClienteCreateValidator().Validate(dto);

        Assert.Contains(resultado.Errors, e => e.PropertyName == "Nombre");
    }

    [Fact]
    public void Create_Acepta_Telefono_Y_Direccion_Nulos()
    {
        var dto = new ClienteCreateDto("Ana García", "ana@test.com", null, null);
        var resultado = new ClienteCreateValidator().Validate(dto);

        Assert.True(resultado.IsValid);
    }
}