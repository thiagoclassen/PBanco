using Clients.API.Client.CreateClient;
using Clients.API.Client.Models;
using ErrorOr;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace Clients.API.UnitTesting.Client.CreateClient;

public class CreateClientEndpointTest
{
    private readonly CreateClientEndpoint _endpoint;
    private readonly ISender _sender = Substitute.For<ISender>();

    public CreateClientEndpointTest()
    {
        _endpoint = new CreateClientEndpoint(_sender);
    }

    [Fact]
    public async Task CreateClient_ShouldCreateClient_WhenValidRequest()
    {
        // Arrange
        var clientId = Guid.NewGuid();

        var clientRequest = new CreateClientRequest
        {
            Name = "John Doe",
            Email = "johndoe@gmail.com",
            CPF = "111.222.333-44",
            BirthDate = new DateTime(1990, 1, 1)
        };

        var client = new BankClient
        {
            Id = clientId,
            Name = clientRequest.Name,
            Email = clientRequest.Email,
            CPF = clientRequest.CPF,
            BirthDate = clientRequest.BirthDate
        };

        var clientResult = new CreateClientResult(clientId);

        var createClientResult = new CreateClientResult(clientId);

        _sender.Send(Arg.Any<CreateClientCommand>(), Arg.Any<CancellationToken>())
            .Returns(clientResult.ToErrorOr());
        // Act
        var result = (CreatedResult)await _endpoint.CreateClient(clientRequest, CancellationToken.None);

        // Assert
        var createClientResponse = (CreateClientResult)result.Value!;
        result.StatusCode.Should().Be(201);
        createClientResponse.ClientId.Should().Be(createClientResult.ClientId);
    }

    [Fact]
    public async Task CreateClient_ShouldFail_WhenInvalidRequest()
    {
        // Arrange
        var clientRequest = new CreateClientRequest
        {
            Name = "John Doe",
            Email = "johndoe@gmail.com",
            CPF = "00",
            BirthDate = new DateTime()
        };

        //var createClientResult = new CreateClientResult(Guid.NewGuid());

        _sender.Send(Arg.Any<CreateClientCommand>(), Arg.Any<CancellationToken>())
            .Returns(Error.Validation().ToErrorOr<CreateClientResult>());
        // Act
        var result = (ObjectResult)await _endpoint.CreateClient(clientRequest, CancellationToken.None);

        // Assert

        result.Value.Should().BeOfType<ValidationProblemDetails>();
        result.Value.As<ValidationProblemDetails>().Errors.Should().NotBeEmpty();
    }
}