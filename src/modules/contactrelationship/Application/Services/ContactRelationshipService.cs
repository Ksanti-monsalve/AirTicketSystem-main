// src/modules/contactrelationship/Application/Services/ContactRelationshipService.cs
using AirTicketSystem.modules.contactrelationship.Application.Interfaces;
using AirTicketSystem.modules.contactrelationship.Application.UseCases;
using AirTicketSystem.modules.contactrelationship.Domain.aggregate;

namespace AirTicketSystem.modules.contactrelationship.Application.Services;

public sealed class ContactRelationshipService : IContactRelationshipService
{
    private readonly CreateContactRelationshipUseCase _create;
    private readonly GetContactRelationshipByIdUseCase _getById;
    private readonly GetAllContactRelationshipsUseCase _getAll;
    private readonly UpdateContactRelationshipUseCase _update;
    private readonly DeleteContactRelationshipUseCase _delete;

    public ContactRelationshipService(
        CreateContactRelationshipUseCase create,
        GetContactRelationshipByIdUseCase getById,
        GetAllContactRelationshipsUseCase getAll,
        UpdateContactRelationshipUseCase update,
        DeleteContactRelationshipUseCase delete)
    {
        _create  = create;
        _getById = getById;
        _getAll  = getAll;
        _update  = update;
        _delete  = delete;
    }

    public Task<ContactRelationship> CreateAsync(string descripcion)
        => _create.ExecuteAsync(descripcion);

    public Task<ContactRelationship> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<IReadOnlyCollection<ContactRelationship>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<ContactRelationship> UpdateAsync(int id, string descripcion)
        => _update.ExecuteAsync(id, descripcion);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}
