using FluentValidation;

namespace Luna.Services.Payment.Application.Commands.Validators;

public sealed  class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
  public CreatePaymentCommandValidator()
  {
    RuleFor(x => x.Amount)
      .NotNull()
      .NotEmpty()
      .WithMessage("Amount can't be null or empty.")
      .WithSeverity(Severity.Error)
      .Configure(x => { x.CascadeMode = CascadeMode.Stop; });

    RuleFor(x => x.Number)
      .NotEmpty().WithMessage("Card number can't be null or empty.")
      .WithSeverity(Severity.Error)
      .Configure(x => { x.CascadeMode = CascadeMode.Stop; });

    RuleFor(x => x.ExpMonth)
      .NotEmpty().WithMessage("Card expiry month can't be null or empty.")
      .WithSeverity(Severity.Error)
      .Configure(x => { x.CascadeMode = CascadeMode.Stop; });

    RuleFor(x => x.ExpYear)
      .NotEmpty().WithMessage("Card expiry year can't be null or empty.")
      .WithSeverity(Severity.Error)
      .Configure(x => { x.CascadeMode = CascadeMode.Stop; });

    RuleFor(x => x.Cvv)
      .NotEmpty().WithMessage("Card cvv can't be null or empty.")
      .WithSeverity(Severity.Error)
      .Configure(x => { x.CascadeMode = CascadeMode.Stop; });

  }
}
