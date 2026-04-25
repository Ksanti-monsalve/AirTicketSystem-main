// src/modules/pilotlicense/Application/UseCases/ReactivatePilotLicenseUseCase.cs
using AirTicketSystem.modules.pilotlicense.Domain.Repositories;

namespace AirTicketSystem.modules.pilotlicense.Application.UseCases;

public sealed class ReactivatePilotLicenseUseCase
{
    private readonly IPilotLicenseRepository _repository;

    public ReactivatePilotLicenseUseCase(IPilotLicenseRepository repository)
        => _repository = repository;

    public async Task ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        var licencia = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una licencia con ID {id}.");

        licencia.Reactivar();
        await _repository.UpdateAsync(licencia);
    }
}
