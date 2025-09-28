using BuildingBlocks.CQRS;
using Clients.API.Client.Persistence;
using FluentValidation;

namespace Clients.API.Client.UpdateClient;

public record UpdateClientCommand(
    Guid ClientId,
    string? Name,
    string? Email,
    string? CPF,
    DateTime? BirthDate)
    : ICommand<ErrorOr<UpdateClientResponse>>;

public record UpdateClientResponse(
    Guid ClientId,
    string Name,
    string Email,
    string CPF,
    DateTime BirthDate,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public class UpdateClientCommandHandler(IClientRepository repository)
    : ICommandHandler<UpdateClientCommand, ErrorOr<UpdateClientResponse>>
{
    public async Task<ErrorOr<UpdateClientResponse>> Handle(
        UpdateClientCommand command,
        CancellationToken cancellationToken)
    {
        var client = await repository.GetByIdAsync(command.ClientId, cancellationToken);

        if (client is null) 
            return Error.NotFound(description: $"Client with ID not found.");

        client.Name = command.Name ?? client.Name;
        client.CPF = command.CPF ?? client.CPF;
        client.Email = command.Email ?? client.Email;
        client.BirthDate = command.BirthDate ?? client.BirthDate;

        await repository.UpdateAsync(client, cancellationToken);

        return client.MapToUpdateClientResponse();
    }
}

public class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
{
    public UpdateClientCommandValidator()
    {
        //TODO - fix validator for update        
        
        RuleFor(x => x.Name)
            .MaximumLength(100);
        RuleFor(x => x.Email)
            .EmailAddress()
            .MaximumLength(50);
        RuleFor(x => x.CPF)
            .Matches(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$")
            .WithMessage("CPF must be in the format XXX.XXX.XXX-XX")
            .MaximumLength(14)
            .WithMessage("CPF must not exceed 14 characters.");
        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.Now)
            .WithMessage("BirthDate must be in the past.")
            .Must(BeAtLeast18YearsOld)
            .WithMessage("Not age legal.");
    }

    private static bool BeAtLeast18YearsOld(DateTime? birthDate)
    {
        if (birthDate is null) return false;
        var today = DateTime.Today;
        var age = today.Year - birthDate.Value.Year;

        if (birthDate.Value.Date > today.AddYears(-age))
        {
            age--;
        }

        return age >= 18;
    }
}