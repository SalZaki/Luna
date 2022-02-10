using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using FluentAssertions;
using Luna.Framework.AspNetCore.Dtos;
using Luna.Services.Payment.Api.Tests.Shared.Helpers;
using Luna.Services.Payment.Api.v1.Controllers;
using Luna.Services.Payment.Application.Dtos;
using Luna.Services.Payment.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Luna.Services.Payment.Api.Tests.Unit.v1.Controllers.GivenAPaymentController;

[Trait("Payment Controller", "Unit Tests")]
public class WhenGetByIdAsyncIsCalled : IAsyncLifetime
{
  private readonly Type _expectedTyped;

  private readonly ResponseDto<PaymentDto> _expectedPaymentResponse;

  private readonly Guid _paymentId;

  private const int ExpectedOkStatusCode = 200;

  private const int ExpectedBadRequestStatusCode = 400;

  private const int ExpectedStatus401Unauthorized = 401;

  private const int ExpectedInternalServerErrorStatusCode = 500;

  private readonly Mock<IMediator> _mockMediator;

  private readonly Mock<ILogger<PaymentsController>> _mockLogger;

  private readonly PaymentsController _sut;

  private IActionResult _actualResponse;

  public WhenGetByIdAsyncIsCalled()
  {
    // Arrange
    var fixture = new Fixture().Customize(new AutoMoqCustomization());

    _expectedTyped = typeof(ResponseDto<PaymentDto>);
    _paymentId = fixture.Create<Guid>();

    _expectedPaymentResponse = fixture
      .Build<ResponseDto<PaymentDto>>()
      .With(x => x.Status, "Success")
      .With(x => x.Version, "1.0")
      .Create();

    // Logger
    _mockLogger = fixture.Create<Mock<ILogger<PaymentsController>>>();
    fixture.Freeze<Mock<ILoggerFactory>>(composer => composer.Do(
      mock => mock
        .Setup(f => f.CreateLogger(It.IsAny<string>()))
        .Returns(_mockLogger.Object)));

    // MediatR
    _mockMediator = fixture.Freeze<Mock<IMediator>>(composer => composer.Do(
      mock => mock
        .Setup(mediator => mediator.Send(It.IsAny<GetPaymentByIdQuery>(), It.IsAny<CancellationToken>()))!
        .ReturnsAsync(_expectedPaymentResponse.Data)
        .Verifiable()));

    _sut = fixture
      .Build<PaymentsController>()
      .OmitAutoProperties()
      .Create();
  }

  public async Task InitializeAsync()
  {
    // Act
    _actualResponse = await _sut.GetByIdAsync(_paymentId, CancellationToken.None);
  }

  public Task DisposeAsync() => Task.CompletedTask;

  [Fact]
  public void Then_Validates_Controller_Constructor_Has_Null_Guards()
  {
    var fixture = new Fixture().Customize(new AutoMoqCustomization());
    var assertion = new GuardClauseAssertion(fixture);
    assertion.Verify(typeof(PaymentsController).GetConstructors(BindingFlags.Public));
  }

