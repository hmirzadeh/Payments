using FluentValidation;
using PaymentGateway.Application.Features.Payment.CommandModels;

namespace PaymentGateway.Application.Common.Validators;
/// <summary>
/// A Class using Fluent validation to validate values of a payment to be processed
/// </summary>
public class PaymentValidator : AbstractValidator<ProcessPaymentCommand>
{
    public PaymentValidator()
    {
        RuleFor(v => v.Amount)
            .GreaterThan(0);


        RuleFor(v => v.CardNumber)
            .MinimumLength(15)
            .MaximumLength(19)
            .NotEmpty();

        RuleFor(v => v.ExpiryMonth)
            .NotEmpty()
            .GreaterThan(0)
            .LessThan(13);

        RuleFor(v => v.ExpiryYear)
            .NotEmpty()
            .GreaterThanOrEqualTo(Convert.ToInt16(DateTime.Now.Year.ToString().Substring(2, 2)));

        RuleFor(v => v.CVV)
            .NotEmpty()
            .MaximumLength(3)
            .MinimumLength(3);
    }
}