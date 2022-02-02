using FluentValidation;
using Luna.Services.Payment.Application.Queries;

namespace Luna.Services.Payment.Application.Validators;

public sealed class GetPaymentByIdQueryValidator : AbstractValidator<GetPaymentByIdQuery>
{
  public GetPaymentByIdQueryValidator()
  {
    RuleFor(x => x.PaymentId)
      .NotNull()
      .NotEmpty()
      .WithMessage("Payment id can't be null or empty.")
      .WithSeverity(Severity.Error)
      .Configure(x => { x.CascadeMode = CascadeMode.Stop; });

    RuleFor(x => x.PaymentId)
      .Must(x => Guid.TryParse(x.ToString(), out var guid))
      .WithMessage("Payment id is invalid.")
      .WithSeverity(Severity.Error)
      .Configure(x => { x.CascadeMode = CascadeMode.Stop; });
  }
}