  [Fact]
  public void Then_Response_Attribute_Has_200_Status_Code()
  {
    // Act
    var attributes = _sut
      .GetAttributesOn(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
      .OfType<ProducesResponseTypeAttribute>();

    // Asserts
    var producesResponseTypeAttributes = attributes as ProducesResponseTypeAttribute[] ?? attributes.ToArray();
    producesResponseTypeAttributes.Should().NotBeNull();
    producesResponseTypeAttributes.Length.Should().BeGreaterThan(0);
    producesResponseTypeAttributes.Select(x => x.StatusCode == ExpectedOkStatusCode).Should().NotBeNull();
  }

  [Fact]
  public void Then_Response_Attribute_Has_400_Status_Code()
  {
    // Act
    var attributes = _sut
      .GetAttributesOn(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
      .OfType<ProducesResponseTypeAttribute>();

    // Asserts
    var producesResponseTypeAttributes = attributes as ProducesResponseTypeAttribute[] ?? attributes.ToArray();
    producesResponseTypeAttributes.Should().NotBeNull();
    producesResponseTypeAttributes.Length.Should().BeGreaterThan(0);
    producesResponseTypeAttributes.Select(x => x.StatusCode == ExpectedBadRequestStatusCode).Should()
      .NotBeNull();
  }

  [Fact]
  public void Then_Response_Attribute_Has_401_Status_Code()
  {
    // Act
    var attributes = _sut
      .GetAttributesOn(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
      .OfType<ProducesResponseTypeAttribute>();

    // Asserts
    var producesResponseTypeAttributes = attributes as ProducesResponseTypeAttribute[] ?? attributes.ToArray();
    producesResponseTypeAttributes.Should().NotBeNull();
    producesResponseTypeAttributes.Length.Should().BeGreaterThan(0);
    producesResponseTypeAttributes.Select(x => x.StatusCode == ExpectedStatus401Unauthorized).Should()
      .NotBeNull();
  }

  [Fact]
  public void Then_Response_Attribute_Has_500_Status_Code()
  {
    // Act
    var attributes = _sut
      .GetAttributesOn(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
      .OfType<ProducesResponseTypeAttribute>();

    // Asserts
    var producesResponseTypeAttributes = attributes as ProducesResponseTypeAttribute[] ?? attributes.ToArray();
    producesResponseTypeAttributes.Should().NotBeNull();
    producesResponseTypeAttributes.Length.Should().BeGreaterThan(0);
    producesResponseTypeAttributes.Select(x => x.StatusCode == ExpectedInternalServerErrorStatusCode).Should()
      .NotBeNull();
  }

  [Fact]
  public void Then_Response_Attribute_Has_PaymentDto_Return_Type()
  {
    // Act
    var attributes = _sut
      .GetAttributesOn(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
      .OfType<ProducesResponseTypeAttribute>();

    // Assert
    var producesResponseTypeAttributes = attributes as ProducesResponseTypeAttribute[] ?? attributes.ToArray();
    producesResponseTypeAttributes.Should().NotBeNull();
    producesResponseTypeAttributes.Length.Should().BeGreaterThan(0);
    producesResponseTypeAttributes.Select(x => x.Type == _expectedTyped).Should().NotBeNull();
  }

  [Fact]
  public void Then_Invokes_Mediator()
  {
    // Assert
    _mockMediator.Verify(x => x.Send(It.IsAny<GetPaymentByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public void Then_Logs_Information_Message()
  {
    // Assert
    _mockLogger.Verify(
      x => x.Log(
        LogLevel.Information,
        It.IsAny<EventId>(),
        It.IsAny<It.IsAnyType>(),
        It.IsAny<Exception>(),
        (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()),
      Times.AtLeastOnce);
  }

  [Fact]
  public void Then_Returns_200_Status_Code()
  {
    // Assert
    var response = _actualResponse.Should().BeOfType<OkObjectResult>().Subject;
    response.StatusCode.Should().Be(ExpectedOkStatusCode);
  }

  [Fact]
  public void And_Then_Returns_Expected_Payment()
  {
    // Assert
    var response = _actualResponse.Should().BeOfType<OkObjectResult>().Subject;
    var actualPaymentResponse = response.Value.Should().BeAssignableTo<ResponseDto<PaymentDto>>().Subject;
    actualPaymentResponse.Should().NotBeNull();
    actualPaymentResponse.Data.Should().NotBeNull();
    actualPaymentResponse.Data.Id.Should().Be(_expectedPaymentResponse.Data.Id);
    actualPaymentResponse.Data.Status.Should().Be(_expectedPaymentResponse.Data.Status);
    actualPaymentResponse.Data.IdempotentKey.Should().Be(_expectedPaymentResponse.Data.IdempotentKey);
    actualPaymentResponse.Data.BankCode.Should().Be(_expectedPaymentResponse.Data.BankCode);
    actualPaymentResponse.Data.BankReason.Should().Be(_expectedPaymentResponse.Data.BankReason);
    actualPaymentResponse.Data.BankStatus.Should().Be(_expectedPaymentResponse.Data.BankStatus);
    actualPaymentResponse.Data.EstimatedSettlementCost.Should()
      .Be(_expectedPaymentResponse.Data.EstimatedSettlementCost);
    actualPaymentResponse.Data.FinalisedOn.Should().Be(_expectedPaymentResponse.Data.FinalisedOn);
    actualPaymentResponse.Data.SubmittedOn.Should().Be(_expectedPaymentResponse.Data.SubmittedOn);
    actualPaymentResponse.Data.UpdatedOn.Should().Be(_expectedPaymentResponse.Data.UpdatedOn);
    actualPaymentResponse.Status.Should().Be(_expectedPaymentResponse.Status);
    actualPaymentResponse.Version.Should().Be(_expectedPaymentResponse.Version);
  }
}
