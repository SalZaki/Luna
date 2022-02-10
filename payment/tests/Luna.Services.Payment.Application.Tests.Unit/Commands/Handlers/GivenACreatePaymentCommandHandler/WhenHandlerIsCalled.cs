using System.Reflection;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Luna.Services.Payment.Application.Commands;
using Luna.Services.Payment.Application.Commands.Handlers;
using Luna.Services.Payment.Application.Data;
using Luna.Services.Payment.Application.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Luna.Services.Payment.Application.Tests.Unit.Commands.Handlers.GivenACreatePaymentCommandHandler;

[Trait("Payment Command Handler", "Unit Tests")]
public class WhenHandleIsCalled : IAsyncLifetime
{
  private readonly CreatePaymentCommandHandler _sut;

  private readonly Mock<IValidator<CreatePaymentCommand>> _mockValidator;

  private readonly Mock<ILogger<CreatePaymentCommandHandler>> _mockLogger;

  private readonly Mock<IMediator> _mockMediator;

  private readonly Mock<ILunaPaymentRepository> _mockLunaPaymentRepository;

  private readonly CreatePaymentCommand _createPaymentCommand;

  private PaymentDto _actualPaymentResponse;

  private readonly PaymentDto _expectedPaymentResponse;

  public WhenHandleIsCalled()
  {
    // Arrange
    var fixture = new Fixture().Customize(new AutoMoqCustomization());
    fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
    fixture.Behaviors.Add(new OmitOnRecursionBehavior());

    // Logger
    _mockLogger = fixture.Create<Mock<ILogger<CreatePaymentCommandHandler>>>();
    fixture.Freeze<Mock<ILoggerFactory>>(composer => composer.Do(
      mock => mock
        .Setup(f => f.CreateLogger(It.IsAny<string>()))
        .Returns(_mockLogger.Object)
        .Verifiable()));

    // BankResponse
    var expectedBankResponse = fixture.Create<BankResponseDto>();

    // MediatR
    _mockMediator = fixture.Freeze<Mock<IMediator>>(composer => composer.Do(
      mock => mock
        .Setup(mediator =>
          mediator.Send(It.IsAny<CreateBankChargeCommand>(), It.IsAny<CancellationToken>()))!
        .ReturnsAsync(expectedBankResponse)
        .Verifiable()));

    // LunaPaymentRepository
    var payment = fixture.Create<Domain.Entities.Payment>();

    _mockLunaPaymentRepository = fixture.Freeze<Mock<ILunaPaymentRepository>>();
    _mockLunaPaymentRepository
      .Setup(repository =>
        repository.SaveAsync(It.IsAny<Domain.Entities.Payment>(), It.IsAny<CancellationToken>()))!
      .ReturnsAsync(payment)
      .Verifiable();

    // Validator
    _mockValidator = fixture.Freeze<Mock<IValidator<CreatePaymentCommand>>>();
    _mockValidator
      .Setup(validator => validator.ValidateAsync(
        It.IsAny<CreatePaymentCommand>(),
        It.IsAny<CancellationToken>()))
      .ReturnsAsync(new ValidationResult())
      .Verifiable();

    _createPaymentCommand = fixture.Create<CreatePaymentCommand>();

    _expectedPaymentResponse = fixture.Build<PaymentDto>()
      .With(x => x.Id, payment.Id)
      .With(x => x.Amount, payment.Amount)
      .With(x => x.BankCode, payment.BankResponse.BankCode)
      .With(x => x.BankReason, payment.BankResponse.Reason)
      .With(x => x.BankStatus, payment.BankResponse.Status)
      .With(x => x.Card, fixture.Build<CardDto>()
        .With(c => c.CardType, payment.Card.CardType)
        .With(x => x.Cvv, payment.Card.Cvv)
        .With(x => x.ExpMonth, payment.Card.ExpMonth)
        .With(x => x.ExpYear, payment.Card.ExpYear)
        .With(x => x.NameOnCard, payment.Card.NameOnCard)
        .With(x => x.Number, payment.Card.Number)
        .Create())
      .With(x => x.Currency, payment.Currency)
      .With(x => x.EstimatedSettlementCost, payment.EstimatedSettlementCost)
      .With(x => x.FinalisedOn, payment.FinalisedOn)
      .With(x => x.IdempotentKey, payment.IdempotentKey)
      .With(x => x.MerchantId, payment.MerchantId)
      .With(x => x.MetaData, payment.Metadata.Select(x => new MetadataDto {Name = x.Name, Value = x.Value}).ToArray())
      .With(x => x.Status, payment.Status)
      .With(x => x.SubmittedOn, payment.SubmittedOn)
      .With(x => x.UpdatedOn, payment.UpdatedOn)
      .Create();

    // CreateBankChargeCommandHandler
    _sut = fixture.Create<CreatePaymentCommandHandler>();
  }

  public async Task InitializeAsync()
  {
    // Act
    _actualPaymentResponse = await _sut.Handle(_createPaymentCommand, CancellationToken.None);
  }

  public Task DisposeAsync() => Task.CompletedTask;

  [Fact]
  public void Then_Validates_Handler_Constructor_Has_Null_Guards()
  {
    // Assert
    var fixture = new Fixture().Customize(new AutoMoqCustomization());
    var assertion = new GuardClauseAssertion(fixture);
    assertion.Verify(typeof(CreateBankChargeCommandHandler).GetConstructors(BindingFlags.Public));
  }

  [Fact]
  public void Then_Validates_Command()
  {
    // Assert
    _mockValidator.Verify(
      x => x.ValidateAsync(It.IsAny<CreatePaymentCommand>(), It.IsAny<CancellationToken>()),
      Times.Once);
  }

  [Fact]
  public void Then_Invokes_Mediator()
  {
    // Assert
    _mockMediator.Verify(x => x.Send(It.IsAny<CreateBankChargeCommand>(), It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public void Then_Logs_Debug_Message()
  {
    // Assert
    _mockLogger.Verify(
      x => x.Log(
        LogLevel.Debug,
        It.IsAny<EventId>(),
        It.IsAny<It.IsAnyType>(),
        It.IsAny<Exception>(),
        (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()),
      Times.AtLeastOnce);
  }

  [Fact]
  public void Then_Saves_To_Repository()
  {
    // Assert
    _mockLunaPaymentRepository.Verify(
      x => x.SaveAsync(It.IsAny<Domain.Entities.Payment>(), It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public void And_Then_Returns_Expected_PaymentResponse()
  {
    // Assert
    _actualPaymentResponse.Should().NotBeNull();
    _actualPaymentResponse.Should().BeEquivalentTo(_expectedPaymentResponse);
  }
}
