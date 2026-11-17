using FluentValidation;
using Portafolio.API.Dtos;

namespace Portafolio.API.Validators;

public class ProveedorCreateValidator : AbstractValidator<ProveedorCreateDto>
{
    public ProveedorCreateValidator()
    {
        RuleFor(x => x.NombreEmpresa)
            .NotEmpty()
                .WithMessage("El nombreempresa no puede estar vacio")
            .MaximumLength(150)
                .WithMessage("El nombreempresa no puede exceder de los 150 caracteres")
            .MinimumLength(2)
                .WithMessage("El nombreempresa no puede ser menor a 2 caracteres");

        RuleFor(x => x.Contacto)
            .MaximumLength(255)
                .WithMessage("El contacto no puede exceder de los 255 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Contacto));

        RuleFor(x => x.Email)
            .MaximumLength(150)
                .WithMessage("El email no puede exceder de los 150 caracteres")
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Telefono)
            .MaximumLength(255)
                .WithMessage("El telefono no puede exceder los 255 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Telefono));
    }
}

public class ProveedorUpdateValidator : AbstractValidator<ProveedorUpdateDto>
{
    public ProveedorUpdateValidator()
    {

        RuleFor(x => x.NombreEmpresa)
            .NotEmpty()
                .WithMessage("El nombreempresa no puede estar vacio")
            .MaximumLength(150)
                .WithMessage("El nombreempresa no puede exceder de los 150 caracteres")
            .MinimumLength(2)
                .WithMessage("El nombreempresa no puede ser menor a 2 caracteres");

        RuleFor(x => x.Contacto)
            .MaximumLength(255)
                .WithMessage("El contacto no puede exceder de los 255 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Contacto));

        RuleFor(x => x.Email)
            .MaximumLength(150)
                .WithMessage("El email no puede exceder de los 150 caracteres")
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Telefono)
            .MaximumLength(255)
                .WithMessage("El telefono no puede exceder los 255 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Telefono));

    }
}