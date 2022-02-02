using FluentValidation;
using Luna.Services.Payment.Application.Queries;

namespace Luna.Services.Payment.Application.Validators;

public sealed class GetPaymentByIdempotentKeyQueryValidator : AbstractValidator<GetPaymentByIdempotentKeyQuery>
{
  public GetPaymentByIdempotentKeyQueryValidator()
  {
    RuleFor(x => x.IdempotentKey)
      .NotNull()
      .NotEmpty()
      .WithMessage("Idempotent key can't be null or empty.")
      .WithSeverity(Severity.Error)
      .Configure(x => { x.CascadeMode = CascadeMode.Stop; });

    RuleFor(x => x.IdempotentKey)
      .Must(x => Guid.TryParse(x.ToString(), out var guid))
      .WithMessage("Idempotent key is invalid.")
      .WithSeverity(Severity.Error)
      .Configure(x => { x.CascadeMode = CascadeMode.Stop; });
  }
}
