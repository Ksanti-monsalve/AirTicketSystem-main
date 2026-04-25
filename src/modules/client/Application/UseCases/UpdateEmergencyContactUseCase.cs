// src/modules/client/Application/UseCases/UpdateEmergencyContactUseCase.cs
using AirTicketSystem.modules.client.Domain.aggregate;
using AirTicketSystem.modules.client.Domain.Repositories;
using AirTicketSystem.modules.contactrelationship.Domain.Repositories;

namespace AirTicketSystem.modules.client.Application.UseCases;

public sealed class UpdateEmergencyContactUseCase
{
    private readonly IEmergencyContactRepository    _repository;
    private readonly IContactRelationshipRepository _relationRepository;

    public UpdateEmergencyContactUseCase(
        IEmergencyContactRepository repository,
        IContactRelationshipRepository relationRepository)
    {
        _repository         = repository;
        _relationRepository = relationRepository;
    }

    public async Task<EmergencyContact> ExecuteAsync(
        int contactId,
        int relacionId,
        CancellationToken cancellationToken = default)
    {
        if (contactId <= 0)
            throw new ArgumentException("El ID del contacto de emergencia no es válido.");

        var contact = await _repository.FindByIdAsync(contactId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un contacto de emergencia con ID {contactId}.");

        _ = await _relationRepository.FindByIdAsync(relacionId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una relación de contacto con ID {relacionId}.");

        contact.ActualizarRelacion(relacionId);
        await _repository.UpdateAsync(contact);
        return contact;
    }
}
