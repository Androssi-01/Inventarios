using Xunit;
using Portafolio.API.Dtos;
using Portafolio.API.Validators;

namespace Portafolio.API.Tests;

public class ClienteValidatorTests
{
    [Fact]
    public void Validator_Deberia_Fallar_Cuando_Email_Es_Invalido()
    {
        // 1. Preparar (Arrange)
        var validator = new ClienteCreateValidator();
        var clienteInvalido = new ClienteCreateDto("Ana", "correo-malo", "555-1234", "Calle 1");

        // 2. Actuar (Act)
        var resultado = validator.Validate(clienteInvalido);

        // 3. Afirmar (Assert)
        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public void Validator_Deberia_Pasar_Cuando_Todos_Los_Datos_Son_Correctos()
    {
        // 1. Preparar
        var validator = new ClienteCreateValidator();
        var clienteValido = new ClienteCreateDto("Ana García", "ana@test.com", "555-1234", "Calle 1");

        // 2. Actuar
        var resultado = validator.Validate(clienteValido);

        // 3. Afirmar
        Assert.True(resultado.IsValid);
    }
}