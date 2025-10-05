using BuildingBlocks.CQRS;
using Clients.API.Client.Models;
using Clients.API.Client.Persistence;
using FluentValidation;

namespace Clients.API.Client.CreateClient;

public record CreateClientCommand(
    string Name,
    string Email,
    string CPF,
    DateTime BirthDate)
    : ICommand<ErrorOr<BankClient>>;

public record CreateClientResult(Guid ClientId);

public class CreateClientCommandHandler(IClientRepository repository)
    : ICommandHandler<CreateClientCommand, ErrorOr<BankClient>>
{
    public async Task<ErrorOr<BankClient>> Handle(CreateClientCommand command, CancellationToken cancellationToken)
    {
        var client = BankClient.Create(command.Name, command.Email, command.CPF, command.BirthDate);

        await repository.AddAsync(client, cancellationToken);

        return client;
    }
}

public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
    public CreateClientCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(50);
        RuleFor(x => x.CPF)
            .NotEmpty()
            .Matches(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$")
            .WithMessage("CPF must be in the format XXX.XXX.XXX-XX")
            .MaximumLength(14)
            .WithMessage("CPF must not exceed 14 characters.");
        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .LessThan(DateTime.Now).WithMessage("BirthDate must be in the past.")
            .Must(BeAtLeast18YearsOld)
            .WithMessage("Not age legal.");
    }

    private static bool BeAtLeast18YearsOld(DateTime birthDate)
    {
        // Todo check this Age validator
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;

        if (birthDate.Date > today.AddYears(-age)) age--;

        return age >= 18;
    }
}