using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace Luna.Framework.AspNetCore;

public class IdempotentKeyFilter : IOperationFilter
{
  public void Apply(OpenApiOperation operation, OperationFilterContext context)
  {
    if (!IsPostMethod(context)) return;

    operation.Parameters ??= new List<OpenApiParameter>();
    operation.Parameters.Add(new OpenApiParameter
    {
      Name = Constants.RequestHeaderKeys.IdempotentKey,
      Description =
        "Idempotent key is a unique id based on UUID [RFC4122] for creating a payment request, which clients can send. If two requests are received with the same id, only the first one will be processed. " +
        "Uniqueness of the key MUST be implemented by the clients. It is RECOMMENDED that UUID [RFC4122] or a similar random identifier be used as an idempotency key. At the moment Luna Payments is not enforcing " +
        "any time based expiration policy for the idempotent keys.",
      In = ParameterLocation.Header,
      AllowEmptyValue = false,
      Example = new OpenApiString(Guid.NewGuid().ToString()),
      Required = false,
      Schema = new OpenApiSchema {Type = "string"}
    });
  }

  private static bool IsPostMethod(OperationFilterContext context)
  {
    return string.Equals(context.ApiDescription.HttpMethod, HttpMethod.Post.Method,
      StringComparison.InvariantCultureIgnoreCase);
  }
}
