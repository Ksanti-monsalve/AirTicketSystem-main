// src/modules/pilotlicense/Application/UseCases/SuspendPilotLicenseUseCase.cs
using AirTicketSystem.modules.pilotlicense.Domain.Repositories;

namespace AirTicketSystem.modules.pilotlicense.Application.UseCases;

public sealed class SuspendPilotLicenseUseCase
{
    private readonly IPilotLicenseRepository _repository;

    public SuspendPilotLicenseUseCase(IPilotLicenseRepository repository)
        => _repository = repository;

    public async Task ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        var licencia = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una licencia con ID {id}.");

        licencia.Suspender();
        await _repository.UpdateAsync(licencia);
    }
}
