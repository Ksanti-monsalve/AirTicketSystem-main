// src/modules/person/Application/UseCases/DeletePersonPhoneUseCase.cs
using AirTicketSystem.modules.person.Domain.Repositories;

namespace AirTicketSystem.modules.person.Application.UseCases;

public sealed class DeletePersonPhoneUseCase
{
    private readonly IPersonPhoneRepository _repository;

    public DeletePersonPhoneUseCase(IPersonPhoneRepository repository)
        => _repository = repository;

    public async Task ExecuteAsync(int phoneId, CancellationToken cancellationToken = default)
    {
        if (phoneId <= 0)
            throw new ArgumentException("El ID del teléfono no es válido.");

        _ = await _repository.FindByIdAsync(phoneId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un teléfono con ID {phoneId}.");

        await _repository.DeleteAsync(phoneId);
    }
}
