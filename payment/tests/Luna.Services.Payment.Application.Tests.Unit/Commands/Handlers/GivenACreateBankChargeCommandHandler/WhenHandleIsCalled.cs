using System.Reflection;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using FluentAssertions;
using Luna.Services.Payment.Application.Commands;
using Luna.Services.Payment.Application.Commands.Handlers;
using Luna.Services.Payment.Application.Dtos;
using Luna.Services.Payment.Application.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Luna.Services.Payment.Application.Tests.Unit.Commands.Handlers.GivenACreateBankChargeCommandHandler;

[Trait("Bank Charge Command Handler", "Unit Tests")]
public class WhenHandleIsCalled : IAsyncLifetime
{
  private readonly CreateBankChargeCommandHandler _sut;

  private readonly Mock<ILogger<CreateBankChargeCommandHandler>> _mockLogger;

  private readonly Mock<IAcquirerBankApiClient> _mockAcquirerBankApiClient;

  private readonly CreateBankChargeCommand _createBankChargeCommand;

  private BankResponseDto _actualBankResponse;

  private readonly BankResponseDto _expectedBankResponse;

  public WhenHandleIsCalled()
  {
    // Arrange
    var fixture = new Fixture().Customize(new AutoMoqCustomization());

    // Logger
    _mockLogger = fixture.Create<Mock<ILogger<CreateBankChargeCommandHandler>>>();
    fixture.Freeze<Mock<ILoggerFactory>>(composer => composer.Do(
      mock => mock
        .Setup(f => f.CreateLogger(It.IsAny<string>()))
        .Returns(_mockLogger.Object)
        .Verifiable()));

    // BankResponse
    _expectedBankResponse = fixture.Create<BankResponseDto>();

    // AcquirerBankApiClient
    _mockAcquirerBankApiClient = fixture.Freeze<Mock<IAcquirerBankApiClient>>(composer => composer.Do(
      mock => mock
        .Setup(acquirerBankApiClient =>
          acquirerBankApiClient.PostChargeAsync(It.IsAny<BankRequestDto>(), It.IsAny<CancellationToken>()))!
        .ReturnsAsync(_expectedBankResponse)
        .Verifiable()));

    _createBankChargeCommand = fixture.Create<CreateBankChargeCommand>();

    // CreateBankChargeCommandHandler
    _sut = fixture.Create<CreateBankChargeCommandHandler>();
  }

  public async Task InitializeAsync()
  {
    // Act
    _actualBankResponse = await _sut.Handle(_createBankChargeCommand, CancellationToken.None);
  }

  public Task DisposeAsync() => Task.CompletedTask;

  [Fact]
  public void Then_Validates_Handler_Constructor_Has_Null_Guards()
  {
    var fixture = new Fixture().Customize(new AutoMoqCustomization());
    var assertion = new GuardClauseAssertion(fixture);
    assertion.Verify(typeof(CreateBankChargeCommandHandler).GetConstructors(BindingFlags.Public));
  }

  [Fact]
  public void Then_Calls_AcquirerBank_Api_Client()
  {
    // Assert
    _mockAcquirerBankApiClient
      .Verify(x => x.PostChargeAsync(It.IsAny<BankRequestDto>(), It.IsAny<CancellationToken>()),
        Times.AtLeastOnce);
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
  public void And_Then_Returns_Expected_BankResponse()
  {
    // Assert
    _actualBankResponse.Should().NotBeNull();
    _actualBankResponse.BankCode.Should().Be(_expectedBankResponse.BankCode);
    _actualBankResponse.Reason.Should().Be(_expectedBankResponse.Reason);
    _actualBankResponse.Status.Should().Be(_expectedBankResponse.Status);
    _actualBankResponse.TransactionId.Should().Be(_expectedBankResponse.TransactionId);
  }
}
