using Luna.Common.Infrastructure.Extensions;
using Luna.Framework.AspNetCore;
using Luna.Framework.AspNetCore.Extensions;
using Luna.Framework.AspNetCore.Resolvers;
using Luna.Services.Payment.Api;
using Luna.Services.Payment.Api.Extensions;
using Luna.Services.Payment.Api.StartUps;
using Luna.Services.Payment.Application.Services;
using Luna.Services.Payment.Application.Data;
using Luna.Services.Payment.Infrastructure.Clients;
using Luna.Services.Payment.Infrastructure.Data;
using Luna.Services.Payment.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Api settings
builder.Services.AddApiSettings(builder.Configuration);

// HttpClient settings
builder.Services.AddHttpClientSettings(builder.Configuration);

// Api version
builder.Services.AddApiVersionCore(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAllOrigins",
    builder => builder.AllowAnyOrigin()
      .AllowAnyMethod()
      .AllowAnyHeader()
      .AllowCredentials());
});

// Swagger
builder.Services.AddSwaggerGen(swagger =>
{
  swagger.OperationFilter<IdempotentKeyFilter>();

  swagger.SwaggerDoc("v1", new OpenApiInfo
  {
    Title = "Luna Payment Api",
    Version = "v1"
  });

  swagger.AddSecurityDefinition(Constants.RequestHeaderKeys.ApiKey, new OpenApiSecurityScheme
  {
    Name = Constants.RequestHeaderKeys.ApiKey,
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.ApiKey,
    Description = "Publicly known api key defined per Luna client and per Luna environment, i.e dev, sand-box or prd"
  });

  swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
  {
    {
      new OpenApiSecurityScheme
      {
        Name = Constants.RequestHeaderKeys.ApiKey,
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Reference = new OpenApiReference
        {
          Type = ReferenceType.SecurityScheme,
          Id = Constants.RequestHeaderKeys.ApiKey
        }
      },
      new [] {"NXyRtwK27y66shI"}
    }
  });
});

// Dependencies
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IIdempotentKeyResolver, IdempotentKeyResolver>();
builder.Services.AddTransient<IApiKeyResolver, ApiKeyResolver>();
builder.Services.AddTransient<IRequestMaskingService, RequestMaskingService>();

builder.Services.AddAcquirerBankHttpClient();
builder.Services.AddApiValidators();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ILunaPaymentRepository, LunaPaymentRepository>();
builder.Services.AddDbContextFactory<LunaPaymentDbContext>(options =>
{
  options.UseInMemoryDatabase(Guid.NewGuid().ToString());
  options.EnableSensitiveDataLogging();
});

// MediatR
builder.Services.AddCQRS(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
  app.UseLunaSwagger();
}

app.UseHttpsRedirection();

// Middlewares
app.UseLoggingMiddleware();
app.UseIdempotentMiddleware();
app.UseExceptionHandlingMiddleware();
app.UseSecurityHeadersMiddleware();
app.UseApiKeyMiddleware();

app.UseAuthorization();
app.MapControllers();

app.Run();
