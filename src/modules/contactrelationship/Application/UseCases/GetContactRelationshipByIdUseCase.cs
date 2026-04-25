// src/modules/contactrelationship/Application/UseCases/GetContactRelationshipByIdUseCase.cs
using AirTicketSystem.modules.contactrelationship.Domain.aggregate;
using AirTicketSystem.modules.contactrelationship.Domain.Repositories;

namespace AirTicketSystem.modules.contactrelationship.Application.UseCases;

public sealed class GetContactRelationshipByIdUseCase
{
    private readonly IContactRelationshipRepository _repository;

    public GetContactRelationshipByIdUseCase(IContactRelationshipRepository repository)
    {
        _repository = repository;
    }

    public async Task<ContactRelationship> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una relación de contacto con ID {id}.");
    }
}
