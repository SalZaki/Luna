using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Luna.Services.Payment.Application.Commands;
using Luna.Services.Payment.Application.Commands.Validators;
using Luna.Services.Payment.Application.Queries;
using Luna.Services.Payment.Application.Validators;

namespace Luna.Services.Payment.Api.StartUps;

[ExcludeFromCodeCoverage]
public static class ValidatorStartup
{
  public static IServiceCollection AddApiValidators(this IServiceCollection services)
  {
    services.AddTransient<IValidator<CreatePaymentCommand>, CreatePaymentCommandValidator>();
    services.AddTransient<IValidator<GetPaymentByIdQuery>, GetPaymentByIdQueryValidator>();
    services.AddTransient<IValidator<GetPaymentByIdempotentKeyQuery>, GetPaymentByIdempotentKeyQueryValidator>();

    return services;
  }
}
