// src/modules/pilotlicense/Application/UseCases/RenewPilotLicenseUseCase.cs
using AirTicketSystem.modules.pilotlicense.Domain.aggregate;
using AirTicketSystem.modules.pilotlicense.Domain.Repositories;

namespace AirTicketSystem.modules.pilotlicense.Application.UseCases;

public sealed class RenewPilotLicenseUseCase
{
    private readonly IPilotLicenseRepository _repository;

    public RenewPilotLicenseUseCase(IPilotLicenseRepository repository)
        => _repository = repository;

    public async Task<PilotLicense> ExecuteAsync(
        int id, DateOnly nuevaFechaVencimiento, CancellationToken cancellationToken = default)
    {
        var licencia = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una licencia con ID {id}.");

        licencia.Renovar(nuevaFechaVencimiento);
        await _repository.UpdateAsync(licencia);
        return licencia;
    }
}
