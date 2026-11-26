using FluentValidation;
using Portafolio.API.Dtos;

namespace Portafolio.API.Validators;

public class ProductoCreateValidator : AbstractValidator<ProductoCreateDto>
{
    public ProductoCreateValidator()
    {
        RuleFor(x => x.Sku)
            .NotEmpty()
                .WithMessage("El sku del producto no puede estar vacio")
            .MaximumLength(255)
                .WithMessage("El sku no puede exceder de los 255 caracteres");

        RuleFor(x => x.Nombre)
            .NotEmpty()
                .WithMessage("El nombre del producto no puede estar vacio")
            .MaximumLength(255)
                .WithMessage("El nombre no puede exceder de los 255 caracteres")
            .MinimumLength(2)
                .WithMessage("El nombre debe tener al menos 2 caracteres");

        RuleFor(x => x.Descripcion)
            .MaximumLength(255)
                .WithMessage("La descripcion no puede exceder de los 255 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Descripcion));

        RuleFor(x => x.Precio)
            .GreaterThanOrEqualTo(0)
                .WithMessage("El Precio no puede ser negativo");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0)
                .WithMessage("El Stock no puede ser negativo"); ;
    }
}

public class ProductoUpdateValidator : AbstractValidator<ProductoUpdateDto>
{
    public ProductoUpdateValidator()
    {

        RuleFor(x => x.Nombre)
            .NotEmpty()
                .WithMessage("El nombre no puede estar vacio")
            .MaximumLength(255)
                .WithMessage("El nombre no puede ser mayor a 255 caracteres")
            .MinimumLength(2)
                .WithMessage("El nombre debe de contener al menos 2 caracteres");
        RuleFor(x => x.Precio)
            .GreaterThanOrEqualTo(0)
                .WithMessage("El Precio no puede ser negativo"); ;
        RuleFor(x => x.Stock)
            .NotNull().WithMessage("El Stock no puede ser null")
        .GreaterThanOrEqualTo(0)
            .WithMessage("El Stock no puede ser negativo"); ;
        RuleFor(x => x.Descripcion)
            .MaximumLength(255).WithMessage("La descripcion no puede ser mayor a 255 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Descripcion));

    }
}