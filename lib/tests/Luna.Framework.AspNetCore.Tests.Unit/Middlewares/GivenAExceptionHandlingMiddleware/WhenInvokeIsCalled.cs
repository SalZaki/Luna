using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentValidation;
using Luna.Framework.AspNetCore.Middlewares;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Luna.Framework.AspNetCore.Tests.Unit.Middlewares.GivenAExceptionHandlingMiddleware;

public sealed class WhenInvokeAsyncIsCalled
{
  private readonly Mock<ILogger<ExceptionHandlingMiddleware>> _mockLogger;

  private readonly Mock<ILoggerFactory> _mockLoggerFactory;

  private readonly Mock<RequestDelegate> _next;

  private readonly ExceptionHandlingMiddleware _sut;

  private readonly HttpContext _httpContext;

  private readonly IOptions<ApiSettings> _apiSettingsOptions;

  private readonly Mock<IHostingEnvironment> _mockHostingEnvironment;

  public WhenInvokeAsyncIsCalled()
  {
    var fixture = new Fixture().Customize(new AutoMoqCustomization());
    _httpContext = new DefaultHttpContext();
    _apiSettingsOptions = Options.Create(new ApiSettings {Name = "K3 Data Exchange Event Centre"});
    _mockHostingEnvironment = fixture.Create<Mock<IHostingEnvironment>>();
    // Logger
    _mockLogger = fixture.Create<Mock<ILogger<ExceptionHandlingMiddleware>>>();
    _mockLoggerFactory = fixture.Freeze<Mock<ILoggerFactory>>(composer => composer.Do(
      mock => mock
        .Setup(f => f.CreateLogger(It.IsAny<string>()))
        .Returns(_mockLogger.Object)
        .Verifiable()));

    _next = fixture.Freeze<Mock<RequestDelegate>>();

    _sut = new ExceptionHandlingMiddleware(_apiSettingsOptions, _mockHostingEnvironment.Object, _next.Object,
      _mockLoggerFactory.Object);
  }

  [Fact]
  public async Task GivenValidHttpContext_WhenInvoke_ThenInvokeSuccessful()
  {
    // Arrange
    var context = new DefaultHttpContext();

    // Act
    await _sut.InvokeAsync(context);

    // Assert
    _next.Verify(n => n.Invoke(It.IsAny<DefaultHttpContext>()), Times.Once);
  }

  [Theory]
  [MemberData(nameof(Data))]
  public async Task GivenException_WhenInvoke_ThenHandleTheException(Exception exception, int statusCodeExpected)
  {
    // Arrange
    var context = new DefaultHttpContext
    {
      Response =
      {
        Body = new MemoryStream()
      }
    };

    _next.Setup(n => n.Invoke(context))
      .Throws(exception)
      .Verifiable();

    // Act
    await _sut.InvokeAsync(context);

    // Assert
    Assert.Equal(context?.Response?.StatusCode, statusCodeExpected);
    _next.Verify();
  }

  public static IEnumerable<object[]> Data =>
    new List<object[]>
    {
      new object[] {new ValidationException("Validation error occured"), StatusCodes.Status400BadRequest},
      new object[] {new Exception(), StatusCodes.Status500InternalServerError}
    };
}
