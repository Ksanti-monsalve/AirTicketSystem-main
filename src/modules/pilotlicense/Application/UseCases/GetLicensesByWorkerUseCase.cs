// src/modules/pilotlicense/Application/UseCases/GetLicensesByWorkerUseCase.cs
using AirTicketSystem.modules.pilotlicense.Domain.aggregate;
using AirTicketSystem.modules.pilotlicense.Domain.Repositories;

namespace AirTicketSystem.modules.pilotlicense.Application.UseCases;

public sealed class GetLicensesByWorkerUseCase
{
    private readonly IPilotLicenseRepository _repository;

    public GetLicensesByWorkerUseCase(IPilotLicenseRepository repository)
        => _repository = repository;

    public async Task<IReadOnlyCollection<PilotLicense>> ExecuteAsync(
        int trabajadorId, CancellationToken cancellationToken = default)
    {
        if (trabajadorId <= 0)
            throw new ArgumentException("El ID del trabajador no es válido.");

        return await _repository.FindByTrabajadorAsync(trabajadorId);
    }
}
