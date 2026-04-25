// src/modules/contactrelationship/Application/UseCases/GetAllContactRelationshipsUseCase.cs
using AirTicketSystem.modules.contactrelationship.Domain.aggregate;
using AirTicketSystem.modules.contactrelationship.Domain.Repositories;

namespace AirTicketSystem.modules.contactrelationship.Application.UseCases;

public sealed class GetAllContactRelationshipsUseCase
{
    private readonly IContactRelationshipRepository _repository;

    public GetAllContactRelationshipsUseCase(IContactRelationshipRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<ContactRelationship>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return await _repository.FindAllAsync();
    }
}
