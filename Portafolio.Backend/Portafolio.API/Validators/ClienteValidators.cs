using FluentValidation;
using Portafolio.API.Dtos;

namespace Portafolio.API.Validators;

public class ClienteCreateValidator : AbstractValidator<ClienteCreateDto>
{
    public ClienteCreateValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
                .WithMessage("El nombre es obligatorio")
            .MinimumLength(2)
                .WithMessage("El nombre debe tener al menos dos caracteres")
            .MaximumLength(150)
                .WithMessage("El nombre no puede exceder 150 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty()
                .WithMessage("El email es obligatorio")
            .EmailAddress()
                .WithMessage("El email no tiene un formato válido")
            .MaximumLength(150)
                .WithMessage("El email no puede exceder 150 caracteres");

        RuleFor(x => x.Telefono)
            .MaximumLength(30)
                .WithMessage("El Teléfono no puede exceder 30 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Telefono));

        RuleFor(x => x.Direccion)
            .MaximumLength(255)
                .WithMessage("La Direccion no puede exceder los 255 caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.Direccion));
    }
}

public class ClienteUpdateValidator : AbstractValidator<ClienteUpdateDto>
{
    public ClienteUpdateValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
                .WithMessage("El nombre es obligatorio")
            .MinimumLength(2)
                .WithMessage("El nombre debe de tener al menos 2 caracteres")
            .MaximumLength(150)
                .WithMessage("El nombre no puede exceder 150 caracteres");

        RuleFor(x => x.Telefono)
            .MaximumLength(30)
                .WithMessage("El teléfono no puede exceder 30 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Telefono));

        RuleFor(x => x.Direccion)
            .MaximumLength(255)
                .WithMessage("La direccion no puede exceder los 255 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Direccion));


    }
}