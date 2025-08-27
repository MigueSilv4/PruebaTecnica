using FluentValidation;
using PruebaTecnica.Models;

namespace PruebaTecnica.Services
{
    public class CardValidator : AbstractValidator<Card>
    {
        public CardValidator()
        {
            RuleFor(c => c.CustomerName)
                .NotEmpty().WithMessage("El nombre del cliente es requerido.")
                .NotNull().WithMessage("El nombre del cliente no puede ser nulo.");

            RuleFor(c => c.CardNumber)
                .NotEmpty().WithMessage("El número de tarjeta es requerido.")
                .NotNull().WithMessage("El número de tarjeta no puede ser nulo.");
        }
    }
}
    