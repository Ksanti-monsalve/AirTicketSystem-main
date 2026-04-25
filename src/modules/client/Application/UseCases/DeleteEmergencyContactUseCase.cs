// src/modules/client/Application/UseCases/DeleteEmergencyContactUseCase.cs
using AirTicketSystem.modules.client.Domain.Repositories;

namespace AirTicketSystem.modules.client.Application.UseCases;

public sealed class DeleteEmergencyContactUseCase
{
    private readonly IEmergencyContactRepository _repository;

    public DeleteEmergencyContactUseCase(IEmergencyContactRepository repository)
        => _repository = repository;

    public async Task ExecuteAsync(int contactId, CancellationToken cancellationToken = default)
    {
        if (contactId <= 0)
            throw new ArgumentException("El ID del contacto de emergencia no es válido.");

        _ = await _repository.FindByIdAsync(contactId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un contacto de emergencia con ID {contactId}.");

        await _repository.DeleteAsync(contactId);
    }
}
