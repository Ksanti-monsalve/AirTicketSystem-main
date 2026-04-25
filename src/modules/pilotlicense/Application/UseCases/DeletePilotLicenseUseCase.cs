// src/modules/pilotlicense/Application/UseCases/DeletePilotLicenseUseCase.cs
using AirTicketSystem.modules.pilotlicense.Domain.Repositories;

namespace AirTicketSystem.modules.pilotlicense.Application.UseCases;

public sealed class DeletePilotLicenseUseCase
{
    private readonly IPilotLicenseRepository _repository;

    public DeletePilotLicenseUseCase(IPilotLicenseRepository repository)
        => _repository = repository;

    public async Task ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        var licencia = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una licencia con ID {id}.");

        if (licencia.Activa.Valor)
            throw new InvalidOperationException(
                "No se puede eliminar una licencia activa. Suspéndala primero.");

        await _repository.DeleteAsync(id);
    }
}
