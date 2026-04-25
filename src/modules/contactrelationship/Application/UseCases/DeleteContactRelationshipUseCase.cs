// src/modules/contactrelationship/Application/UseCases/DeleteContactRelationshipUseCase.cs
using AirTicketSystem.modules.contactrelationship.Domain.Repositories;

namespace AirTicketSystem.modules.contactrelationship.Application.UseCases;

public sealed class DeleteContactRelationshipUseCase
{
    private readonly IContactRelationshipRepository _repository;

    public DeleteContactRelationshipUseCase(IContactRelationshipRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        _ = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una relación de contacto con ID {id}.");

        await _repository.DeleteAsync(id);
    }
}
