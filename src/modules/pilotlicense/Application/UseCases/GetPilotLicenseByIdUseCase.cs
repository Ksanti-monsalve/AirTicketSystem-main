// src/modules/pilotlicense/Application/UseCases/GetPilotLicenseByIdUseCase.cs
using AirTicketSystem.modules.pilotlicense.Domain.aggregate;
using AirTicketSystem.modules.pilotlicense.Domain.Repositories;

namespace AirTicketSystem.modules.pilotlicense.Application.UseCases;

public sealed class GetPilotLicenseByIdUseCase
{
    private readonly IPilotLicenseRepository _repository;

    public GetPilotLicenseByIdUseCase(IPilotLicenseRepository repository)
        => _repository = repository;

    public async Task<PilotLicense> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la licencia no es válido.");

        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una licencia con ID {id}.");
    }
}
