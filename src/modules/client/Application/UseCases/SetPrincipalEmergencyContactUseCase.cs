// src/modules/client/Application/UseCases/SetPrincipalEmergencyContactUseCase.cs
using AirTicketSystem.modules.client.Domain.aggregate;
using AirTicketSystem.modules.client.Domain.Repositories;

namespace AirTicketSystem.modules.client.Application.UseCases;

public sealed class SetPrincipalEmergencyContactUseCase
{
    private readonly IEmergencyContactRepository _repository;

    public SetPrincipalEmergencyContactUseCase(IEmergencyContactRepository repository)
        => _repository = repository;

    public async Task<EmergencyContact> ExecuteAsync(
        int contactId, CancellationToken cancellationToken = default)
    {
        if (contactId <= 0)
            throw new ArgumentException("El ID del contacto de emergencia no es válido.");

        var contact = await _repository.FindByIdAsync(contactId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un contacto de emergencia con ID {contactId}.");

        await _repository.DesmarcarPrincipalByClienteAsync(contact.ClienteId);

        contact.MarcarComoPrincipal();
        await _repository.UpdateAsync(contact);
        return contact;
    }
}
