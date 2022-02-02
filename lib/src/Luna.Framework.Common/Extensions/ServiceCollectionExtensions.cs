using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Luna.Common.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddCQRS(
    this IServiceCollection services,
    Assembly[]? assemblies = null,
    Action<IServiceCollection>? buildActions = null)
  {
    services.AddMediatR(assemblies ?? new[] {Assembly.GetExecutingAssembly()});

    return services;
  }
}
